using System.ComponentModel.DataAnnotations;

namespace CarRental.API.DTOs.Customers
{
    public class AddCustomerDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ContactInformation { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string DriverLicenseNumber { get; set; } = string.Empty;


    }
}
