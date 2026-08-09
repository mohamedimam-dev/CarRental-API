using System.ComponentModel.DataAnnotations;

namespace CarRental.API.DTOs.RentalBookings
{
    public class CancelRentalBookingDTO
    {
        [Required]
        public int CancelledByUserId { get; set; }
    }
}
