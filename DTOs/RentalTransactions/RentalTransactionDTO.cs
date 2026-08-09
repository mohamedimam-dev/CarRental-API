namespace CarRental.API.DTOs.RentalTransactions
{
    public class RentalTransactionDTO
    {
        public int TransactionId { get; set; }

        public int BookingId { get; set; }

        public int? ReturnId { get; set; }

        public byte? PaymentMethod { get; set; }

        public decimal PaidInitialTotalDueAmount { get; set; }

        public decimal? ActualTotalDueAmount { get; set; }

        public decimal? TotalRemaining { get; set; }

        public decimal? TotalRefundedAmount { get; set; }

        public DateTime TransactionDate { get; set; }

        public DateTime? UpdatedTransactionDate { get; set; }

        public int CreatedByUserId { get; set; }

        public int? UpdatedByUserId { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
