using CarRental.API.Common;
using CarRental.API.DTOs.VehiclesReturn;

namespace CarRental.API.Services.Interfaces
{
    public interface IVehicleReturnService
    {
        Task<ServiceResult<VehicleReturnDTO>> AddAsync(AddVehicleReturnDTO dto);

        Task<ServiceResult<VehicleReturnDTO>> GetByIdAsync(int returnId);

        Task<ServiceResult<VehicleReturnDTO>> GetByBookingIdAsync(int bookingId);
      
        Task<List<VehicleReturnListDTO>> GetAllAsync();
    }
}
