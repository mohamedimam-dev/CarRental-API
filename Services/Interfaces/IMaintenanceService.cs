using CarRental.API.Common;
using CarRental.API.DTOs.Maintenance;

namespace CarRental.API.Services.Interfaces
{
    public interface IMaintenanceService
    {
        Task<ServiceResult<MaintenanceDTO>> AddAsync(AddMaintenanceDTO dto);

        Task<ServiceResult<MaintenanceDTO>> UpdateAsync(
            int maintenanceId,
            UpdateMaintenanceDTO dto);
     
        Task<ServiceResult<bool>> CancelAsync(
            int maintenanceId,
            CancelMaintenanceDTO dto);

        Task<ServiceResult<MaintenanceDTO>> GetByIdAsync(int maintenanceId);
       
        Task<List<MaintenanceListDTO>> GetAllAsync();
    }
}
