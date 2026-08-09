using System.ComponentModel.DataAnnotations;

namespace CarRental.API.DTOs.Auth
{
    public class TokenResponseDTO
    {
        [Required]
        public string AccessToken { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
