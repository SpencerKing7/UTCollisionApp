using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTCollisionApp.Models
{
    public class SeverityPredictorData
    {
        public float Milepoint { get; set; }
        public float Lat_utm_y { get; set; }
        public float Long_utm_x { get; set; }
        public float Work_zone_related { get; set; }
        public float Pedestrian_involved { get; set; }
        public float Bicyclist_involved { get; set; }
        public float Motorcycle_involved { get; set; }
        public float Improper_restraint { get; set; }
        public float Unrestrained { get; set; }
        public float Dui { get; set; }
        public float Intersection_related { get; set; }
        public float Wild_animal_related { get; set; }
        public float Domestic_animal_related { get; set; }
        public float Overturn_rollover { get; set; }
        public float Commercial_motor_veh_involved { get; set; }
        public float Teenage_driver_involved { get; set; }
        public float Older_driver_involved { get; set; }
        public float Night_dark_condition { get; set; }
        public float Single_vehicle { get; set; }
        public float Distracted_driving { get; set; }
        public float Drowsy_driving { get; set; }
        public float Roadway_departure { get; set; }
        public float Route_89 { get; set; }
        public float Route_Other { get; set; }
        public float Main_road_name_Other { get; set; }
        public float City_Other { get; set; }
        public float City_SALT_LAKE_CITY { get; set; }
        public float City_WEST_VALLEY_CITY { get; set; }
        public float County_name_Other { get; set; }
        public float County_name_SALT_LAKE { get; set; }
        public float County_name_UTAH { get; set; }
        public float County_name_WEBER { get; set; }

        public Tensor<float> AsTensor()
        {
            float[] data = new float[]
            {
                Milepoint, Lat_utm_y, Long_utm_x, Work_zone_related, Pedestrian_involved, Bicyclist_involved,
                Motorcycle_involved, Improper_restraint, Unrestrained, Dui, Intersection_related,
                Wild_animal_related, Domestic_animal_related, Overturn_rollover, Commercial_motor_veh_involved,
                Teenage_driver_involved, Older_driver_involved, Night_dark_condition, Single_vehicle,
                Distracted_driving, Drowsy_driving, Roadway_departure, Route_89, Route_Other, Main_road_name_Other,
                City_Other, City_SALT_LAKE_CITY, City_WEST_VALLEY_CITY, County_name_Other, County_name_SALT_LAKE,
                County_name_UTAH, County_name_WEBER
            };
            int[] dimensions = new int[] { 1, 32 };
            return new DenseTensor<float>(data, dimensions);
        }
    }
}