using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Entities;

[Index("PlateNumber", Name = "UQ__Vehicles__0369262436397076", IsUnique = true)]
[Index("Vin", Name = "UQ__Vehicles__C5DF234C0479B8BE", IsUnique = true)]
public partial class Vehicle
{
    [Key]
    [Column("VehicleID")]
    public int VehicleId { get; set; }

    [StringLength(50)]
    public string Make { get; set; } = null!;

    [StringLength(50)]
    public string Model { get; set; } = null!;

    [Column("VIN")]
    [StringLength(50)]
    public string Vin { get; set; } = null!;

    [StringLength(30)]
    public string Color { get; set; } = null!;

    [StringLength(50)]
    public string EngineNumber { get; set; } = null!;

    public int Year { get; set; }

    public int Mileage { get; set; }

    [Column("FuelTypeID")]
    public int FuelTypeId { get; set; }

    [StringLength(20)]
    public string PlateNumber { get; set; } = null!;

    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal RentalPricePerDay { get; set; }

    public bool IsAvailableForRent { get; set; }

    [Column("CreatedByUserID")]
    public int CreatedByUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Vehicles")]
    public virtual VehicleCategory Category { get; set; } = null!;

    [ForeignKey("CreatedByUserId")]
    [InverseProperty("Vehicles")]
    public virtual User CreatedByUser { get; set; } = null!;

    [ForeignKey("FuelTypeId")]
    [InverseProperty("Vehicles")]
    public virtual FuelType FuelType { get; set; } = null!;

    [InverseProperty("Vehicle")]
    public virtual ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();

    [InverseProperty("Vehicle")]
    public virtual ICollection<RentalBooking> RentalBookings { get; set; } = new List<RentalBooking>();
}
