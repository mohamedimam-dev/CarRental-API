using System.ComponentModel.DataAnnotations;

namespace CarRental.API.DTOs.Auth
{
    public class RefreshRequestDTO
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;
    }
}
