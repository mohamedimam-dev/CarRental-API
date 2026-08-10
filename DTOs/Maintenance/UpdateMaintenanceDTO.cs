using System.ComponentModel.DataAnnotations;

namespace CarRental.API.DTOs.Maintenance
{

    public class UpdateMaintenanceDTO
    {
        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        public DateTime MaintenanceDate { get; set; }

        public DateOnly? ExpectedFinishDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }


    }
}
