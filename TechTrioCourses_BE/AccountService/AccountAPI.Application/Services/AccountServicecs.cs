using AccountAPI.Application.DTOs.Request;
using AccountAPI.Application.DTOs.Response;
using AccountAPI.Application.Interfaces;
using AccountAPI.Application.Interfaces.IExternalServices;
using AccountAPI.Application.Interfaces.ISecurity;
using AccountAPI.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;
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
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IOtpGenerator _otpGenerator;

        public AccountService(
           IAccountRepository accountRepo,
         IMapper mapper,
         IEmailService emailService,
     IConfiguration configuration,
      IUserApiClient userApiClient,
   IPasswordHasher passwordHasher,
         IJwtTokenService jwtTokenService,
      IOtpGenerator otpGenerator)
        {
            _accountRepo = accountRepo;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
            _userAPIClient = userApiClient;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _otpGenerator = otpGenerator;
        }

        public async Task<AuthResult?> LoginAsync(LoginRequest request)
        {
            var account = await _accountRepo.GetByEmailAsync(request.Email);

            if (account == null)
            {
                return null;
            }

            if (!_passwordHasher.VerifyPassword(request.Password, account.PasswordHash))
            {
                return null;
            }

            if (account.Status != AccountStatusEnum.Active)
            {
                return null;
            }

            var userResponse = await _userAPIClient.GetUserByAccountIdFromUserAPI(account.Id);

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

            var accessToken = _jwtTokenService.GenerateAccessToken(accountResponse, userResponse, jwtId);
            var refreshToken = _jwtTokenService.GenerateRefreshToken(account.Id, jwtId);

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
                var principal = _jwtTokenService.ValidateRefreshToken(request.RefreshToken);
                if (principal == null)
                {
                    return null;
                }

                var accountIdClaim = principal.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
                if (string.IsNullOrEmpty(accountIdClaim))
                {
                    return null;
                }

                var accountId = Guid.Parse(accountIdClaim);

                var account = await _accountRepo.GetByIdAsync(accountId);
                if (account == null || account.Status != AccountStatusEnum.Active)
                {
                    return null;
                }

                var userResponse = await _userAPIClient.GetUserByAccountIdFromUserAPI(account.Id);

                var newJwtId = Guid.NewGuid().ToString();
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

                var newAccessToken = _jwtTokenService.GenerateAccessToken(accountResponse, userResponse, newJwtId);
                var newRefreshToken = _jwtTokenService.GenerateRefreshToken(account.Id, newJwtId);

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
            var account = await _accountRepo.GetByEmailAsync(email);
            return _mapper.Map<AccountResponse>(account);
        }

        public async Task<AccountResponse?> RegisterAsync(RegisterRequest request)
        {
            if (await _accountRepo.EmailExistsAsync(request.Email))
            {
                return null;
            }

            var account = _mapper.Map<Account>(request);
            account.PasswordHash = _passwordHasher.HashPassword(request.Password);
            account.Status = AccountStatusEnum.Disable;

            var createdAccount = await _accountRepo.CreateAsync(account);

            var userResponse = await _userAPIClient.RegisterUser(createdAccount.Id, request);

            if (userResponse == null)
            {
                return null;
            }

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
            var account = await RegisterAsync(request);

            if (account == null)
            {
                return (null, string.Empty);
            }

            var otp = _otpGenerator.GenerateOtp();

            try
            {
                await _emailService.SendOtpEmailAsync(request.Email, otp, "Registration");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RegisterWithOtpAsync] Failed to send OTP email: {ex.Message}");
            }

            return (account, otp);
        }

        public async Task<bool> VerifyAndActivateAccountAsync(string email, string otp, string storedOtpData)
        {
            if (!_otpGenerator.ValidateOtpCookieData(storedOtpData, otp, out _))
            {
                return false;
            }

            var account = await _accountRepo.GetByEmailAsync(email);
            if (account == null)
            {
                return false;
            }

            account.Status = AccountStatusEnum.Active;
            await _accountRepo.UpdateAsync(account);
            return true;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var account = await _accountRepo.GetByEmailAsync(request.Email);

            if (account == null)
            {
                return false;
            }

            if (!_passwordHasher.VerifyPassword(request.OldPassword, account.PasswordHash))
            {
                return false;
            }

            account.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);

            await _accountRepo.UpdateAsync(account);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email, ResetPasswordRequest request)
        {
            var account = await _accountRepo.GetByEmailAsync(email);

            if (account == null)
            {
                return false;
            }

            account.PasswordHash = _passwordHasher.HashPassword(request.Password);

            await _accountRepo.UpdateAsync(account);
            return true;
        }



        public string CreateOtpCookieData(string otp, string purpose, int expirationMinutes = 10)
        {
            return _otpGenerator.CreateOtpCookieData(otp, purpose, expirationMinutes);
        }

        public string CreateSafeCookieName(string email)
        {
            return $"otp_{email.ToLower().Replace("@", "_").Replace(".", "_")}";
        }

        public async Task<string?> SendOtpAsync(string email, string purpose)
        {
            var otp = _otpGenerator.GenerateOtp();

            try
            {
                await _emailService.SendOtpEmailAsync(email, otp, purpose);
                return _otpGenerator.CreateOtpCookieData(otp, purpose);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SendOtpAsync] Failed to send OTP email: {ex.Message}");
                return null;
            }
        }
    }
}
