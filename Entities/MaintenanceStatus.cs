using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Entities;

[Table("MaintenanceStatus")]
[Index("StatusName", Name = "UQ__Maintena__05E7698AFF1BCEA5", IsUnique = true)]
public partial class MaintenanceStatus
{
    [Key]
    [Column("MaintenanceStatusID")]
    public int MaintenanceStatusId { get; set; }

    [StringLength(100)]
    public string StatusName { get; set; } = null!;

    [InverseProperty("MaintenanceStatus")]
    public virtual ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
}
