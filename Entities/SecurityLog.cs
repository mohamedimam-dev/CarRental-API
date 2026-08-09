using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.API.Entities
{

    public partial class SecurityLog
    {
        [Key]
        [Column("LogID")]
        public int LogId { get; set; }

        [StringLength(50)]
        public string EventType { get; set; } = null!;

        [Column("UserID")]
        public int? UserId { get; set; }

        [StringLength(45)]
        public string IPAddress { get; set; } = null!;

        [StringLength(255)]
        public string Endpoint { get; set; } = null!;

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SecurityLogs")]
        public virtual User? User { get; set; }
    }
}
