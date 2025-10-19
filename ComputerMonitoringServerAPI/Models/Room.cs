using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerMonitoringServerAPI.Models
{
    [Table("rooms")]
    public class Room
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("contest_id")]
        public long ContestId { get; set; }

        [Required]
        [Column("access_code")]
        public string? AccessCode { get; set; }

        [Required]
        [Column("rg_start_time")]
        public DateTime RgStartTime { get; set; }

        [Required]
        [Column("rg_end_time")]
        public DateTime RgEndTime { get; set; }

        [Column("capacity")]
        public int? Capacity { get; set; } = 50;

        [ForeignKey("ContestId")]
        public virtual Contest? Contest { get; set; }
    }
}
