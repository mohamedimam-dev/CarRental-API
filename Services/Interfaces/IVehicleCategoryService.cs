using CarRental.API.DTOs.VehicleCategory;

namespace CarRental.API.Services.Interfaces
{
    public interface IVehicleCategoryService
    {
        Task<List<VehicleCategoryDTO>> GetAllAsync();

    }
}
