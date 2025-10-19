using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerMonitoringServerAPI.Models
{
    [Table("audit_logs")]
    public class AuditLog
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("type")]
        [MaxLength(255)]
        public string? Type { get; set; }

        [Column("attempt_id")]
        public long? AttemptId { get; set; }

        [Column("process_id")]
        public long? ProcessId { get; set; }

        [Column("image_id")]
        public long? ImageId { get; set; }

        [Column("alert_id")]
        public long? AlertId { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("details")]
        public string? Details { get; set; }

        [ForeignKey("AttemptId")]
        public virtual Attempt? Attempt { get; set; }

        [ForeignKey("ProcessId")]
        public virtual Process? ProcessNavigation { get; set; }

        [ForeignKey("ImageId")]
        public virtual Image? Image { get; set; }

        [ForeignKey("AlertId")]
        public virtual Alert? Alert { get; set; }
    }
}
