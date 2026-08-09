using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Entities;

[Index("BookingId", Name = "UQ__VehicleR__73951ACC600522F3", IsUnique = true)]
public partial class VehicleReturn
{
    [Key]
    [Column("ReturnID")]
    public int ReturnId { get; set; }

    [Column("BookingID")]
    public int BookingId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ActualReturnDate { get; set; }

    public byte ActualRentalDays { get; set; }

    public int Mileage { get; set; }

    public int ConsumedMileage { get; set; }

    [StringLength(500)]
    public string? FinalCheckNotes { get; set; }

    [Column(TypeName = "smallmoney")]
    public decimal AdditionalCharges { get; set; }

    [Column(TypeName = "smallmoney")]
    public decimal ActualTotalDueAmount { get; set; }

    [Column("CreatedByUserID")]
    public int CreatedByUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("BookingId")]
    [InverseProperty("VehicleReturn")]
    public virtual RentalBooking Booking { get; set; } = null!;

    [ForeignKey("CreatedByUserId")]
    [InverseProperty("VehicleReturns")]
    public virtual User CreatedByUser { get; set; } = null!;

    [InverseProperty("Return")]
    public virtual ICollection<RentalTransaction> RentalTransactions { get; set; } = new List<RentalTransaction>();
}
