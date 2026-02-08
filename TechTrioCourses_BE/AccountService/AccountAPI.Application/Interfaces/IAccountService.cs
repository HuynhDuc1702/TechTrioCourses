using AccountAPI.Application.DTOs.Request;
using AccountAPI.Application.DTOs.Response;


namespace AccountAPI.Application.Interfaces
{
    public interface IAccountService
    {
        Task<AuthResult?> LoginAsync(LoginRequest request);
        Task<AccountResponse?> RegisterAsync(RegisterRequest request);
        Task<bool> ChangePasswordAsync(ChangePasswordRequest request);
        Task<bool> ResetPasswordAsync(string email, ResetPasswordRequest request);
        Task<(AccountResponse? account, string otp)> RegisterWithOtpAsync(RegisterRequest request);
        Task<bool> VerifyAndActivateAccountAsync(string email, string otp, string storedOtpData);
        string CreateOtpCookieData(string otp, string purpose, int expirationMinutes = 10);
        Task<AccountResponse?> GetUserByEmailAsync(string email);
        string GenerateOtp();
        Task<AuthResult?> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
