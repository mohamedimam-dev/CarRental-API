using System.ComponentModel.DataAnnotations;

namespace CarRental.API.DTOs.MaintenanceCompletion
{
    public class UpdateMaintenanceCompletionDTO
    {
        [Range(0, double.MaxValue)]
        public decimal FinalCost { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        [Range(0, int.MaxValue)]
        public int VehicleMileage { get; set; }

        [Required]
        public bool IsPassedInspection { get; set; }


    }
}