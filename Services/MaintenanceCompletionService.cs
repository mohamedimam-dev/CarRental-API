using CarRental.API.Common;
using CarRental.API.Data;
using CarRental.API.DTOs.MaintenanceCompletion;
using CarRental.API.Entities;
using CarRental.API.Enums;
using CarRental.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class MaintenanceCompletionService : IMaintenanceCompletionService
    {
        private readonly CarRentalDbContext _context;

        public MaintenanceCompletionService(CarRentalDbContext context)
        {
            _context = context;
        }


        public async Task<ServiceResult<MaintenanceCompletionDTO>> AddAsync(
          AddMaintenanceCompletionDTO dto,
          int createdByUserId)
        {
            bool userExists = await _context.Users
               .AnyAsync(u => u.UserId == createdByUserId);

            if (!userExists)
                return ServiceResult<MaintenanceCompletionDTO>
                    .NotFound("User not found.");

            Maintenance? maintenance = await _context.Maintenances
                .Include(m => m.Vehicle)
                .FirstOrDefaultAsync(m => m.MaintenanceId == dto.MaintenanceId);

            if (maintenance == null)
                return ServiceResult<MaintenanceCompletionDTO>
                    .NotFound("Maintenance not found.");

            if (maintenance.MaintenanceStatusId != (int)enMaintenanceStatus.InProgress)
                return ServiceResult<MaintenanceCompletionDTO>
                    .Conflict("Only maintenance in progress can be completed.");

            bool completionExists = await _context.MaintenanceCompletions
                .AnyAsync(c => c.MaintenanceId == dto.MaintenanceId);

            if (completionExists)
                return ServiceResult<MaintenanceCompletionDTO>
                    .Conflict("Maintenance has already been completed.");

            if (dto.VehicleMileage < maintenance.Vehicle.Mileage)
                return ServiceResult<MaintenanceCompletionDTO>
                    .BadRequest("Vehicle mileage cannot be less than the current mileage.");

            if (dto.CompletedDate < DateOnly.FromDateTime(maintenance.MaintenanceDate))
                return ServiceResult<MaintenanceCompletionDTO>
                    .BadRequest("Completed date cannot be earlier than the maintenance date.");

            if (dto.FinalCost < 0)
                return ServiceResult<MaintenanceCompletionDTO>
                    .BadRequest("Final cost cannot be negative.");

            MaintenanceCompletion completion = new MaintenanceCompletion
            {
                MaintenanceId = dto.MaintenanceId,
                CompletedDate = dto.CompletedDate,
                FinalCost = dto.FinalCost,
                Notes = dto.Notes,
                VehicleMileage = dto.VehicleMileage,
                IsPassedInspection = dto.IsPassedInspection,
                CreatedByUserId = createdByUserId
            };

            _context.MaintenanceCompletions.Add(completion);

            maintenance.MaintenanceStatusId = (int)enMaintenanceStatus.Completed;

            maintenance.Vehicle.Mileage = dto.VehicleMileage;
            maintenance.Vehicle.IsAvailableForRent = dto.IsPassedInspection;

            await _context.SaveChangesAsync();

            MaintenanceCompletionDTO completionDto = new MaintenanceCompletionDTO
            {
                CompletionId = completion.CompletionId,
                MaintenanceId = completion.MaintenanceId,
                CompletedDate = completion.CompletedDate,
                CreatedByUserId = completion.CreatedByUserId,
                FinalCost = completion.FinalCost,
                Notes = completion.Notes,
                VehicleMileage = completion.VehicleMileage,
                IsPassedInspection = completion.IsPassedInspection,
                CreatedDate = completion.CreatedDate,
                UpdatedByUserId = completion.UpdatedByUserId,
                UpdatedDate = completion.UpdatedDate
            };

            return ServiceResult<MaintenanceCompletionDTO>.Success(completionDto);
        }
     
        public async Task<List<MaintenanceCompletionListDTO>> GetAllAsync()
        {
            return await _context.MaintenanceCompletions
                .AsNoTracking()
                .OrderByDescending(c => c.CompletionId)
                .Select(c => new MaintenanceCompletionListDTO
                {
                    CompletionId = c.CompletionId,
                    MaintenanceId = c.MaintenanceId,
                    CompletedDate = c.CompletedDate,
                    FinalCost = c.FinalCost,
                    IsPassedInspection = c.IsPassedInspection
                })
                .ToListAsync();
        }

        public async Task<ServiceResult<MaintenanceCompletionDTO>> GetByIdAsync(int completionId)
        {
            MaintenanceCompletionDTO? completion = await _context.MaintenanceCompletions
                .AsNoTracking()
                .Where(c => c.CompletionId == completionId)
                .Select(c => new MaintenanceCompletionDTO
                {
                    CompletionId = c.CompletionId,
                    MaintenanceId = c.MaintenanceId,
                    CompletedDate = c.CompletedDate,
                    CreatedByUserId = c.CreatedByUserId,
                    FinalCost = c.FinalCost,
                    Notes = c.Notes,
                    VehicleMileage = c.VehicleMileage,
                    IsPassedInspection = c.IsPassedInspection,
                    CreatedDate = c.CreatedDate,
                    UpdatedByUserId = c.UpdatedByUserId,
                    UpdatedDate = c.UpdatedDate
                })
                .FirstOrDefaultAsync();

            if (completion == null)
                return ServiceResult<MaintenanceCompletionDTO>
                    .NotFound("Maintenance completion not found.");

            return ServiceResult<MaintenanceCompletionDTO>.Success(completion);
        }

        public async Task<ServiceResult<MaintenanceCompletionDTO>> UpdateAsync(
          int completionId,
          UpdateMaintenanceCompletionDTO dto,
          int updatedByUserId)
        {
            bool userExists = await _context.Users
                .AnyAsync(u => u.UserId == updatedByUserId);

            if (!userExists)
                return ServiceResult<MaintenanceCompletionDTO>
                    .NotFound("User not found.");

            MaintenanceCompletion? completion = await _context.MaintenanceCompletions
                .Include(c => c.Maintenance)
                    .ThenInclude(m => m.Vehicle)
                .FirstOrDefaultAsync(c => c.CompletionId == completionId);

            if (completion == null)
                return ServiceResult<MaintenanceCompletionDTO>
                    .NotFound("Maintenance completion not found.");

            if (dto.VehicleMileage < completion.Maintenance.Vehicle.Mileage)
                return ServiceResult<MaintenanceCompletionDTO>
                    .BadRequest("Vehicle mileage cannot be less than the current mileage.");

            completion.FinalCost = dto.FinalCost;
            completion.Notes = dto.Notes;
            completion.VehicleMileage = dto.VehicleMileage;
            completion.IsPassedInspection = dto.IsPassedInspection;

            completion.UpdatedByUserId = updatedByUserId;
            completion.UpdatedDate = DateTime.Now;

            completion.Maintenance.Vehicle.Mileage = dto.VehicleMileage;
            completion.Maintenance.Vehicle.IsAvailableForRent =
                dto.IsPassedInspection;

            await _context.SaveChangesAsync();

            MaintenanceCompletionDTO completionDto = new MaintenanceCompletionDTO
            {
                CompletionId = completion.CompletionId,
                MaintenanceId = completion.MaintenanceId,
                CompletedDate = completion.CompletedDate,
                CreatedByUserId = completion.CreatedByUserId,
                FinalCost = completion.FinalCost,
                Notes = completion.Notes,
                VehicleMileage = completion.VehicleMileage,
                IsPassedInspection = completion.IsPassedInspection,
                CreatedDate = completion.CreatedDate,
                UpdatedByUserId = completion.UpdatedByUserId,
                UpdatedDate = completion.UpdatedDate
            };

            return ServiceResult<MaintenanceCompletionDTO>.Success(completionDto);
        }
   
    }
}
