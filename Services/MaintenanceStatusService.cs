using CarRental.API.Data;
using CarRental.API.DTOs.MaintenanceStatus;
using CarRental.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class MaintenanceStatusService : IMaintenanceStatusService
    {
        private readonly CarRentalDbContext _context;

        public MaintenanceStatusService(CarRentalDbContext context)
        {
            _context = context;
        }


        public async Task<List<MaintenanceStatusDTO>> GetAllAsync()
        {
            return await _context.MaintenanceStatuses
                .AsNoTracking()
                .OrderBy(s => s.MaintenanceStatusId)
                .Select(s => new MaintenanceStatusDTO
                {
                    MaintenanceStatusId = s.MaintenanceStatusId,
                    StatusName = s.StatusName
                })
                .ToListAsync();
        }
    }
}
