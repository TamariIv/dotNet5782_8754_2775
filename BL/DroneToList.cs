using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneToList
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public Enums.WeightCategories MaxWeight { get; set; }
        public int Battery { get; set; }
        public Enums.DroneStatus DroneStatus { get; set; }
        public ParcelInDelivey ParcelInDelivery { get; set; }
        public Location Location { get; set; }
        public int ParcelInDeliveryId { get; set; }
    }
}
