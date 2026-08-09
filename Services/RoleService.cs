using CarRental.API.Data;
using CarRental.API.DTOs.Roles;
using CarRental.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class RoleService : IRoleService
    {

        private readonly CarRentalDbContext _context;

        public RoleService(CarRentalDbContext context)
        {
            _context = context;
        }


        public async Task<List<RoleDTO>> GetAllAsync()
        {
            return await _context.Roles
                .AsNoTracking()
                .OrderBy(r => r.RoleId)
                .Select(r => new RoleDTO
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    Description = r.Description
                })
                .ToListAsync();
        }
    }
}
