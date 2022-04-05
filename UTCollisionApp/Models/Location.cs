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
        public int LocationId { get; set; }
        public string Route { get; set; }
        public float Milepoint { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string MainRoadName { get; set; }
        public string City { get; set; }
        public string CountyName { get; set; }
    }
}
