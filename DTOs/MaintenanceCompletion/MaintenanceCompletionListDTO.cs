namespace CarRental.API.DTOs.MaintenanceCompletion
{
    public class MaintenanceCompletionListDTO
    {
        public int CompletionId { get; set; }

        public int MaintenanceId { get; set; }

        public DateOnly CompletedDate { get; set; }

        public decimal FinalCost { get; set; }

        public bool IsPassedInspection { get; set; }
    }
}