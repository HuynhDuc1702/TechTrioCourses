using AccountAPI.Application.Interfaces.ISecurity;

namespace AccountAPI.Infrastructure.Security
{


    public class OtpGenerator : IOtpGenerator
    {
        public string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        public string CreateOtpCookieData(string otp, string purpose, int expirationMinutes = 10)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);
            return $"{otp}|{expiresAt:O}|{purpose}";
        }

        public bool ValidateOtpCookieData(string storedOtpData, string otp, out string purpose)
        {
            purpose = string.Empty;
            var parts = storedOtpData.Split('|');

            if (parts.Length != 3)
            {
                return false;
            }

            var storedOtp = parts[0];
            var expiresAt = DateTime.Parse(parts[1]);
            purpose = parts[2];

            return storedOtp == otp && DateTime.UtcNow <= expiresAt;
        }
    }
}
