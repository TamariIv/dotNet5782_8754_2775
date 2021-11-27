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
        public WeightCategories MaxWeight { get; set; }
        public int Battery { get; set; }
        public DroneStatus DroneStatus { get; set; }
        public Location Location { get; set; }
        public int ParcelInDeliveryId { get; set; }

        public override string ToString()
        {
            return string.Format(
                "Id is: {0}\n" +
                "model: {1}\n" +
                "max weight is: {2}\n" +
                "battery: {3}\n" +
                "drone status: {4}\n" +
                "current location: {5}\n" +
                "parcel in delivey: {6}\n",
                Id, Model, MaxWeight, Battery, DroneStatus, Location, (ParcelInDeliveryId == 0 ? "none" : ParcelInDeliveryId));
        }
    }
}
