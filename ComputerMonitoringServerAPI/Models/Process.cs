using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerMonitoringServerAPI.Models
{
    [Table("process")]
    public class Process
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("pid")]
        public int Pid { get; set; }

        [Required]
        [Column("name")]
        public string? Name { get; set; }

        [Column("parent_id")]
        public long? ParentId { get; set; }

        [Column("start_time")]
        public DateTime? StartTime { get; set; }

        [Column("end_time")]
        public DateTime? EndTime { get; set; }

        [Column("data")]
        public string? Data { get; set; }

        [Column("attempt_id")]
        public long? AttemptId { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(255)]
        public string? Status { get; set; }

        [ForeignKey("AttemptId")]
        public virtual Attempt? Attempt { get; set; }
    }
}
