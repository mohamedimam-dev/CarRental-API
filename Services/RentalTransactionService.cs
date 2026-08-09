using CarRental.API.Common;
using CarRental.API.Data;
using CarRental.API.DTOs.RentalTransactions;
using CarRental.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class RentalTransactionService : IRentalTransactionService
    {
        private readonly CarRentalDbContext _context;

        public RentalTransactionService(CarRentalDbContext context)
        {
            _context = context;
        }


        public async Task<List<RentalTransactionListDTO>> GetAllAsync()
        {
            return await _context.RentalTransactions
                .AsNoTracking()
                .OrderByDescending(t => t.TransactionId)
                .Select(t => new RentalTransactionListDTO
                {
                    TransactionId = t.TransactionId,
                    BookingId = t.BookingId,
                    PaidInitialTotalDueAmount = t.PaidInitialTotalDueAmount,
                    ActualTotalDueAmount = t.ActualTotalDueAmount,
                    TotalRemaining = t.TotalRemaining,
                    TransactionDate = t.TransactionDate
                })
                .ToListAsync();
        }

        public async Task<ServiceResult<RentalTransactionDTO>> GetByIdAsync(int transactionId)
        {
            RentalTransactionDTO? transaction = await _context.RentalTransactions
                .AsNoTracking()
                .Where(t => t.TransactionId == transactionId)
                .Select(t => new RentalTransactionDTO
                {
                    TransactionId = t.TransactionId,
                    BookingId = t.BookingId,
                    ReturnId = t.ReturnId,
                    PaymentMethod = t.PaymentMethod,
                    PaidInitialTotalDueAmount = t.PaidInitialTotalDueAmount,
                    ActualTotalDueAmount = t.ActualTotalDueAmount,
                    TotalRemaining = t.TotalRemaining,
                    TotalRefundedAmount = t.TotalRefundedAmount,
                    TransactionDate = t.TransactionDate,
                    UpdatedTransactionDate = t.UpdatedTransactionDate,
                    CreatedByUserId = t.CreatedByUserId,
                    UpdatedByUserId = t.UpdatedByUserId,
                    UpdatedDate = t.UpdatedDate
                })
                .FirstOrDefaultAsync();

            if (transaction == null)
                return ServiceResult<RentalTransactionDTO>
                    .NotFound("Rental transaction not found.");

            return ServiceResult<RentalTransactionDTO>.Success(transaction);
        }

        public async Task<ServiceResult<RentalTransactionDTO>> GetByBookingIdAsync(int bookingId)
        {
            RentalTransactionDTO? transaction = await _context.RentalTransactions
                .AsNoTracking()
                .Where(t => t.BookingId == bookingId)
                .Select(t => new RentalTransactionDTO
                {
                    TransactionId = t.TransactionId,
                    BookingId = t.BookingId,
                    ReturnId = t.ReturnId,
                    PaymentMethod = t.PaymentMethod,
                    PaidInitialTotalDueAmount = t.PaidInitialTotalDueAmount,
                    ActualTotalDueAmount = t.ActualTotalDueAmount,
                    TotalRemaining = t.TotalRemaining,
                    TotalRefundedAmount = t.TotalRefundedAmount,
                    TransactionDate = t.TransactionDate,
                    UpdatedTransactionDate = t.UpdatedTransactionDate,
                    CreatedByUserId = t.CreatedByUserId,
                    UpdatedByUserId = t.UpdatedByUserId,
                    UpdatedDate = t.UpdatedDate
                })
                .FirstOrDefaultAsync();

            if (transaction == null)
                return ServiceResult<RentalTransactionDTO>
                    .NotFound("Rental transaction not found.");

            return ServiceResult<RentalTransactionDTO>.Success(transaction);
        }
   
    }
}
