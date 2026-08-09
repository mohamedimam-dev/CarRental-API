using System.ComponentModel.DataAnnotations;

namespace CarRental.API.DTOs.Users
{
    public class UpdateUserDTO
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [Required]
        public int RoleId { get; set; }

        public bool IsActive { get; set; }
    }
}
