using System.ComponentModel.DataAnnotations;

namespace CarRental.API.DTOs.Vehicles
{
    public class AddVehicleDTO
    {
        [Required]
        [StringLength(50)]
        public string Make { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Vin { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Color { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string EngineNumber { get; set; } = string.Empty;

        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Mileage { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int FuelTypeId { get; set; }

        [Required]
        [StringLength(20)]
        public string PlateNumber { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal RentalPricePerDay { get; set; }

        [Required]
        public bool IsAvailableForRent { get; set; }

    }
}
