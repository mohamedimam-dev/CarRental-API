using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Entities;

[Index("DriverLicenseNumber", Name = "UQ__Customer__C32FF260A6B5B7C0", IsUnique = true)]
public partial class Customer
{
    [Key]
    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string ContactInformation { get; set; } = null!;

    [StringLength(20)]
    public string DriverLicenseNumber { get; set; } = null!;

    [Column("CreatedByUserID")]
    public int CreatedByUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("CreatedByUserId")]
    [InverseProperty("Customers")]
    public virtual User CreatedByUser { get; set; } = null!;

    [InverseProperty("Customer")]
    public virtual ICollection<RentalBooking> RentalBookings { get; set; } = new List<RentalBooking>();
}
