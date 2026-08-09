using CarRental.API.DTOs.MaintenanceStatus;

namespace CarRental.API.Services.Interfaces
{
    public interface IMaintenanceStatusService
    {
        Task<List<MaintenanceStatusDTO>> GetAllAsync();

    }
}
