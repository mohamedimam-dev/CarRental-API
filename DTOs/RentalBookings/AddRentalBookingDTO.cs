using System.ComponentModel.DataAnnotations;

namespace CarRental.API.DTOs.RentalBookings
{
    public class AddRentalBookingDTO
    {
        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }

        [Range(1, int.MaxValue)]
        public int VehicleId { get; set; }
       
        [Required]
        public DateOnly RentalStartDate { get; set; }

        [Required]
        public DateOnly RentalEndDate { get; set; }

        [StringLength(100)]
        public string? PickupLocation { get; set; }

        [StringLength(100)]
        public string? DropoffLocation { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal RentalPricePerDay { get; set; }

        [StringLength(500)]
        public string? InitialCheckNotes { get; set; }


    }
}
