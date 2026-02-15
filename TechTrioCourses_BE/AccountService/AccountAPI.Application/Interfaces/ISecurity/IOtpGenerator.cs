namespace AccountAPI.Application.Interfaces.ISecurity
{
    public interface IOtpGenerator
    {
        string GenerateOtp();
        string CreateOtpCookieData(string otp, string purpose, int expirationMinutes = 10);
        bool ValidateOtpCookieData(string storedOtpData, string otp, out string purpose);
    }
}
