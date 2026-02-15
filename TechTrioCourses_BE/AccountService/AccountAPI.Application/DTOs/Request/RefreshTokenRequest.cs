using System.ComponentModel.DataAnnotations;

namespace AccountAPI.Application.DTOs.Request
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}
