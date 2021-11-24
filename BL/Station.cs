using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int AvailableAvailableChargeSlots { get; set; }
        public List<DroneInCharging> DronesCharging { get; set; }

        public override string ToString()
        {
            return string.Format(
                "Id is: {0}\n" +
                "Name of station: {1}\n" +
                "Location:\n{2}\n" +
                "Number of open charge slots: {3}\n", 
                Id, Name, Location, AvailableAvailableChargeSlots);
        }
    }
}
