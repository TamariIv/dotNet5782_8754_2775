﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DataSource
    {
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<Station> Stations = new List<Station>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<Parcel> Parcels = new List<Parcel>();
        internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();

        public static Random r = new Random();

        /// <summary>
        ///
        /// </summary>
        internal class Config
        {
            internal static int ParcelId = 1000000;
        }

        /// <summary>
        /// initialize the data lists 
        /// </summary>
        /// 
        public static void Initialize()
        {
            //initialize stations:
            createStation();
            //initialize 10 customers:
            createCustomer(10);
            //initialize 10 parcels:
            createParcel(10);
            //initialize 5 drones:
            createDrone(5);
        }

        private static void createStation()
        {
            Station s = new Station();
            s.Id = r.Next(10000, 100000); //id number with 5 digits
            s.Name = "Pisgat Zeev";
            s.Longitude = 31.831146;
            s.Latitude = 35.242632;
            s.ChargeSlots = r.Next(11);
            Stations.Add(s);

            s = new Station();
            s.Id = r.Next(10000, 100000);
            s.Name = "Givat Shaul";
            s.Longitude = 31.790835;
            s.Latitude = 35.195144;
            s.ChargeSlots = r.Next(11);
            Stations.Add(s);
        }
        private static void createCustomer(int numOfCustomers)
        {
            string[] namesArray = { "Avraham", "Yitshak", "Yaakov", "Sarah", "Rivka", "Rahel", "Leah", "David", "Moshe", "Aharon" };
            string[] familyNameArray = { "Cohen", "Levi", "Israel", "Shalom", "Silver", "Shushan", "Yosefi", "Dayan", "Ben-David", "Uzan" };
            string[] firstDigits = { "050-", "052-", "054-" };
            for (int i = 0; i < numOfCustomers; i++)
            {
                Customer c = new Customer();
                c.Id = r.Next(100000000, 1000000000);
                c.Name = namesArray[i] + " " + familyNameArray[i];
                c.Phone = firstDigits[r.Next(3)];
                for (int j = 0; j < 7; j++)
                {
                    c.Phone += (r.Next(0, 11)).ToString();
                }
                c.Latitude = 35 + r.NextDouble();
                c.Longitude = 31 + r.NextDouble();
                Customers.Add(c);
            }
      
        }
        private static void createParcel(int numOfParcels)
        {
            for (int i = 0; i < numOfParcels; i++)
            {
                Parcel p = new Parcel();
                p.Id = Config.ParcelId++;
                p.SenderId = r.Next(10000000, 100000000); //buisness number
                int temp = r.Next(0, Customers.Count()); //random number between 0 - length of the customers list as index for TargetId
                p.TargetId = Customers[temp].Id; //the ID of the cutomer
                p.Weight = (WeightCategories)r.Next(3);
                p.Priority = (Priorities)r.Next(3);
                DateTime start = new DateTime(2020, 1, 1);
                int range = (DateTime.Today - start).Days;
                p.Delivered = start.AddDays(r.Next(range));
                range = (p.Delivered - start).Days;
                p.PickedUp = start.AddDays(r.Next(range));
                range = (p.PickedUp - start).Days;
                p.Scheduled = start.AddDays(r.Next(range));
                range = (p.Scheduled - start).Days;
                p.Requested = start.AddDays(r.Next(range));
                p.DroneId = 0;
                Parcels.Add(p);
            }

        }
        private static void createDrone(int numOfDrones)
        {
            for (int i = 0; i < numOfDrones; i++)
            {
                Drone d = new Drone();
                d.Id = r.Next(1001, 10000);
                d.Model = ((char)(r.Next(65, 91))) + (r.Next(111, 999)).ToString(); // for example: S503
                d.MaxWeight = (WeightCategories)r.Next(3);
                d.Status = DroneStatus.available;
                d.Battery = r.Next(100) + r.NextDouble();
                Drones.Add(d);
            }

        }
    }

}


