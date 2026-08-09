namespace CarRental.API.DTOs.Customers
{
    public class CustomerListDTO
    {
        public int CustomerId { get; set; }

        public string Name { get; set; } = null!;

        public string ContactInformation { get; set; } = null!;

        public string DriverLicenseNumber { get; set; } = null!;
    }
}
