namespace CarRental.API.DTOs.Vehicles
{
    public class VehicleDTO
    {
        public int VehicleId { get; set; }

        public string Make { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string Vin { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public string EngineNumber { get; set; } = string.Empty;

        public int Year { get; set; }

        public int Mileage { get; set; }

        public int FuelTypeId { get; set; }

        public string PlateNumber { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public decimal RentalPricePerDay { get; set; }

        public bool IsAvailableForRent { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
