namespace CarRental.API.DTOs.RentalBookings
{
    public class RentalBookingListDTO
    {
        public int BookingId { get; set; }

        public int CustomerId { get; set; }

        public int VehicleId { get; set; }

        public DateOnly RentalStartDate { get; set; }

        public DateOnly RentalEndDate { get; set; }

        public byte InitialRentalDays { get; set; }

        public decimal InitialTotalDueAmount { get; set; }

        public int BookingStatusId { get; set; }
    }
}
