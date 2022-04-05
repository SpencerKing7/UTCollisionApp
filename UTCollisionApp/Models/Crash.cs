using System;
using System.ComponentModel.DataAnnotations;

namespace UTCollisionApp.Models
{
    public class Crash
    {
        [Key]
        [Required]
        public int CrashId { get; set; }
        public DateTime CrashDateTime { get; set; }
        public int CrashSeverityId { get; set; }
        public int LocationId { get; set; }
        public int FactorId { get; set; }
    }
}
