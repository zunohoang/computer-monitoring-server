using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerMonitoringServerAPI.Models
{
    [Table("contest_process_blacklist")]
    public class ContestProcessBlacklist
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("process_id")]
        public long ProcessId { get; set; }

        [Required]
        [Column("contest_id")]
        public long ContestId { get; set; }

        [ForeignKey("ProcessId")]
        public virtual ProcessBlacklist? ProcessBlacklist { get; set; }

        [ForeignKey("ContestId")]
        public virtual Contest? Contest { get; set; }
    }
}
