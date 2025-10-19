using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerMonitoringServerAPI.Models
{
    [Table("violations")]
    public class Violation
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("severity")]
        [MaxLength(255)]
        public string? Severity { get; set; }

        [Required]
        [Column("note")]
        [MaxLength(255)]
        public string? Note { get; set; }

        [Required]
        [Column("handled")]
        public bool Handled { get; set; }

        [Required]
        [Column("handled_at")]
        public DateTime HandledAt { get; set; }

        [Required]
        [Column("handled_by")]
        public long HandledBy { get; set; }

        [Required]
        [Column("attempt_id")]
        public long AttemptId { get; set; }

        [Required]
        [Column("alert_id")]
        public long AlertId { get; set; }

        [Required]
        [Column("created_by")]
        public long CreatedBy { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        [Column("log_start_time")]
        public long LogStartTime { get; set; }

        [Required]
        [Column("log_end_time")]
        public long LogEndTime { get; set; }

        [ForeignKey("HandledBy")]
        public virtual User? HandledByNavigation { get; set; }

        [ForeignKey("AttemptId")]
        public virtual Attempt? Attempt { get; set; }

        [ForeignKey("AlertId")]
        public virtual Alert? Alert { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User? CreatedByNavigation { get; set; }
    }
}
