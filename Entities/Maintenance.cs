using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Entities;

[Table("Maintenance")]
public partial class Maintenance
{
    [Key]
    [Column("MaintenanceID")]
    public int MaintenanceId { get; set; }

    [Column("VehicleID")]
    public int VehicleId { get; set; }

    [StringLength(300)]
    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime MaintenanceDate { get; set; }

    public DateOnly? ExpectedFinishDate { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Cost { get; set; }

    [Column("MaintenanceStatusID")]
    public int MaintenanceStatusId { get; set; }

    [Column("CreatedByUserID")]
    public int CreatedByUserId { get; set; }

    [Column("UpdatedByUserID")]
    public int? UpdatedByUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    [ForeignKey("CreatedByUserId")]
    [InverseProperty("MaintenanceCreatedByUsers")]
    public virtual User CreatedByUser { get; set; } = null!;

    [InverseProperty("Maintenance")]
    public virtual MaintenanceCompletion? MaintenanceCompletion { get; set; }

    [ForeignKey("MaintenanceStatusId")]
    [InverseProperty("Maintenances")]
    public virtual MaintenanceStatus MaintenanceStatus { get; set; } = null!;

    [ForeignKey("UpdatedByUserId")]
    [InverseProperty("MaintenanceUpdatedByUsers")]
    public virtual User? UpdatedByUser { get; set; }

    [ForeignKey("VehicleId")]
    [InverseProperty("Maintenances")]
    public virtual Vehicle Vehicle { get; set; } = null!;
}
