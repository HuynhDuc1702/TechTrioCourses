using System.ComponentModel.DataAnnotations;

namespace AccountAPI.DTOs.Request
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}
