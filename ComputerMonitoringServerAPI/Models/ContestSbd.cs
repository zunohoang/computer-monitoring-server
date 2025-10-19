using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerMonitoringServerAPI.Models
{
    [Table("contest_sbd")]
    public class ContestSbd
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("sbd")]
        public long Sbd { get; set; }

        [Required]
        [Column("full_name")]
        public string? FullName { get; set; }

        [Required]
        [Column("contest_id")]
        public long ContestId { get; set; }

        [Column("user_id")]
        public long? UserId { get; set; }

        [ForeignKey("ContestId")]
        public virtual Contest? Contest { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
