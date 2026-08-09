using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.API.Entities
{

    public partial class AuditLog
    {
        [Key]
        [Column("AuditLogID")]
        public int AuditLogId { get; set; }

        [Column("UserID")]
        public int UserId { get; set; }

        [StringLength(50)]
        public string Action { get; set; } = null!;

        [StringLength(50)]
        public string EntityName { get; set; } = null!;

        public int EntityId { get; set; }

        [StringLength(45)]
        public string IPAddress { get; set; } = null!;

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("AuditLogs")]
        public virtual User User { get; set; } = null!;
    }
}
