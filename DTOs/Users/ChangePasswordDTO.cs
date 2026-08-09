using System.ComponentModel.DataAnnotations;

namespace CarRental.API.DTOs.Users
{
    public class ChangePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
