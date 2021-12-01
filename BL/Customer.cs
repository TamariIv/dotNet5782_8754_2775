﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Customer
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Phone { get; init; }
        public Location Location { get; init; }
        public List<Parcel> Send { get; init; }
        public List<Parcel> Receive { get; init; }


        public override string ToString()
        {
            string senderParcels = string.Join(" , ", Send);
            string recieverParcels = string.Join(" , ", Receive);
            return string.Format(
                "Id is: {0}\n" +
                "Name of customer: {1}\n" +
                "Phone number: {2}\n" +
                "Location: {3}\n Parcels from customer: {4}\n parcels to customer: {5}", Id, Name, Phone, Location, senderParcels, recieverParcels);
        }


    }

}
