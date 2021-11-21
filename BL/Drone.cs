using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public Enums.WeightCategories MaxWeight { get; set; }
        public int Battery { get; set; }
        public Enums.DroneStatus DroneStatus { get; set; }
        public ParcelInDelivey ParcelInDelivery { get; set; }
        public Location CurrentLocation { get; set; }


        public override string ToString()
        {
            return string.Format(
                "Id is: {0}\n" +
                "model: {1}\n" +
                "max weight is: {2}\n", 
                Id, Model, MaxWeight);
        }
    }
}
