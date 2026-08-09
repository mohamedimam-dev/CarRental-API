using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Entities;

[Index("CategoryName", Name = "UQ__VehicleC__8517B2E02A91B4DA", IsUnique = true)]
public partial class VehicleCategory
{
    [Key]
    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [StringLength(50)]
    public string CategoryName { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
