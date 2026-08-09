namespace CarRental.API.DTOs.MaintenanceCompletion
{
    public class MaintenanceCompletionDTO
    {
        public int CompletionId { get; set; }

        public int MaintenanceId { get; set; }

        public DateOnly CompletedDate { get; set; }

        public int CreatedByUserId { get; set; }

        public decimal FinalCost { get; set; }

        public string? Notes { get; set; }

        public int VehicleMileage { get; set; }

        public bool IsPassedInspection { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedByUserId { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}