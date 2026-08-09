namespace CarRental.API.DTOs.Maintenance
{
    public class MaintenanceListDTO
    {
        public int MaintenanceId { get; set; }

        public int VehicleId { get; set; }

        public DateTime MaintenanceDate { get; set; }

        public DateOnly? ExpectedFinishDate { get; set; }

        public decimal Cost { get; set; }

        public int MaintenanceStatusId { get; set; }
    }
}