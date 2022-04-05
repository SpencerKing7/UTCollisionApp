using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UTCollisionApp.Models
{
    public class Location
    {
        [Key]
        [Required]
        public int LOCATION_ID { get; set; }
        public string ROUTE { get; set; }
        public float MILEPOINT { get; set; }
        public float LAT_UTM_Y { get; set; }
        public float LONG_UTM_X { get; set; }
        public string MAIN_ROAD_NAME { get; set; }
        public string CITY { get; set; }
        [Required(ErrorMessage ="Please choose a county where crash occured")]
        public string COUNTY_NAME { get; set; }
    }
}
