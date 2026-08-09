using System;
using System.Collections.Generic;
using CarRental.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Data;

public partial class CarRentalDbContext : DbContext
{
    public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BookingStatus> BookingStatuses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<FuelType> FuelTypes { get; set; }

    public virtual DbSet<Maintenance> Maintenances { get; set; }

    public virtual DbSet<MaintenanceCompletion> MaintenanceCompletions { get; set; }

    public virtual DbSet<MaintenanceStatus> MaintenanceStatuses { get; set; }

    public virtual DbSet<RentalBooking> RentalBookings { get; set; }

    public virtual DbSet<RentalTransaction> RentalTransactions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleCategory> VehicleCategories { get; set; }

    public virtual DbSet<VehicleReturn> VehicleReturns { get; set; }

    public virtual DbSet<SecurityLog> SecurityLogs { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookingStatus>(entity =>
        {
            entity.HasKey(e => e.BookingStatusId).HasName("PK__BookingS__54F9C0BD71B93607");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8D916967A");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Customers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customers_Users");
        });

        modelBuilder.Entity<FuelType>(entity =>
        {
            entity.HasKey(e => e.FuelTypeId).HasName("PK__FuelType__048BEE57C2B49FE8");
        });

        modelBuilder.Entity<Maintenance>(entity =>
        {
            entity.HasKey(e => e.MaintenanceId).HasName("PK__Maintena__E60542B50045A7F8");

            entity.Property(e => e.MaintenanceStatusId).HasDefaultValue(1);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.MaintenanceCreatedByUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Maintenance_CreatedByUse");

            entity.HasOne(d => d.MaintenanceStatus).WithMany(p => p.Maintenances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Maintenance_MaintenanceStatus");

            entity.HasOne(d => d.UpdatedByUser).WithMany(p => p.MaintenanceUpdatedByUsers).HasConstraintName("FK_Maintenance_UpdatedByUser");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Maintenances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Maintenance_Vehicle");
        });

        modelBuilder.Entity<MaintenanceCompletion>(entity =>
        {
            entity.HasKey(e => e.CompletionId).HasName("PK__Maintena__77FA70AF99B6533E");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsPassedInspection).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.MaintenanceCompletionCreatedByUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceCompletion_CreatedByUser");

            entity.HasOne(d => d.Maintenance).WithOne(p => p.MaintenanceCompletion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceCompletion_Maintenance");

            entity.HasOne(d => d.UpdatedByUser).WithMany(p => p.MaintenanceCompletionUpdatedByUsers).HasConstraintName("FK_MaintenanceCompletion_UpdatedByUser");
        });

        modelBuilder.Entity<MaintenanceStatus>(entity =>
        {
            entity.HasKey(e => e.MaintenanceStatusId).HasName("PK__Maintena__B4B00191EFB46372");
        });

        modelBuilder.Entity<RentalBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__RentalBo__73951ACDCB8CD2A7");

            entity.Property(e => e.BookingStatusId).HasDefaultValue(1);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.BookingStatus).WithMany(p => p.RentalBookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RentalBooking_BookingStatus");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.RentalBookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RentalBooking_Users");

            entity.HasOne(d => d.Customer).WithMany(p => p.RentalBookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RentalBooking_Customer");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.RentalBookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RentalBooking_Vehicle");
        });

        modelBuilder.Entity<RentalTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__RentalTr__55433A4B071A11C9");

            entity.Property(e => e.PaymentMethod).HasDefaultValue((byte)1);
            entity.Property(e => e.TransactionDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Booking).WithMany(p => p.RentalTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transactions_Booking");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.RentalTransactionCreatedByUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transactions_CreatedByUser");

            entity.HasOne(d => d.Return).WithMany(p => p.RentalTransactions).HasConstraintName("FK_Transactions_Return");

            entity.HasOne(d => d.UpdatedByUser).WithMany(p => p.RentalTransactionUpdatedByUsers).HasConstraintName("FK_Transactions_UpdatedByUser");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3AE58DB173");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC26D2ACD5");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("PK__Vehicles__476B54B228A40BE3");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsAvailableForRent).HasDefaultValue(true);

            entity.HasOne(d => d.Category).WithMany(p => p.Vehicles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vehicles_Categories");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Vehicles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vehicles_Users");

            entity.HasOne(d => d.FuelType).WithMany(p => p.Vehicles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vehicles_FuelTypes");
        });

        modelBuilder.Entity<VehicleCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__VehicleC__19093A2BBC5B1C7E");
        });

        modelBuilder.Entity<VehicleReturn>(entity =>
        {
            entity.HasKey(e => e.ReturnId).HasName("PK__VehicleR__F445E988D4F2726B");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Booking).WithOne(p => p.VehicleReturn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VehicleReturns_Booking");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.VehicleReturns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VehicleReturns_Users");
        });

        modelBuilder.Entity<SecurityLog>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.User)
                .WithMany(p => p.SecurityLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SecurityLogs_Users");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.AuditLogId);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.User)
                .WithMany(p => p.AuditLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuditLogs_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
