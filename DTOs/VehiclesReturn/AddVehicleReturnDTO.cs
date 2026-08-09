using System.ComponentModel.DataAnnotations;

namespace CarRental.API.DTOs.VehiclesReturn
{

    public class AddVehicleReturnDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int BookingId { get; set; }

        [Required]
        public DateTime ActualReturnDate { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Mileage { get; set; }

        [MaxLength(500)]
        public string? FinalCheckNotes { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal AdditionalCharges { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CreatedByUserId { get; set; }
    }
}
