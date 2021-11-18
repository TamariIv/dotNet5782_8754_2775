using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public IBL.BO.Enums.WeightCategories MaxWeight { get; set; }
        public int Battery { get; set; }
        public IBL.BO.Enums.DroneStatus DroneStatus { get; set; }

        public override string ToString()
        {
            return string.Format("Id is: {0}\nmodel: {1}\nmax weight is: " + " {4}\n", Id, Model, MaxWeight);
        }
    }
}
