using CarRental.API.Common;
using CarRental.API.Data;
using CarRental.API.DTOs.Maintenance;
using CarRental.API.Entities;
using CarRental.API.Enums;
using CarRental.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly CarRentalDbContext _context;

        public MaintenanceService(CarRentalDbContext context)
        {
            _context = context;
        }


        public async Task<ServiceResult<MaintenanceDTO>> AddAsync(
          AddMaintenanceDTO dto,
          int createdByUserId)
        {
            bool userExists = await _context.Users
                .AnyAsync(u => u.UserId == createdByUserId);

            if (!userExists)
                return ServiceResult<MaintenanceDTO>
                    .NotFound("User not found.");

            Vehicle? vehicle = await _context.Vehicles
                .FindAsync(dto.VehicleId);

            if (vehicle == null)
                return ServiceResult<MaintenanceDTO>
                    .NotFound("Vehicle not found.");

            if (!vehicle.IsAvailableForRent)
                return ServiceResult<MaintenanceDTO>
                    .Conflict("Vehicle is not available for maintenance.");

            bool maintenanceExists = await _context.Maintenances
                .AnyAsync(m =>
                    m.VehicleId == dto.VehicleId &&
                    m.MaintenanceStatusId == (int)enMaintenanceStatus.InProgress);

            if (maintenanceExists)
                return ServiceResult<MaintenanceDTO>
                    .Conflict("Vehicle already has a maintenance in progress.");

            Maintenance maintenance = new Maintenance
            {
                VehicleId = dto.VehicleId,
                Description = dto.Description,
                MaintenanceDate = dto.MaintenanceDate,
                ExpectedFinishDate = dto.ExpectedFinishDate,
                Cost = dto.Cost,
                MaintenanceStatusId = (int)enMaintenanceStatus.InProgress,
                CreatedByUserId = createdByUserId
            };

            _context.Maintenances.Add(maintenance);

            vehicle.IsAvailableForRent = false;

            await _context.SaveChangesAsync();

            MaintenanceDTO maintenanceDto = new MaintenanceDTO
            {
                MaintenanceId = maintenance.MaintenanceId,
                VehicleId = maintenance.VehicleId,
                Description = maintenance.Description,
                MaintenanceDate = maintenance.MaintenanceDate,
                ExpectedFinishDate = maintenance.ExpectedFinishDate,
                Cost = maintenance.Cost,
                MaintenanceStatusId = maintenance.MaintenanceStatusId,
                CreatedByUserId = maintenance.CreatedByUserId,
                UpdatedByUserId = maintenance.UpdatedByUserId,
                UpdatedDate = maintenance.UpdatedDate
            };

            return ServiceResult<MaintenanceDTO>.Success(maintenanceDto);
        }

        public async Task<ServiceResult<bool>> CancelAsync(
            int maintenanceId,
            int cancelledByUserId)
        {
            bool userExists = await _context.Users
                .AnyAsync(u => u.UserId == cancelledByUserId);

            if (!userExists)
                return ServiceResult<bool>
                    .NotFound("User not found.");

            Maintenance? maintenance = await _context.Maintenances
                .Include(m => m.Vehicle)
                .FirstOrDefaultAsync(m => m.MaintenanceId == maintenanceId);

            if (maintenance == null)
                return ServiceResult<bool>
                    .NotFound("Maintenance not found.");

            if (maintenance.MaintenanceStatusId != (int)enMaintenanceStatus.InProgress)
                return ServiceResult<bool>
                    .Conflict("Only maintenance in progress can be cancelled.");

            maintenance.MaintenanceStatusId = (int)enMaintenanceStatus.Cancelled;

            maintenance.UpdatedByUserId = cancelledByUserId;
            maintenance.UpdatedDate = DateTime.Now;

            maintenance.Vehicle.IsAvailableForRent = true;

            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }
      
        public async Task<List<MaintenanceListDTO>> GetAllAsync()
        {
            return await _context.Maintenances
                .AsNoTracking()
                .OrderByDescending(m => m.MaintenanceId)
                .Select(m => new MaintenanceListDTO
                {
                    MaintenanceId = m.MaintenanceId,
                    VehicleId = m.VehicleId,
                    MaintenanceDate = m.MaintenanceDate,
                    ExpectedFinishDate = m.ExpectedFinishDate,
                    Cost = m.Cost,
                    MaintenanceStatusId = m.MaintenanceStatusId
                })
                .ToListAsync();
        }

        public async Task<ServiceResult<MaintenanceDTO>> GetByIdAsync(int maintenanceId)
        {
            MaintenanceDTO? maintenance = await _context.Maintenances
                .AsNoTracking()
                .Where(m => m.MaintenanceId == maintenanceId)
                .Select(m => new MaintenanceDTO
                {
                    MaintenanceId = m.MaintenanceId,
                    VehicleId = m.VehicleId,
                    Description = m.Description,
                    MaintenanceDate = m.MaintenanceDate,
                    ExpectedFinishDate = m.ExpectedFinishDate,
                    Cost = m.Cost,
                    MaintenanceStatusId = m.MaintenanceStatusId,
                    CreatedByUserId = m.CreatedByUserId,
                    UpdatedByUserId = m.UpdatedByUserId,
                    UpdatedDate = m.UpdatedDate
                })
                .FirstOrDefaultAsync();

            if (maintenance == null)
                return ServiceResult<MaintenanceDTO>
                    .NotFound("Maintenance not found.");

            return ServiceResult<MaintenanceDTO>.Success(maintenance);
        }

        public async Task<ServiceResult<MaintenanceDTO>> UpdateAsync(
            int maintenanceId,
            UpdateMaintenanceDTO dto,
            int updatedByUserId)
        {
            Maintenance? maintenance = await _context.Maintenances
                .FindAsync(maintenanceId);

            if (maintenance == null)
                return ServiceResult<MaintenanceDTO>
                    .NotFound("Maintenance not found.");

            bool userExists = await _context.Users
                .AnyAsync(u => u.UserId == updatedByUserId);

            if (!userExists)
                return ServiceResult<MaintenanceDTO>
                    .NotFound("User not found.");

            maintenance.Description = dto.Description;
            maintenance.MaintenanceDate = dto.MaintenanceDate;
            maintenance.ExpectedFinishDate = dto.ExpectedFinishDate;
            maintenance.Cost = dto.Cost;

            maintenance.UpdatedByUserId = updatedByUserId;
            maintenance.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            MaintenanceDTO maintenanceDto = new MaintenanceDTO
            {
                MaintenanceId = maintenance.MaintenanceId,
                VehicleId = maintenance.VehicleId,
                Description = maintenance.Description,
                MaintenanceDate = maintenance.MaintenanceDate,
                ExpectedFinishDate = maintenance.ExpectedFinishDate,
                Cost = maintenance.Cost,
                MaintenanceStatusId = maintenance.MaintenanceStatusId,
                CreatedByUserId = maintenance.CreatedByUserId,
                UpdatedByUserId = maintenance.UpdatedByUserId,
                UpdatedDate = maintenance.UpdatedDate
            };

            return ServiceResult<MaintenanceDTO>.Success(maintenanceDto);
        }
    
    }
}
