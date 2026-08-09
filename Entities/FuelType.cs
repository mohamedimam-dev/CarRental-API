using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Entities;

[Index("FuelType1", Name = "UQ__FuelType__2F4FDCEC745FC45A", IsUnique = true)]
public partial class FuelType
{
    [Key]
    [Column("FuelTypeID")]
    public int FuelTypeId { get; set; }

    [Column("FuelType")]
    [StringLength(20)]
    public string FuelType1 { get; set; } = null!;

    [InverseProperty("FuelType")]
    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
