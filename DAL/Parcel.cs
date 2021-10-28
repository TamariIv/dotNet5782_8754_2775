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
                return string.Format("Id is: {0}\nId of the sender: {1}\nId of the target: {2}\nWeight is: {3}\npriority: {4}\nrequested date: {5}\nDrone id: {6}\nscheduled date: {7}\npickedUp date: {8}\ndelivered date: {9}\n", Id, SenderId, TargetId, Weight, Priority, Requested.ToString("dd/MM/yyyy"), DroneId, Scheduled.ToString("dd/MM/yyyy"), PickedUp.ToString("dd/MM/yyyy"), Delivered.ToString("dd/MM/yyyy"));
            }
        }
    }
   
}
