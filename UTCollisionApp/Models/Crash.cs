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

        // Foreign Location Key
        public int LocationId { get; set; }
        public Location Location { get; set; }

        // Foreign Factor Key
        public int FactorId { get; set; }
        public Factor Factor { get; set; }
    }
}
