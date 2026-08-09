using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Entities;

[Table("MaintenanceCompletion")]
[Index("MaintenanceId", Name = "UQ__Maintena__E60542B41CE536EC", IsUnique = true)]
public partial class MaintenanceCompletion
{
    [Key]
    [Column("CompletionID")]
    public int CompletionId { get; set; }

    [Column("MaintenanceID")]
    public int MaintenanceId { get; set; }

    public DateOnly CompletedDate { get; set; }

    [Column("CreatedByUserID")]
    public int CreatedByUserId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal FinalCost { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public int VehicleMileage { get; set; }

    public bool IsPassedInspection { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [Column("UpdatedByUserID")]
    public int? UpdatedByUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    [ForeignKey("CreatedByUserId")]
    [InverseProperty("MaintenanceCompletionCreatedByUsers")]
    public virtual User CreatedByUser { get; set; } = null!;

    [ForeignKey("MaintenanceId")]
    [InverseProperty("MaintenanceCompletion")]
    public virtual Maintenance Maintenance { get; set; } = null!;

    [ForeignKey("UpdatedByUserId")]
    [InverseProperty("MaintenanceCompletionUpdatedByUsers")]
    public virtual User? UpdatedByUser { get; set; }
}
