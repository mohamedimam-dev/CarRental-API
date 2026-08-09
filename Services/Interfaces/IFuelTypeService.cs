using CarRental.API.DTOs.FuelType;
namespace CarRental.API.Services.Interfaces
{
    public interface IFuelTypeService
    {
        Task<List<FuelTypeDTO>> GetAllAsync();

    }
}
