using CarRental.API.Common;
using CarRental.API.DTOs.MaintenanceCompletion;

namespace CarRental.API.Services.Interfaces
{
    public interface IMaintenanceCompletionService
    {

        Task<ServiceResult<MaintenanceCompletionDTO>> AddAsync(
            AddMaintenanceCompletionDTO dto);

        Task<ServiceResult<MaintenanceCompletionDTO>> UpdateAsync(
            int completionId,
            UpdateMaintenanceCompletionDTO dto);
      
        Task<ServiceResult<MaintenanceCompletionDTO>> GetByIdAsync(
            int completionId);
       
        Task<List<MaintenanceCompletionListDTO>> GetAllAsync();
    }
}
