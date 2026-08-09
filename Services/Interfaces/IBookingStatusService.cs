using CarRental.API.DTOs.BookingStatus;

namespace CarRental.API.Services.Interfaces
{
    public interface IBookingStatusService
    {
        Task<List<BookingStatusDTO>> GetAllAsync();

    }
}
