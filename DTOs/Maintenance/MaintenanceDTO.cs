namespace CarRental.API.DTOs.Maintenance
{

    public class MaintenanceDTO
    {
        public int MaintenanceId { get; set; }

        public int VehicleId { get; set; }

        public string? Description { get; set; }

        public DateTime MaintenanceDate { get; set; }

        public DateOnly? ExpectedFinishDate { get; set; }

        public decimal Cost { get; set; }

        public int MaintenanceStatusId { get; set; }

        public int CreatedByUserId { get; set; }

        public int? UpdatedByUserId { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
