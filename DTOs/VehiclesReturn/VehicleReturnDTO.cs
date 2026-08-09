namespace CarRental.API.DTOs.VehiclesReturn
{
    public class VehicleReturnDTO
    {
        public int ReturnId { get; set; }

        public int BookingId { get; set; }

        public DateTime ActualReturnDate { get; set; }

        public byte ActualRentalDays { get; set; }

        public int Mileage { get; set; }

        public int ConsumedMileage { get; set; }

        public string? FinalCheckNotes { get; set; }

        public decimal AdditionalCharges { get; set; }

        public decimal ActualTotalDueAmount { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
