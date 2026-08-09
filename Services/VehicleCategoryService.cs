using CarRental.API.Data;
using CarRental.API.DTOs.VehicleCategory;
using CarRental.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class VehicleCategoryService : IVehicleCategoryService
    {
        private readonly CarRentalDbContext _context;

        public VehicleCategoryService(CarRentalDbContext context)
        {
            _context = context;
        }


        public async Task<List<VehicleCategoryDTO>> GetAllAsync()
        {
            return await _context.VehicleCategories
                .AsNoTracking()
                .OrderBy(c => c.CategoryName)
                .Select(c => new VehicleCategoryDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                })
                .ToListAsync();
        }
    }
}
