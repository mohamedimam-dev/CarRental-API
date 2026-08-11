using CarRental.API.Common;
using CarRental.API.Data;
using CarRental.API.DTOs.Vehicles;
using CarRental.API.Entities;
using CarRental.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly CarRentalDbContext _context;

        public VehicleService(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<VehicleDTO>> AddAsync(
            AddVehicleDTO dto,
            int createdByUserId)
        {
            bool userExists = await _context.Users
                .AnyAsync(u => u.UserId == createdByUserId);

            if (!userExists)
                return ServiceResult<VehicleDTO>
                    .NotFound("User not found.");

            bool fuelTypeExists = await _context.FuelTypes
                .AnyAsync(f => f.FuelTypeId == dto.FuelTypeId);

            if (!fuelTypeExists)
                return ServiceResult<VehicleDTO>
                    .NotFound("Fuel type not found.");

            bool categoryExists = await _context.VehicleCategories
                .AnyAsync(c => c.CategoryId == dto.CategoryId);

            if (!categoryExists)
                return ServiceResult<VehicleDTO>
                    .NotFound("Vehicle category not found.");

            bool vinExists = await _context.Vehicles
                .AnyAsync(v => v.Vin == dto.Vin);

            if (vinExists)
                return ServiceResult<VehicleDTO>
                    .Conflict("A vehicle with the same VIN already exists.");

            bool plateExists = await _context.Vehicles
                .AnyAsync(v => v.PlateNumber == dto.PlateNumber);

            if (plateExists)
                return ServiceResult<VehicleDTO>
                    .Conflict("A vehicle with the same plate number already exists.");

            Vehicle vehicle = new Vehicle
            {
                Make = dto.Make,
                Model = dto.Model,
                Vin = dto.Vin,
                Color = dto.Color,
                EngineNumber = dto.EngineNumber,
                Year = dto.Year,
                Mileage = dto.Mileage,
                FuelTypeId = dto.FuelTypeId,
                PlateNumber = dto.PlateNumber,
                CategoryId = dto.CategoryId,
                RentalPricePerDay = dto.RentalPricePerDay,
                IsAvailableForRent = dto.IsAvailableForRent,
                CreatedByUserId = createdByUserId
            };

            _context.Vehicles.Add(vehicle);

            await _context.SaveChangesAsync();

            VehicleDTO vehicleDto = new VehicleDTO
            {
                VehicleId = vehicle.VehicleId,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Vin = vehicle.Vin,
                Color = vehicle.Color,
                EngineNumber = vehicle.EngineNumber,
                Year = vehicle.Year,
                Mileage = vehicle.Mileage,
                FuelTypeId = vehicle.FuelTypeId,
                PlateNumber = vehicle.PlateNumber,
                CategoryId = vehicle.CategoryId,
                RentalPricePerDay = vehicle.RentalPricePerDay,
                IsAvailableForRent = vehicle.IsAvailableForRent,
                CreatedByUserId = vehicle.CreatedByUserId,
                CreatedDate = vehicle.CreatedDate
            };

            return ServiceResult<VehicleDTO>.Success(vehicleDto);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int vehicleId)
        {
            Vehicle? vehicle = await _context.Vehicles
                .FindAsync(vehicleId);

            if (vehicle == null)
                return ServiceResult<bool>
                    .NotFound("Vehicle not found.");

            bool hasBookings = await _context.RentalBookings
                .AnyAsync(b => b.VehicleId == vehicleId);

            if (hasBookings)
                return ServiceResult<bool>
                    .Conflict("Vehicle cannot be deleted because it has rental bookings.");

            _context.Vehicles.Remove(vehicle);

            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }
       
        public async Task<List<VehicleListDTO>> GetAllAsync()
        {
            return await _context.Vehicles
                .AsNoTracking()
                .OrderByDescending(v => v.VehicleId)
                .Select(v => new VehicleListDTO
                {
                    VehicleId = v.VehicleId,
                    Make = v.Make,
                    Model = v.Model,
                    PlateNumber = v.PlateNumber,
                    RentalPricePerDay = v.RentalPricePerDay,
                    IsAvailableForRent = v.IsAvailableForRent
                })
                .ToListAsync();
        }

        public async Task<ServiceResult<VehicleDTO>> GetByIdAsync(int vehicleId)
        {
            VehicleDTO? vehicle = await _context.Vehicles
                .AsNoTracking()
                .Where(v => v.VehicleId == vehicleId)
                .Select(v => new VehicleDTO
                {
                    VehicleId = v.VehicleId,
                    Make = v.Make,
                    Model = v.Model,
                    Vin = v.Vin,
                    Color = v.Color,
                    EngineNumber = v.EngineNumber,
                    Year = v.Year,
                    Mileage = v.Mileage,
                    FuelTypeId = v.FuelTypeId,
                    PlateNumber = v.PlateNumber,
                    CategoryId = v.CategoryId,
                    RentalPricePerDay = v.RentalPricePerDay,
                    IsAvailableForRent = v.IsAvailableForRent,
                    CreatedByUserId = v.CreatedByUserId,
                    CreatedDate = v.CreatedDate
                })
                .FirstOrDefaultAsync();

            if (vehicle == null)
                return ServiceResult<VehicleDTO>
                    .NotFound("Vehicle not found.");

            return ServiceResult<VehicleDTO>.Success(vehicle);
        }

        public async Task<ServiceResult<VehicleDTO>> UpdateAsync(int vehicleId, UpdateVehicleDTO dto)
        {
            Vehicle? vehicle = await _context.Vehicles
                .FindAsync(vehicleId);

            if (vehicle == null)
                return ServiceResult<VehicleDTO>
                    .NotFound("Vehicle not found.");

            bool fuelTypeExists = await _context.FuelTypes
                .AnyAsync(f => f.FuelTypeId == dto.FuelTypeId);

            if (!fuelTypeExists)
                return ServiceResult<VehicleDTO>
                    .NotFound("Fuel type not found.");

            bool categoryExists = await _context.VehicleCategories
                .AnyAsync(c => c.CategoryId == dto.CategoryId);

            if (!categoryExists)
                return ServiceResult<VehicleDTO>
                    .NotFound("Vehicle category not found.");

            bool vinExists = await _context.Vehicles
                .AnyAsync(v =>
                    v.Vin == dto.Vin &&
                    v.VehicleId != vehicleId);

            if (vinExists)
                return ServiceResult<VehicleDTO>
                    .Conflict("A vehicle with the same VIN already exists.");

            bool plateExists = await _context.Vehicles
                .AnyAsync(v =>
                    v.PlateNumber == dto.PlateNumber &&
                    v.VehicleId != vehicleId);

            if (plateExists)
                return ServiceResult<VehicleDTO>
                    .Conflict("A vehicle with the same plate number already exists.");

            vehicle.Make = dto.Make;
            vehicle.Model = dto.Model;
            vehicle.Vin = dto.Vin;
            vehicle.Color = dto.Color;
            vehicle.EngineNumber = dto.EngineNumber;
            vehicle.Year = dto.Year;
            vehicle.Mileage = dto.Mileage;
            vehicle.FuelTypeId = dto.FuelTypeId;
            vehicle.PlateNumber = dto.PlateNumber;
            vehicle.CategoryId = dto.CategoryId;
            vehicle.RentalPricePerDay = dto.RentalPricePerDay;
            vehicle.IsAvailableForRent = dto.IsAvailableForRent;

            await _context.SaveChangesAsync();

            VehicleDTO vehicleDto = new VehicleDTO
            {
                VehicleId = vehicle.VehicleId,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Vin = vehicle.Vin,
                Color = vehicle.Color,
                EngineNumber = vehicle.EngineNumber,
                Year = vehicle.Year,
                Mileage = vehicle.Mileage,
                FuelTypeId = vehicle.FuelTypeId,
                PlateNumber = vehicle.PlateNumber,
                CategoryId = vehicle.CategoryId,
                RentalPricePerDay = vehicle.RentalPricePerDay,
                IsAvailableForRent = vehicle.IsAvailableForRent,
                CreatedByUserId = vehicle.CreatedByUserId,
                CreatedDate = vehicle.CreatedDate
            };

            return ServiceResult<VehicleDTO>.Success(vehicleDto);
        }
    
    }
}
