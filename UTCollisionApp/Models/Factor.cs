using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UTCollisionApp.Models
{
    public class Factor
    {
        [Key]
        [Required]
        public int FactorId { get; set; }
        public bool WorkZoneRelated { get; set; }
        public bool PedestrianInvolved { get; set; }
        public bool BicyclistInvovled { get; set; }
        public bool MotorcycleInvovled { get; set; }
        public bool ImproperRestraint { get; set; }
        public bool Unrestrained { get; set; }
        public bool DUI { get; set; }
        public bool IntersectionRelated { get; set; }
        public bool WildAnimalRelated { get; set; }
        public bool DomesticAnimalRelated { get; set; }
        public bool OverturnRollover { get; set; }
        public bool CommercialMotorVehicleInvolved { get; set; }
        public bool TeenageDriverInvolved { get; set; }
        public bool OlderDriverInvovled { get; set; }
        public bool NightDarkCondition { get; set; }
        public bool SingleVehicle { get; set; }
        public bool DistractedDriving { get; set; }
        public bool DrowsyDriving { get; set; }
        public bool RoadwayDeparture { get; set; }
    }
}
