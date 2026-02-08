using AccountAPI.Application.DTOs.Response;
using AccountAPI.Application.Interfaces.ISecurity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechTrioCourses.Shared.Dtos.User;
using TechTrioCourses.Shared.Enums;

namespace AccountAPI.Infrastructure.Security
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(AccountResponse account, UserResponseForLogin? user, string jwtId)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roleName = user != null ? ConvertRoleToEnumName(user.Role) : "Student";

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

        public string GenerateRefreshToken(Guid accountId, string jwtId)
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

        public ClaimsPrincipal? ValidateRefreshToken(string refreshToken)
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

        private string ConvertRoleToEnumName(UserRoleEnum role)
        {
            return role switch
            {
                UserRoleEnum.Admin => "Admin",
                UserRoleEnum.Instructor => "Instructor",
                UserRoleEnum.Student => "Student",
                _ => "Student"
            };
        }
    }
}
