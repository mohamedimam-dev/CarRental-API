using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Entities;

[Index("Username", Name = "UQ__Users__536C85E42016EBEC", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [StringLength(50)]
    public string Username { get; set; } = null!;

    [StringLength(500)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(30)]
    public string? Phone { get; set; }

    [Column("RoleID")]
    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [StringLength(255)]
    public string? RefreshTokenHash { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RefreshTokenExpiresAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RefreshTokenRevokedAt { get; set; }

    [InverseProperty("CreatedByUser")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    [InverseProperty("CreatedByUser")]
    public virtual ICollection<MaintenanceCompletion> MaintenanceCompletionCreatedByUsers { get; set; } = new List<MaintenanceCompletion>();

    [InverseProperty("UpdatedByUser")]
    public virtual ICollection<MaintenanceCompletion> MaintenanceCompletionUpdatedByUsers { get; set; } = new List<MaintenanceCompletion>();

    [InverseProperty("CreatedByUser")]
    public virtual ICollection<Maintenance> MaintenanceCreatedByUsers { get; set; } = new List<Maintenance>();

    [InverseProperty("UpdatedByUser")]
    public virtual ICollection<Maintenance> MaintenanceUpdatedByUsers { get; set; } = new List<Maintenance>();

    [InverseProperty("CreatedByUser")]
    public virtual ICollection<RentalBooking> RentalBookings { get; set; } = new List<RentalBooking>();

    [InverseProperty("CreatedByUser")]
    public virtual ICollection<RentalTransaction> RentalTransactionCreatedByUsers { get; set; } = new List<RentalTransaction>();

    [InverseProperty("UpdatedByUser")]
    public virtual ICollection<RentalTransaction> RentalTransactionUpdatedByUsers { get; set; } = new List<RentalTransaction>();

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("CreatedByUser")]
    public virtual ICollection<VehicleReturn> VehicleReturns { get; set; } = new List<VehicleReturn>();

    [InverseProperty("CreatedByUser")]
    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    [InverseProperty("User")]
    public virtual ICollection<SecurityLog> SecurityLogs { get; set; }
    = new List<SecurityLog>();

    [InverseProperty("User")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; }
        = new List<AuditLog>();
}
