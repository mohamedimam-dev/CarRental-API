using CarRental.API.Data;
using CarRental.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using CarRental.API.DTOs.FuelType;

namespace CarRental.API.Services
{
    public class FuelTypeService : IFuelTypeService
    {
        private readonly CarRentalDbContext _context;

        public FuelTypeService(CarRentalDbContext context)
        {
            _context = context;
        }


        public async Task<List<FuelTypeDTO>> GetAllAsync()
        {
            return await _context.FuelTypes
                .AsNoTracking()
                .OrderBy(f => f.FuelType1)
                .Select(f => new FuelTypeDTO
                {
                    FuelTypeId = f.FuelTypeId,
                    FuelType = f.FuelType1
                })
                .ToListAsync();
        }
    }
}
