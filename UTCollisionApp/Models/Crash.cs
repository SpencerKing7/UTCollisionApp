using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTCollisionApp.Models
{
    public class Crash
    {
        [Key]
        [Required]
        public int CRASH_ID { get; set; }
        public DateTime CRASH_DATETIME { get; set; }

        [Range(1,5, ErrorMessage = "Please enter correct value")]
        public int CRASH_SEVERITY_ID { get; set; }

        // Foreign Location Key
        [Required]
        [ForeignKey("LOCATION_ID")]
        public Location Location { get; set; }

        // Foreign Factor Key
        [Required]
        [ForeignKey("FACTOR_ID")]
        public Factor Factor { get; set; }
    }
}
