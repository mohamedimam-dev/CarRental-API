using System.ComponentModel.DataAnnotations;

namespace CarRental.API.DTOs.Maintenance
{
    public class AddMaintenanceDTO
    {
        [Required]
        public int VehicleId { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        public DateTime MaintenanceDate { get; set; }

        public DateOnly? ExpectedFinishDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }
    }
}
