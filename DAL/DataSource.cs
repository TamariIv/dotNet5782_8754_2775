using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;

namespace DAL
{
    namespace DalObject
    {

        public class DataSource
        {
            internal static List<Drone> Drones;
            internal static List<Station> Stations;
            internal static List<Customer> Customers;
            internal static List<Parcel> Parcels;

            public static Random r = new Random();

            internal class Config
            {
                /// <summary>
                /// the first parcel num. (and than it rises in one)
                /// </summary>
                internal static int ParcelId = 1000000; 
            }

            /// <summary>
            /// initialize the lists 
            /// </summary>
            /// 
            public static void Initialize()
            {   
                //initialize stations
                Station s = new Station();
                s.Id = r.Next(10000, 100000); //id number with 5 digits
                s.Name = "Pisgat Zeev";
                s.Longitude = 31.831146861430444;
                s.Latitude = 35.24263233548004;
                s.CahrgeSlots = r.Next(11);
                Stations.Add(s);
            
                s = new Station();
                s.Id = r.Next(10000, 100000);
                s.Name = "Givat Shaul";
                s.Longitude = 31.79083501738606;
                s.Latitude = 35.19514418203499;
                s.CahrgeSlots = r.Next(11);
                Stations.Add(s);
            
                for (int i = 0; i < 10; i++)
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

                //initialize drones
                for (int i = 0; i < 5; i++) 
                {
                    Drone d = new Drone();
                    d.Id = r.Next(1001, 10000);
                    d.Model = (r.Next(65, 91)).ToString() + (r.Next(111, 999)).ToString(); // for example: SE503
                    d.MaxWeight = (WeightCategories)r.Next(3);
                    d.Status = (DroneStatus)r.Next(3);
                    d.Battery = r.Next(100) + r.NextDouble();
                    Drones.Add(d);
                }

                //initialize customers
                string[] namesArray = { "Avraham", "Yitshak", "Yaakov", "Sarah", "Rivka", "Rahel", "Leah", "David", "Moshe", "Aharon" };
                string[] familyNameArray = { "Cohen", "Levi", "Israel" };
                string[] firstDigits = { "050", "052", "054" };
                for (int i = 0; i < 5; i++)
                {
                    Customer c = new Customer();
                    c.Id = r.Next(100000000, 1000000000);
                    c.Name = namesArray[i] + " " + familyNameArray[i % 3];
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
        }

    }
}

