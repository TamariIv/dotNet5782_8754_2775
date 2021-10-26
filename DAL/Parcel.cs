using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DateTime Requested { get; set; }
            public int DroneId { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }

            public override string ToString()
            {
                return string.Format("Id is: {0}\nId of the sender: {1}\nId of the target: {2}\nWeight is: {3}\npriority: {4}\n requested date: {5}\n Drone id {6}\n" +
                    " scheduled date: {7}\npickedUp date: {8} \ndelivered date: {9}", Id, SenderId, TargetId, Weight, Priority, Requested, DroneId, Scheduled, PickedUp, Delivered);
            }
        }
    }
   
}
