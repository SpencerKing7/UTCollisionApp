using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTCollisionApp.Models
{
    public class CityPredictorData
    {
        public float Crash_Severity_ID { get; set; }
        public float Work_zone_related { get; set; }
        public float Pedestrian_involved { get; set; }
        public float Bicyclist_involved { get; set; }
        public float Motorcycle_involved { get; set; }
        public float Improper_restraint { get; set; }
        public float Unrestrained { get; set; }
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
        public float County_Name_Utah { get; set; }
        public float County_Name_Salt_Lake { get; set; }

        public Tensor<float> AsTensor()
        {
            float[] data = new float[]
            {
                Crash_Severity_ID, Work_zone_related, Pedestrian_involved, Bicyclist_involved,
                Motorcycle_involved, Improper_restraint, Unrestrained, Intersection_related,
                Wild_animal_related, Domestic_animal_related, Overturn_rollover, Commercial_motor_veh_involved,
                Teenage_driver_involved, Older_driver_involved, Night_dark_condition, Single_vehicle,
                Distracted_driving, Drowsy_driving, Roadway_departure, County_Name_Utah, County_Name_Salt_Lake
            };
            int[] dimensions = new int[] { 1, 21 };
            return new DenseTensor<float>(data, dimensions);
        }
    }
}