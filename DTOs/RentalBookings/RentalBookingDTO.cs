namespace CarRental.API.DTOs.RentalBookings
{
    namespace CarRental.API.DTOs.RentalBookings
    {
        public class RentalBookingDTO
        {
            public int BookingId { get; set; }

            public int CustomerId { get; set; }

            public int VehicleId { get; set; }

            public DateOnly RentalStartDate { get; set; }

            public DateOnly RentalEndDate { get; set; }

            public string? PickupLocation { get; set; }

            public string? DropoffLocation { get; set; }

            public byte InitialRentalDays { get; set; }

            public decimal RentalPricePerDay { get; set; }

            public decimal InitialTotalDueAmount { get; set; }

            public string? InitialCheckNotes { get; set; }

            public int BookingStatusId { get; set; }

            public int CreatedByUserId { get; set; }

            public DateTime CreatedDate { get; set; }
        }
    }
}
