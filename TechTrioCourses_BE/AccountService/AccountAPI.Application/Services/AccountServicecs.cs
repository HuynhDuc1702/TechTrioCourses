using AccountAPI.Application.DTOs.Request;
using AccountAPI.Application.DTOs.Response;
using AccountAPI.Application.Interfaces;
using AccountAPI.Application.Interfaces.IExternalServices;
using AccountAPI.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TechTrioCourses.Shared.Dtos.User;
using TechTrioCourses.Shared.Enums;

namespace AccountAPI.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IUserApiClient _userAPIClient;

        public AccountService(IAccountRepository accountRepo, 
            IMapper mapper, 
            IEmailService emailService, 
            IHttpClientFactory httpClientFactory, 
            IConfiguration configuration,
            IUserApiClient userApiClient)
        {
            _accountRepo = accountRepo;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
            _userAPIClient=userApiClient;
        }

        public async Task<AuthResult?> LoginAsync(LoginRequest request)
        {
            // Get account by email
            var account = await _accountRepo.GetByEmailAsync(request.Email);

            if (account == null)
            {
                return null;
            }

            // Verify password
            if (!VerifyPassword(request.Password, account.PasswordHash))
            {
                return null;
            }

            // Check if account is active
            if (account.Status != AccountStatusEnum.Active)
            {
                return null;
            }

            // Get user data from UserAPI
            var userResponse = await _userAPIClient.GetUserByAccountIdFromUserAPI(account.Id);

            // Generate tokens
            var jwtId = Guid.NewGuid().ToString();
            var accountResponse = new AccountResponse
            {
                Id = account.Id,
                UserId = userResponse.Id,
                FullName = userResponse.FullName,
                AvatarUrl = userResponse.AvatarUrl,
                Email = account.Email,
                Role = userResponse.Role,
                Status = account.Status,
                CreatedAt = account.CreatedAt ?? DateTime.UtcNow,


            };
            var accessToken = GenerateJwtToken(accountResponse, userResponse, jwtId);
            var refreshToken = GenerateRefreshToken(account.Id, jwtId);

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var accessTokenDuration = double.Parse(jwtSettings["DurationInMinutes"]!);
            var refreshTokenDuration = int.Parse(jwtSettings["RefreshTokenDurationInDays"]!);

            return new AuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenDuration),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(refreshTokenDuration)
            };
        }

        public async Task<AuthResult?> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                // Validate refresh token
                var principal = ValidateRefreshToken(request.RefreshToken);
                if (principal == null)
                {
                    return null;
                }

                // Extract account ID from refresh token
                var accountIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if (string.IsNullOrEmpty(accountIdClaim))
                {
                    return null;
                }

                var accountId = Guid.Parse(accountIdClaim);

                // Get account from database
                var account = await _accountRepo.GetByIdAsync(accountId);
                if (account == null || account.Status != AccountStatusEnum.Active)
                {
                    return null;
                }

                // Get user data from UserAPI
                var userResponse = await _userAPIClient.GetUserByAccountIdFromUserAPI(account.Id);

                // Generate new tokens
                var newJwtId = Guid.NewGuid().ToString();
                var accountResponse = new AccountResponse
                {
                    Id = account.Id,
                   UserId=userResponse.Id,
                   FullName=userResponse.FullName,
                   AvatarUrl=userResponse.AvatarUrl,
                   Email=account.Email,
                   Role=userResponse.Role,
                   Status=account.Status,
                   CreatedAt=account.CreatedAt ?? DateTime.UtcNow,

                };
                var newAccessToken = GenerateJwtToken(accountResponse, userResponse, newJwtId);
                var newRefreshToken = GenerateRefreshToken(account.Id, newJwtId);

                var jwtSettings = _configuration.GetSection("JwtSettings");
                var accessTokenDuration = double.Parse(jwtSettings["DurationInMinutes"]!);
                var refreshTokenDuration = int.Parse(jwtSettings["RefreshTokenDurationInDays"]!);

                return new AuthResult
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenDuration),
                    RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(refreshTokenDuration)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RefreshTokenAsync] Error: {ex.Message}");
                return null;
            }
        }

        public async Task<AccountResponse?> GetUserByEmailAsync(string email)
        {
             var account=await _accountRepo.GetByEmailAsync(email);
            return _mapper.Map<AccountResponse>(account);
        }

        public async Task<AccountResponse?> RegisterAsync(RegisterRequest request)
        {
            // Check if email already exists
            if (await _accountRepo.EmailExistsAsync(request.Email))
            {
                return null;
            }

            // Create Account with Disable status (will be activated after OTP verification)
            var account = _mapper.Map<Account>(request);
            account.PasswordHash = HashPassword(request.Password);
            account.Status = AccountStatusEnum.Disable;

            var createdAccount = await _accountRepo.CreateAccountAsync(account);

            // Create User via UserAPI
            var userResponse = await _userAPIClient.RegisterUser(createdAccount.Id, request);

            if (userResponse == null)
            {
                // If user creation fails, we should rollback account creation
                // For simplicity, we'll just return null
                return null;
            }

            // Map to response
            return new AccountResponse
            {
                Id = createdAccount.Id,
                UserId = userResponse.Id,
                Email = createdAccount.Email,
                FullName = userResponse.FullName,
                AvatarUrl = userResponse.AvatarUrl,
                Role = userResponse.Role,
                CreatedAt = createdAccount.CreatedAt ?? DateTime.UtcNow
            };
        }

        public async Task<(AccountResponse? account, string otp)> RegisterWithOtpAsync(RegisterRequest request)
        {
            // Register the account
            var account = await RegisterAsync(request);

            if (account == null)
            {
                return (null, string.Empty);
            }

            // Generate OTP
            var otp = GenerateOtp();

            // Send OTP email
            try
            {
                await _emailService.SendOtpEmailAsync(request.Email, otp, "Registration");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RegisterWithOtpAsync] Failed to send OTP email: {ex.Message}");
                // Continue anyway - account is created
            }

            return (account, otp);
        }

        public async Task<bool> VerifyAndActivateAccountAsync(string email, string otp, string storedOtpData)
        {
            // Parse stored OTP data from cookie (format: "otp|expiresAt|purpose")
            var parts = storedOtpData.Split('|');
            if (parts.Length != 3)
            {
                return false;
            }

            var storedOtp = parts[0];
            var expiresAt = DateTime.Parse(parts[1]);

            // Check if OTP matches and hasn't expired
            if (storedOtp != otp || DateTime.UtcNow > expiresAt)
            {
                return false;
            }

            // Activate the account
            var account = await _accountRepo.GetByEmailAsync(email);
            if (account == null)
            {
                return false;
            }

            account.Status = AccountStatusEnum.Active;
            return await _accountRepo.UpdateAccountAsync(account);
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
        {
            // Get account by email
            var account = await _accountRepo.GetByEmailAsync(request.Email);

            if (account == null)
            {
                return false;
            }

            // Verify old password
            if (!VerifyPassword(request.OldPassword, account.PasswordHash))
            {
                return false;
            }

            // Update password
            account.PasswordHash = HashPassword(request.NewPassword);

            return await _accountRepo.UpdateAccountAsync(account);
        }

        public async Task<bool> ResetPasswordAsync(string email, ResetPasswordRequest request)
        {
            // Get account by email
            var account = await _accountRepo.GetByEmailAsync(email);

            if (account == null)
            {
                return false;
            }

            // Update password directly (no old password verification needed for reset)
            account.PasswordHash = HashPassword(request.Password);

            return await _accountRepo.UpdateAccountAsync(account);
        }

        // Helper Methods
        public string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        public string CreateOtpCookieData(string otp, string purpose, int expirationMinutes = 10)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);
            return $"{otp}|{expiresAt:O}|{purpose}";
        }

        
        private string HashPassword(string password)
        {
            
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

           
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

           
            byte[] hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            
            return Convert.ToBase64String(hashBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

           
            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }

        
     

     
        private string GenerateRefreshToken(Guid accountId, string jwtId)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
               {
             new Claim(JwtRegisteredClaimNames.Sub, accountId.ToString()),
             new Claim(JwtRegisteredClaimNames.Jti, jwtId),
             new Claim("token_type", "refresh")
            };

            var refreshTokenDuration = int.Parse(jwtSettings["RefreshTokenDurationInDays"]!);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(refreshTokenDuration),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       
        private ClaimsPrincipal? ValidateRefreshToken(string refreshToken)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out var validatedToken);

              
                var tokenType = principal.FindFirst("token_type")?.Value;
                if (tokenType != "refresh")
                {
                    return null;
                }

             
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        return null;
                    }
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private string GenerateJwtToken(AccountResponse account, UserResponseForLogin? user, string jwtId)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

          
            var roleName = user != null ? _userAPIClient.ConvertRoleToEnumName(user.Role) : "Student";

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, account.Email),
            new Claim(ClaimTypes.Role, roleName),
            new Claim(JwtRegisteredClaimNames.Jti, jwtId),
            new Claim("token_type", "access")
             };

            var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
           expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"]!)),
           signingCredentials: credentials
           );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
