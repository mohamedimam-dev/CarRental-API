using System.ComponentModel.DataAnnotations;

namespace CarRental.API.DTOs.Maintenance
{
    public class CancelMaintenanceDTO
    {
        [Required]
        public int CancelledByUserId { get; set; }
    }
}
