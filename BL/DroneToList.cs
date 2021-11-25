﻿using System;
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
        public double Battery { get; set; }
        public DroneStatus DroneStatus { get; set; }
        public ParcelInDelivey ParcelInDelivery { get; set; }
        public Location Location { get; set; }
        public int ParcelInDeliveryId { get; set; }
    }
}
