﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelToList
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string TargetName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatus ParcelStatus { get; set; }

        public override string ToString()
        {
            return string.Format(
                "Id is: {0}\n" +
                "Id of the sender: {1}\n" +
                "Id of the target: {2}\n" +
                "Weight is: {3}\n" +
                "priority: {4}\n" +
                "rcel ststus: {5}\n",
                Id, SenderName, SenderName, Weight, Priority, ParcelStatus);
        }
    }
}
