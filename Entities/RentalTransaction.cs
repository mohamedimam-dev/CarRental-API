using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Entities;

public partial class RentalTransaction
{
    [Key]
    [Column("TransactionID")]
    public int TransactionId { get; set; }

    [Column("BookingID")]
    public int BookingId { get; set; }

    [Column("ReturnID")]
    public int? ReturnId { get; set; }

    public byte? PaymentMethod { get; set; }

    [Column(TypeName = "smallmoney")]
    public decimal PaidInitialTotalDueAmount { get; set; }

    [Column(TypeName = "smallmoney")]
    public decimal? ActualTotalDueAmount { get; set; }

    [Column(TypeName = "smallmoney")]
    public decimal? TotalRemaining { get; set; }

    [Column(TypeName = "smallmoney")]
    public decimal? TotalRefundedAmount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TransactionDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedTransactionDate { get; set; }

    [Column("CreatedByUserID")]
    public int CreatedByUserId { get; set; }

    [Column("UpdatedByUserID")]
    public int? UpdatedByUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    [ForeignKey("BookingId")]
    [InverseProperty("RentalTransactions")]
    public virtual RentalBooking Booking { get; set; } = null!;

    [ForeignKey("CreatedByUserId")]
    [InverseProperty("RentalTransactionCreatedByUsers")]
    public virtual User CreatedByUser { get; set; } = null!;

    [ForeignKey("ReturnId")]
    [InverseProperty("RentalTransactions")]
    public virtual VehicleReturn? Return { get; set; }

    [ForeignKey("UpdatedByUserId")]
    [InverseProperty("RentalTransactionUpdatedByUsers")]
    public virtual User? UpdatedByUser { get; set; }
}
