using CarRental.API.Common;
using CarRental.API.DTOs.RentalTransactions;

namespace CarRental.API.Services.Interfaces
{
    public interface IRentalTransactionService
    {
        Task<ServiceResult<RentalTransactionDTO>> GetByIdAsync(int transactionId);

        Task<ServiceResult<RentalTransactionDTO>> GetByBookingIdAsync(int bookingId);
       
        Task<List<RentalTransactionListDTO>> GetAllAsync();
    }
}
