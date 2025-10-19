using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerMonitoringServerAPI.Models
{
    [Table("alerts")]
    public class Alert
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("code")]
        [MaxLength(255)]
        public string? Code { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(255)]
        public string? Name { get; set; }

        [Required]
        [Column("description")]
        [MaxLength(255)]
        public string? Description { get; set; }

        [Required]
        [Column("severity")]
        [MaxLength(255)]
        public string? Severity { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
