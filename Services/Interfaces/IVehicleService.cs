using CarRental.API.Common;
using CarRental.API.DTOs.Vehicles;

namespace CarRental.API.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<ServiceResult<VehicleDTO>> AddAsync(
            AddVehicleDTO dto,
            int createdByUserId);

        Task<ServiceResult<VehicleDTO>> GetByIdAsync(int vehicleId);
       
        Task<List<VehicleListDTO>> GetAllAsync();

        Task<ServiceResult<VehicleDTO>> UpdateAsync(int vehicleId, UpdateVehicleDTO dto);
       
        Task<ServiceResult<bool>> DeleteAsync(int vehicleId);
    }
}
