namespace CarRental.API.DTOs.Customers
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }

        public string Name { get; set; } = null!;

        public string ContactInformation { get; set; } = null!;

        public string DriverLicenseNumber { get; set; } = null!;

        public int CreatedByUserId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
