using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerMonitoringServerAPI.Models
{
    [Table("images")]
    public class Image
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("url")]
        public string? Url { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("meta")]
        public string? Meta { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(255)]
        public string? Status { get; set; }
    }
}
