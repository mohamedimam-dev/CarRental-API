namespace CarRental.API.DTOs.Vehicles
{
    public class VehicleListDTO
    {
        public int VehicleId { get; set; }

        public string Make { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string PlateNumber { get; set; } = string.Empty;

        public decimal RentalPricePerDay { get; set; }

        public bool IsAvailableForRent { get; set; }
    }
}
