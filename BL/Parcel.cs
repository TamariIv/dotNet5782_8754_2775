using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Parcel
    {
        public int Id { get; set; }
        CustomerInParcel Sender { get; set; }
        CustomerInParcel Target { get; set; }
        public Enums.WeightCategories Weight { get; set; }
        public Enums.Priorities Priority { get; set; }
        DroneInParcel AssignedDrone { get; set; }
        public DateTime Requested { get; set; } // creating parcel time
        public DateTime Scheduled { get; set; } // assigning drone to parcel time
        public DateTime PickedUp { get; set; }  // pick up time
        public DateTime Delivered { get; set; } // delivey time

        public override string ToString()
        {
            return string.Format(
                "Id is: {0}\n" +
                "Id of the sender: {1}\n" +
                "Id of the target: {2}\n" +
                "Weight is: {3}\n" +
                "priority: {4}\n" +
                "requested date: {5}\n" +
                //"Drone id: {6}\n" + 
                "scheduled date: {7}\n" +
                "pickedUp date: {8}\n" +
                "delivered date: {9}\n",
                Id, Sender, Target, Weight, Priority, Requested.ToString("dd/MM/yyyy"), Scheduled, PickedUp, Delivered);
        }
    }
}
