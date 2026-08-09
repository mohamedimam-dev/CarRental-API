using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Entities;

[Table("RentalBooking")]
public partial class RentalBooking
{
    [Key]
    [Column("BookingID")]
    public int BookingId { get; set; }

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [Column("VehicleID")]
    public int VehicleId { get; set; }

    public DateOnly RentalStartDate { get; set; }

    public DateOnly RentalEndDate { get; set; }

    [StringLength(100)]
    public string? PickupLocation { get; set; }

    [StringLength(100)]
    public string? DropoffLocation { get; set; }

    public byte InitialRentalDays { get; set; }

    [Column(TypeName = "smallmoney")]
    public decimal RentalPricePerDay { get; set; }

    [Column(TypeName = "smallmoney")]
    public decimal InitialTotalDueAmount { get; set; }

    [StringLength(500)]
    public string? InitialCheckNotes { get; set; }

    [Column("BookingStatusID")]
    public int BookingStatusId { get; set; }

    [Column("CreatedByUserID")]
    public int CreatedByUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("BookingStatusId")]
    [InverseProperty("RentalBookings")]
    public virtual BookingStatus BookingStatus { get; set; } = null!;

    [ForeignKey("CreatedByUserId")]
    [InverseProperty("RentalBookings")]
    public virtual User CreatedByUser { get; set; } = null!;

    [ForeignKey("CustomerId")]
    [InverseProperty("RentalBookings")]
    public virtual Customer Customer { get; set; } = null!;

    [InverseProperty("Booking")]
    public virtual ICollection<RentalTransaction> RentalTransactions { get; set; } = new List<RentalTransaction>();

    [ForeignKey("VehicleId")]
    [InverseProperty("RentalBookings")]
    public virtual Vehicle Vehicle { get; set; } = null!;

    [InverseProperty("Booking")]
    public virtual VehicleReturn? VehicleReturn { get; set; }
}
