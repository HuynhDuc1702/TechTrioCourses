using AccountAPI.DTOs.Request;
using AccountAPI.DTOs.Response;
using AccountAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;

        public AccountsController(IAccountService accountService, IEmailService emailService)
        {
            _accountService = accountService;
            _emailService = emailService;
        }

        // POST: api/Accounts/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResult>> Login([FromBody] LoginRequest request)
        {
            var result = await _accountService.LoginAsync(request);

            if (result == null)
            {
                var user = await _accountService.GetUserByEmailAsync(request.Email);
                if (user != null && user.Status == AccountAPI.Enums.AccountStatusEnum.Banned)
                {
                    return Unauthorized(new { message = "Your account has been banned please contact Admin for help" });
                }
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(result);
        }

        // POST: api/Accounts/refresh-token
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResult>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _accountService.RefreshTokenAsync(request);

            if (result == null)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token" });
            }

            return Ok(result);
        }

        // POST: api/Accounts/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AccountResponse>> Register([FromBody] RegisterRequest request)
        {
            // Register account and generate OTP
            var (account, otp) = await _accountService.RegisterWithOtpAsync(request);

            if (account == null)
            {
                return BadRequest(new { message = "Email already exists" });
            }

            // Create OTP cookie data
            var otpData = _accountService.CreateOtpCookieData(otp, "Registration");

            // Store OTP in secure HTTP-only cookie
            var safeCookieName = CreateSafeCookieName(request.Email);
            Response.Cookies.Append(safeCookieName, otpData, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(10)
            });

            return Ok(new
            {
                account = account,
                message = "Registration successful. Please check your email for OTP verification."
            });
        }

        // POST: api/Accounts/send-otp
        [HttpPost("send-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
        {

            // Generate OTP
            var otp = _accountService.GenerateOtp();
            await _emailService.SendOtpEmailAsync(request.Email, otp, request.Purpose);

            // Create OTP cookie data
            var otpData = _accountService.CreateOtpCookieData(otp, request.Purpose);

            // Store OTP in secure HTTP-only cookie
            var safeCookieName = CreateSafeCookieName(request.Email);
            Response.Cookies.Append(safeCookieName, otpData, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(10)
            });

            return Ok(new OtpResponse
            {
                Message = "OTP sent successfully to your email",
                ExpiresAt = DateTime.UtcNow.AddMinutes(10)
            });
        }

        // POST: api/Accounts/verify-otp
        [HttpPost("verify-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            // Retrieve OTP from cookie
            var safeCookieName = CreateSafeCookieName(request.Email);
            if (!Request.Cookies.TryGetValue(safeCookieName, out var otpData))
            {
                return BadRequest(new { message = "OTP not found or expired" });
            }

            // Verify OTP and activate account
            var isValid = await _accountService.VerifyAndActivateAccountAsync(request.Email, request.Code, otpData);

            if (!isValid)
            {
                return BadRequest(new { message = "Invalid or expired OTP" });
            }

            // Delete the OTP cookie after successful verification
            Response.Cookies.Delete(safeCookieName);

            return Ok(new { message = "OTP verified successfully" });
        }

        // POST: api/Accounts/change-password
        [HttpPost("change-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _accountService.ChangePasswordAsync(request);

            if (!result)
            {
                return BadRequest(new { message = "Invalid email or old password" });
            }

            return Ok(new { message = "Password changed successfully" });
        }

        // POST: api/Accounts/reset-password
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            // Reset password
            var result = await _accountService.ResetPasswordAsync(request.Email, request);

            if (!result)
            {
                return BadRequest(new { message = "Failed to reset password. Account not found." });
            }

            return Ok(new { message = "Password reset successfully" });
        }

        // Helper method to create safe cookie names
        private string CreateSafeCookieName(string email)
        {
            return $"otp_{email.ToLower().Replace("@", "_").Replace(".", "_")}";
        }
    }
}
