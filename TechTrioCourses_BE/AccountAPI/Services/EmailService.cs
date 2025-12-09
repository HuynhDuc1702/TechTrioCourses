using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using AccountAPI.Services.Interfaces;

namespace AccountAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var host = _config["Smtp:Host"];
            var port = int.Parse(_config["Smtp:Port"] ?? "587");
            var user = _config["Smtp:User"];
            var pass = _config["Smtp:Pass"];
            var from = _config["Smtp:From"] ?? user;
            var enableSsl = bool.Parse(_config["Smtp:Ssl"] ?? "true");

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = enableSsl,
            };

            using var msg = new MailMessage(from!, toEmail)
            {
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            await client.SendMailAsync(msg);
        }

        public async Task SendOtpEmailAsync(string toEmail, string otp, string purpose)
        {
            var subject = GetOtpEmailSubject(purpose);
            var body = GenerateOtpEmailBody(otp, purpose);
            await SendEmailAsync(toEmail, subject, body);
        }

        private string GetOtpEmailSubject(string purpose)
        {
            return purpose switch
            {
                "Registration" => "Verify Your Email - Registration OTP",
                "PasswordReset" => "Password Reset - OTP Code",
                "EmailVerification" => "Email Verification - OTP Code",
                _ => "Your OTP Code"
            };
        }

        private string GenerateOtpEmailBody(string otp, string purpose)
        {
            var purposeText = purpose switch
            {
                "Registration" => "complete your registration",
                "PasswordReset" => "reset your password",
                "EmailVerification" => "verify your email",
                _ => "verify your action"
            };

            return $@"
<html>
<body style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
    <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
        <h2 style='color: #333; text-align: center;'>Verification Code</h2>
        <p style='color: #666; font-size: 16px;'>Please use the following code to {purposeText}:</p>
        <div style='background-color: #f8f9fa; padding: 20px; border-radius: 5px; text-align: center; margin: 20px 0;'>
            <h1 style='color: #007bff; letter-spacing: 8px; margin: 0; font-size: 36px;'>{otp}</h1>
        </div>
        <p style='color: #666; font-size: 14px;'>This code will expire in <strong>10 minutes</strong>.</p>
        <p style='color: #999; font-size: 12px; margin-top: 30px; border-top: 1px solid #eee; padding-top: 20px;'>
          If you didn't request this code, please ignore this email.
        </p>
    </div>
</body>
</html>";
        }
    }
}


