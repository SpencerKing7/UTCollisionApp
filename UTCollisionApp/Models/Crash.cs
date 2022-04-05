using System;
using System.ComponentModel.DataAnnotations;

namespace UTCollisionApp.Models
{
    public class Crash
    {
        [Key]
        [Required]
        public int CRASH_ID { get; set; }
        public DateTime CRASH_DATETIME { get; set; }
        public int CRASH_SEVERITY_ID { get; set; }

        // Foreign Location Key
        [Required]
        public int LOCATION_ID { get; set; }
        public Location Location { get; set; }

        // Foreign Factor Key
        [Required]
        public int FACTOR_ID { get; set; }
        public Factor Factor { get; set; }
    }
}
