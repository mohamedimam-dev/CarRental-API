namespace CarRental.API.DTOs.VehiclesReturn
{
    public class VehicleReturnListDTO
    {
        public int ReturnId { get; set; }

        public int BookingId { get; set; }

        public DateTime ActualReturnDate { get; set; }

        public decimal ActualTotalDueAmount { get; set; }
    }
}
