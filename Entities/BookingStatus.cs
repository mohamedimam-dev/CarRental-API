using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Entities;

[Table("BookingStatus")]
[Index("StatusName", Name = "UQ__BookingS__05E7698AAFC2A2BE", IsUnique = true)]
public partial class BookingStatus
{
    [Key]
    [Column("BookingStatusID")]
    public int BookingStatusId { get; set; }

    [StringLength(100)]
    public string StatusName { get; set; } = null!;

    [InverseProperty("BookingStatus")]
    public virtual ICollection<RentalBooking> RentalBookings { get; set; } = new List<RentalBooking>();
}
