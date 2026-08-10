using CarRental.API.Common;
using CarRental.API.DTOs.Maintenance;

namespace CarRental.API.Services.Interfaces
{
    public interface IMaintenanceService
    {
        Task<ServiceResult<MaintenanceDTO>> AddAsync(
            AddMaintenanceDTO dto,
            int createdByUserId);
      
        Task<ServiceResult<MaintenanceDTO>> UpdateAsync(
            int maintenanceId,
            UpdateMaintenanceDTO dto,
            int updatedByUserId);

        Task<ServiceResult<bool>> CancelAsync(
            int maintenanceId,
            int cancelledByUserId);

        Task<ServiceResult<MaintenanceDTO>> GetByIdAsync(int maintenanceId);
       
        Task<List<MaintenanceListDTO>> GetAllAsync();
    }
}
