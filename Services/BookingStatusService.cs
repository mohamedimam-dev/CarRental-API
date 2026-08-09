using CarRental.API.Data;
using CarRental.API.DTOs.BookingStatus;
using CarRental.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class BookingStatusService : IBookingStatusService
    {
        private readonly CarRentalDbContext _context;

        public BookingStatusService(CarRentalDbContext context)
        {
            _context = context;
        }


        public async Task<List<BookingStatusDTO>> GetAllAsync()
        {
            return await _context.BookingStatuses
                .AsNoTracking()
                .OrderBy(s => s.BookingStatusId)
                .Select(s => new BookingStatusDTO
                {
                    BookingStatusId = s.BookingStatusId,
                    StatusName = s.StatusName
                })
                .ToListAsync();
        }
    }
}
