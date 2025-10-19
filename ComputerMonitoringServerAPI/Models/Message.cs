using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerMonitoringServerAPI.Models
{
    [Table("messages")]
    public class Message
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("type")]
        [MaxLength(255)]
        public string? Type { get; set; }

        [Required]
        [Column("content")]
        public string? Content { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Required]
        [Column("created_by")]
        public long CreatedBy { get; set; }

        [Required]
        [Column("room_id")]
        public long RoomId { get; set; }

        [Required]
        [Column("attempt_id")]
        public long AttemptId { get; set; }

        [Required]
        [Column("contest_id")]
        public long ContestId { get; set; }

        [Required]
        [Column("title")]
        [MaxLength(255)]
        public string? Title { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User? CreatedByNavigation { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room? Room { get; set; }

        [ForeignKey("AttemptId")]
        public virtual Attempt? Attempt { get; set; }

        [ForeignKey("ContestId")]
        public virtual Contest? Contest { get; set; }
    }
}
