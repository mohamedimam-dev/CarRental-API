using CarRental.API.Common;
using CarRental.API.DTOs.RentalBookings;
using CarRental.API.DTOs.RentalBookings.CarRental.API.DTOs.RentalBookings;

namespace CarRental.API.Services.Interfaces
{
    public interface IRentalBookingService
    {
        Task<ServiceResult<RentalBookingDTO>> AddAsync(
            AddRentalBookingDTO dto,
            int createdByUserId);

        Task<ServiceResult<RentalBookingDTO>> GetByIdAsync(int bookingId);
       
        Task<List<RentalBookingListDTO>> GetAllAsync();

        Task<ServiceResult<bool>> CancelAsync(
            int bookingId,
            int cancelledByUserId);
    }
}
