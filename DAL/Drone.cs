using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatus Status { get; set; }
            public double Battery { get; set; }

            public override string ToString()
            {
                return string.Format("Id is: {0}\nmodel: {1}\nmax weight is: {2}\nstatus is: {3}\nbattery:" +
                    " {4}\n", Id, Model, MaxWeight, Status,(int)Battery);
            }
        }
    }

}
