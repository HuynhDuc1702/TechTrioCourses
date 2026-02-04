namespace AccountAPI.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlBody);
        Task SendOtpEmailAsync(string toEmail, string otp, string purpose);
    }
}


