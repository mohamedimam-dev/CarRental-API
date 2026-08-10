using CarRental.API.Common;
using CarRental.API.Data;
using CarRental.API.DTOs.RentalBookings;
using CarRental.API.DTOs.RentalBookings.CarRental.API.DTOs.RentalBookings;
using CarRental.API.Entities;
using CarRental.API.Enums;
using CarRental.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class RentalBookingService : IRentalBookingService
    {
        private readonly CarRentalDbContext _context;

        public RentalBookingService(CarRentalDbContext context)
        {
            _context = context;
        }


        public async Task<ServiceResult<RentalBookingDTO>> AddAsync(
      AddRentalBookingDTO dto,
      int createdByUserId)
        {
            bool customerExists = await _context.Customers
                .AnyAsync(c => c.CustomerId == dto.CustomerId);

            if (!customerExists)
                return ServiceResult<RentalBookingDTO>
                    .NotFound("Customer not found.");

            bool userExists = await _context.Users
                .AnyAsync(u => u.UserId == createdByUserId);

            if (!userExists)
                return ServiceResult<RentalBookingDTO>
                    .NotFound("User not found.");

            Vehicle? vehicle = await _context.Vehicles
                .FindAsync(dto.VehicleId);

            if (vehicle == null)
                return ServiceResult<RentalBookingDTO>
                    .NotFound("Vehicle not found.");

            if (!vehicle.IsAvailableForRent)
                return ServiceResult<RentalBookingDTO>
                    .Conflict("Vehicle is not available for rent.");

            if (dto.RentalStartDate < DateOnly.FromDateTime(DateTime.Today))
                return ServiceResult<RentalBookingDTO>
                    .BadRequest("Rental start date cannot be in the past.");

            if (dto.RentalEndDate < dto.RentalStartDate)
                return ServiceResult<RentalBookingDTO>
                    .BadRequest("Rental end date cannot be earlier than the rental start date.");

            byte initialRentalDays =
                checked((byte)
                (dto.RentalEndDate.DayNumber
                - dto.RentalStartDate.DayNumber + 1));

            decimal initialTotalDueAmount =
                initialRentalDays * dto.RentalPricePerDay;

            RentalBooking booking = new RentalBooking
            {
                CustomerId = dto.CustomerId,
                VehicleId = dto.VehicleId,
                RentalStartDate = dto.RentalStartDate,
                RentalEndDate = dto.RentalEndDate,
                PickupLocation = dto.PickupLocation,
                DropoffLocation = dto.DropoffLocation,
                InitialRentalDays = initialRentalDays,
                RentalPricePerDay = dto.RentalPricePerDay,
                InitialTotalDueAmount = initialTotalDueAmount,
                InitialCheckNotes = dto.InitialCheckNotes,
                BookingStatusId = (int)enBookingStatus.Reserved,
                CreatedByUserId = createdByUserId
            };

            booking.RentalTransactions.Add(new RentalTransaction
            {
                PaidInitialTotalDueAmount = initialTotalDueAmount,
                CreatedByUserId = createdByUserId
            });

            _context.RentalBookings.Add(booking);

            vehicle.IsAvailableForRent = false;

            await _context.SaveChangesAsync();

            RentalBookingDTO bookingDto = new RentalBookingDTO
            {
                BookingId = booking.BookingId,
                CustomerId = booking.CustomerId,
                VehicleId = booking.VehicleId,
                RentalStartDate = booking.RentalStartDate,
                RentalEndDate = booking.RentalEndDate,
                PickupLocation = booking.PickupLocation,
                DropoffLocation = booking.DropoffLocation,
                InitialRentalDays = booking.InitialRentalDays,
                RentalPricePerDay = booking.RentalPricePerDay,
                InitialTotalDueAmount = booking.InitialTotalDueAmount,
                InitialCheckNotes = booking.InitialCheckNotes,
                BookingStatusId = booking.BookingStatusId,
                CreatedByUserId = booking.CreatedByUserId,
                CreatedDate = booking.CreatedDate
            };

            return ServiceResult<RentalBookingDTO>.Success(bookingDto);
        }

        public async Task<ServiceResult<bool>> CancelAsync(
          int bookingId,
          int cancelledByUserId)
        {
            bool userExists = await _context.Users
                .AnyAsync(u => u.UserId == cancelledByUserId);

            if (!userExists)
                return ServiceResult<bool>
                    .NotFound("User not found.");

            RentalBooking? booking = await _context.RentalBookings
                .Include(b => b.Vehicle)
                .Include(b => b.RentalTransactions)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
                return ServiceResult<bool>
                    .NotFound("Rental booking not found.");

            if (booking.BookingStatusId != (int)enBookingStatus.Reserved)
                return ServiceResult<bool>
                    .Conflict("Only reserved bookings can be cancelled.");

            RentalTransaction? transaction =
                booking.RentalTransactions.FirstOrDefault();

            if (transaction == null)
                return ServiceResult<bool>
                    .Conflict("Rental transaction not found.");

            TimeSpan elapsed =
                DateTime.Now - booking.CreatedDate;

            decimal refundAmount;

            if (elapsed <= TimeSpan.FromHours(1))
            {
                refundAmount = transaction.PaidInitialTotalDueAmount;
            }
            else
            {
                refundAmount =
                    transaction.PaidInitialTotalDueAmount
                    - booking.RentalPricePerDay;

                if (refundAmount < 0)
                    refundAmount = 0;
            }

            transaction.TotalRefundedAmount = refundAmount;
            transaction.TotalRemaining = 0;
            transaction.UpdatedTransactionDate = DateTime.Now;
            transaction.UpdatedByUserId = cancelledByUserId;
            transaction.UpdatedDate = DateTime.Now;

            booking.BookingStatusId = (int)enBookingStatus.Cancelled;

            booking.Vehicle.IsAvailableForRent = true;

            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }
      
        public async Task<List<RentalBookingListDTO>> GetAllAsync()
        {
            return await _context.RentalBookings
                .AsNoTracking()
                .OrderByDescending(b => b.BookingId)
                .Select(b => new RentalBookingListDTO
                {
                    BookingId = b.BookingId,
                    CustomerId = b.CustomerId,
                    VehicleId = b.VehicleId,
                    RentalStartDate = b.RentalStartDate,
                    RentalEndDate = b.RentalEndDate,
                    InitialRentalDays = b.InitialRentalDays,
                    InitialTotalDueAmount = b.InitialTotalDueAmount,
                    BookingStatusId = b.BookingStatusId
                })
                .ToListAsync();
        }

        public async Task<ServiceResult<RentalBookingDTO>> GetByIdAsync(int bookingId)
        {
            RentalBookingDTO? booking = await _context.RentalBookings
                .AsNoTracking()
                .Where(b => b.BookingId == bookingId)
                .Select(b => new RentalBookingDTO
                {
                    BookingId = b.BookingId,
                    CustomerId = b.CustomerId,
                    VehicleId = b.VehicleId,
                    RentalStartDate = b.RentalStartDate,
                    RentalEndDate = b.RentalEndDate,
                    PickupLocation = b.PickupLocation,
                    DropoffLocation = b.DropoffLocation,
                    InitialRentalDays = b.InitialRentalDays,
                    RentalPricePerDay = b.RentalPricePerDay,
                    InitialTotalDueAmount = b.InitialTotalDueAmount,
                    InitialCheckNotes = b.InitialCheckNotes,
                    BookingStatusId = b.BookingStatusId,
                    CreatedByUserId = b.CreatedByUserId,
                    CreatedDate = b.CreatedDate
                })
                .FirstOrDefaultAsync();

            if (booking == null)
                return ServiceResult<RentalBookingDTO>
                    .NotFound("Rental booking not found.");

            return ServiceResult<RentalBookingDTO>.Success(booking);
        }
   
    }
}
