using CarRental.API.Common;
using CarRental.API.Data;
using CarRental.API.DTOs.VehiclesReturn;
using CarRental.API.Entities;
using CarRental.API.Enums;
using CarRental.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class VehicleReturnService : IVehicleReturnService
    {
        private readonly CarRentalDbContext _context;

        public VehicleReturnService(CarRentalDbContext context)
        {
            _context = context;
        }


        public async Task<ServiceResult<VehicleReturnDTO>> AddAsync(AddVehicleReturnDTO dto)
        {
            bool userExists = await _context.Users
                .AnyAsync(u => u.UserId == dto.CreatedByUserId);

            if (!userExists)
                return ServiceResult<VehicleReturnDTO>
                    .NotFound("User not found.");

            RentalBooking? booking = await _context.RentalBookings
                .Include(b => b.Vehicle)
                .Include(b => b.RentalTransactions)
                .FirstOrDefaultAsync(b => b.BookingId == dto.BookingId);

            if (booking == null)
                return ServiceResult<VehicleReturnDTO>
                    .NotFound("Rental booking not found.");

            if (booking.BookingStatusId != (int)enBookingStatus.Reserved)
                return ServiceResult<VehicleReturnDTO>
                    .Conflict("Only reserved bookings can be returned.");

            bool returnExists = await _context.VehicleReturns
                .AnyAsync(r => r.BookingId == dto.BookingId);

            if (returnExists)
                return ServiceResult<VehicleReturnDTO>
                    .Conflict("Vehicle return already exists for this booking.");

            if (dto.ActualReturnDate <
                booking.RentalStartDate.ToDateTime(TimeOnly.MinValue))
                return ServiceResult<VehicleReturnDTO>
                    .BadRequest("Actual return date cannot be earlier than the rental start date.");

            if (dto.Mileage < booking.Vehicle.Mileage)
                return ServiceResult<VehicleReturnDTO>
                    .BadRequest("Mileage cannot be less than the current vehicle mileage.");

            byte actualRentalDays =
                checked((byte)
                (DateOnly.FromDateTime(dto.ActualReturnDate).DayNumber
                - booking.RentalStartDate.DayNumber + 1));

            int consumedMileage =
                dto.Mileage - booking.Vehicle.Mileage;

            decimal actualTotalDueAmount =
                (actualRentalDays * booking.RentalPricePerDay)
                + dto.AdditionalCharges;

            VehicleReturn vehicleReturn = new VehicleReturn
            {
                BookingId = booking.BookingId,
                ActualReturnDate = dto.ActualReturnDate,
                ActualRentalDays = actualRentalDays,
                Mileage = dto.Mileage,
                ConsumedMileage = consumedMileage,
                FinalCheckNotes = dto.FinalCheckNotes,
                AdditionalCharges = dto.AdditionalCharges,
                ActualTotalDueAmount = actualTotalDueAmount,
                CreatedByUserId = dto.CreatedByUserId
            };

            _context.VehicleReturns.Add(vehicleReturn);

            RentalTransaction? transaction =
                booking.RentalTransactions.FirstOrDefault();

            if (transaction == null)
                return ServiceResult<VehicleReturnDTO>
                    .Conflict("Rental transaction not found.");

            transaction.Return = vehicleReturn;
            transaction.ActualTotalDueAmount = actualTotalDueAmount;

            if (actualTotalDueAmount > transaction.PaidInitialTotalDueAmount)
            {
                transaction.TotalRemaining =
                    actualTotalDueAmount
                    - transaction.PaidInitialTotalDueAmount;

                transaction.TotalRefundedAmount = 0;
            }
            else
            {
                transaction.TotalRemaining = 0;

                transaction.TotalRefundedAmount =
                    transaction.PaidInitialTotalDueAmount
                    - actualTotalDueAmount;
            }

            transaction.UpdatedTransactionDate = DateTime.Now;
            transaction.UpdatedByUserId = dto.CreatedByUserId;
            transaction.UpdatedDate = DateTime.Now;

            booking.Vehicle.Mileage = dto.Mileage;
            booking.Vehicle.IsAvailableForRent = true;
            booking.BookingStatusId = (int)enBookingStatus.Returned;

            await _context.SaveChangesAsync();

            VehicleReturnDTO vehicleReturnDto = new VehicleReturnDTO
            {
                ReturnId = vehicleReturn.ReturnId,
                BookingId = vehicleReturn.BookingId,
                ActualReturnDate = vehicleReturn.ActualReturnDate,
                ActualRentalDays = vehicleReturn.ActualRentalDays,
                Mileage = vehicleReturn.Mileage,
                ConsumedMileage = vehicleReturn.ConsumedMileage,
                FinalCheckNotes = vehicleReturn.FinalCheckNotes,
                AdditionalCharges = vehicleReturn.AdditionalCharges,
                ActualTotalDueAmount = vehicleReturn.ActualTotalDueAmount,
                CreatedByUserId = vehicleReturn.CreatedByUserId,
                CreatedDate = vehicleReturn.CreatedDate
            };

            return ServiceResult<VehicleReturnDTO>.Success(vehicleReturnDto);
        }
      
        public async Task<List<VehicleReturnListDTO>> GetAllAsync()
        {
            return await _context.VehicleReturns
                .AsNoTracking()
                .OrderByDescending(r => r.ReturnId)
                .Select(r => new VehicleReturnListDTO
                {
                    ReturnId = r.ReturnId,
                    BookingId = r.BookingId,
                    ActualReturnDate = r.ActualReturnDate,
                    ActualTotalDueAmount = r.ActualTotalDueAmount
                })
                .ToListAsync();
        }

        public async Task<ServiceResult<VehicleReturnDTO>> GetByIdAsync(int returnId)
        {
            VehicleReturnDTO? vehicleReturn = await _context.VehicleReturns
                .AsNoTracking()
                .Where(r => r.ReturnId == returnId)
                .Select(r => new VehicleReturnDTO
                {
                    ReturnId = r.ReturnId,
                    BookingId = r.BookingId,
                    ActualReturnDate = r.ActualReturnDate,
                    ActualRentalDays = r.ActualRentalDays,
                    Mileage = r.Mileage,
                    ConsumedMileage = r.ConsumedMileage,
                    FinalCheckNotes = r.FinalCheckNotes,
                    AdditionalCharges = r.AdditionalCharges,
                    ActualTotalDueAmount = r.ActualTotalDueAmount,
                    CreatedByUserId = r.CreatedByUserId,
                    CreatedDate = r.CreatedDate
                })
                .FirstOrDefaultAsync();

            if (vehicleReturn == null)
                return ServiceResult<VehicleReturnDTO>
                    .NotFound("Vehicle return not found.");

            return ServiceResult<VehicleReturnDTO>.Success(vehicleReturn);
        }

        public async Task<ServiceResult<VehicleReturnDTO>> GetByBookingIdAsync(int bookingId)
        {
            VehicleReturnDTO? vehicleReturn = await _context.VehicleReturns
                .AsNoTracking()
                .Where(r => r.BookingId == bookingId)
                .Select(r => new VehicleReturnDTO
                {
                    ReturnId = r.ReturnId,
                    BookingId = r.BookingId,
                    ActualReturnDate = r.ActualReturnDate,
                    ActualRentalDays = r.ActualRentalDays,
                    Mileage = r.Mileage,
                    ConsumedMileage = r.ConsumedMileage,
                    FinalCheckNotes = r.FinalCheckNotes,
                    AdditionalCharges = r.AdditionalCharges,
                    ActualTotalDueAmount = r.ActualTotalDueAmount,
                    CreatedByUserId = r.CreatedByUserId,
                    CreatedDate = r.CreatedDate
                })
                .FirstOrDefaultAsync();

            if (vehicleReturn == null)
                return ServiceResult<VehicleReturnDTO>
                    .NotFound("Vehicle return not found.");

            return ServiceResult<VehicleReturnDTO>.Success(vehicleReturn);
        }
   
    }
}
