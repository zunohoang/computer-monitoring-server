using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerMonitoringServerAPI.Models
{
    [Table("attempt")]
    public class Attempt
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("contest_id")]
        public long ContestId { get; set; }

        [Required]
        [Column("sbd")]
        public long Sbd { get; set; }

        [Required]
        [Column("ip_ad")]
        public string? IpAd { get; set; }

        [Column("location")]
        public string? Location { get; set; }

        [Column("room_id")]
        public long? RoomId { get; set; }

        [Column("status")]
        [MaxLength(255)]
        public string? Status { get; set; } = "pending";

        [Column("started_at")]
        public DateTime? StartedAt { get; set; }

        [Column("ended_at")]
        public DateTime? EndedAt { get; set; }

        [ForeignKey("ContestId")]
        public virtual Contest? Contest { get; set; }

        [ForeignKey("Sbd")]
        public virtual ContestSbd? ContestSbdNavigation { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room? Room { get; set; }
    }
}
