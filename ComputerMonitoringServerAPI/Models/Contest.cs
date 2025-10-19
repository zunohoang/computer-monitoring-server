using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerMonitoringServerAPI.Models
{
    [Table("contests")]
    public class Contest
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }  

        [Required]
        [Column("name", TypeName = "text")]
        public string Name { get; set; }

        [Column("description", TypeName = "text")]
        public string? Description { get; set; }

        [Column("start_time")]
        public DateTime? StartTime { get; set; }

        [Column("end_time")]
        public DateTime? EndTime { get; set; }

        [Column("created_by")]
        public long CreatedBy { get; set; } 

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
