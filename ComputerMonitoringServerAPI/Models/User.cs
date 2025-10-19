using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerMonitoringServerAPI.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("username")]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        [Column("email")]
        public string? Email { get; set; }

        [Required]
        [Column("password_hash")]
        public string? PasswordHash { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("role")]
        public string Role { get; set; } = "participant";

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(255)]
        [Column("full_name")]
        public string? FullName { get; set; }

        [Required]
        [Column("new_column")]
        public long NewColumn { get; set; }
    }
}
