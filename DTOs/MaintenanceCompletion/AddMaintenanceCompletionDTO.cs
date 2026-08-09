using System.ComponentModel.DataAnnotations;

namespace CarRental.API.DTOs.MaintenanceCompletion
{
    public class AddMaintenanceCompletionDTO
    {
        [Required]
        public int MaintenanceId { get; set; }

        [Required]
        public DateOnly CompletedDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal FinalCost { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        [Range(0, int.MaxValue)]
        public int VehicleMileage { get; set; }

        [Required]
        public bool IsPassedInspection { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }
    }
}