namespace CarRental.API.DTOs.RentalTransactions
{
    public class RentalTransactionListDTO
    {
        public int TransactionId { get; set; }

        public int BookingId { get; set; }

        public decimal PaidInitialTotalDueAmount { get; set; }

        public decimal? ActualTotalDueAmount { get; set; }

        public decimal? TotalRemaining { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}
