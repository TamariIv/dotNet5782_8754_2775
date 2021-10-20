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
                //internal static int Drone_i = 0;
                //internal static int Station_i = 0;
                //internal static int Customer_i = 0;
                //internal static int Parcel_i = 0;
                //internal static int ParcelNum = 0;
                internal int ParcelId;

            }
            public static void Initialize()
            {
                for (int i = 0; i < 2; i++) //initialize station
                {
                    Station s = new Station();
                    s.Id = r.Next(10000, 10000);
                    s.Name = r.Next(10000, 100000);
                    s.Longitude = r.NextDouble() + r.Next(-180, 80);
                    s.Latitude = r.NextDouble() + r.Next(-90, 90);
                    s.ChargeSlots = r.Next(1, 6);
                    Stations.Add(s);
                }
               
                for (int i = 0; i < 5; i++) //initialize drone
                {
                    Drone d = new Drone();
                    d.Id = r.Next(1001, 10000); 
                    d.Model = (r.Next(65, 91)).ToString() + (r.Next(111,999)).ToString(); // for example: SE503
                    d.MaxWeight = (WeightCategories)r.Next(3);
                    Drones.Add(d);
                }

                string[] namesArray = { "Avraham", "Yitshak", "Yaakov", "Sarah", "Rivka", "Rahel", "Leah", "David", "Moshe", "Aharon" };
                string[] firstDigits = { "050", "052", "054" };
                for (int i = 0; i < 10; i++)
                {
                    Customer c = new Customer();
                    c.Id = r.Next(100000000, 1000000000);
                    c.Name = namesArray[i];
                    c.Phone = firstDigits[r.Next(3)] + r.Next(1111111, 10000000);
                    for (int j = 0; j < 7; j++)
                    {
                        c.Phone += (r.Next(0, 11)).ToString();
                    }
                    c.Latitude = r.NextDouble() + r.Next(-90, 90);
                    c.Longitude = r.NextDouble() + r.Next(-180, 80);
                }

            }
        }

    }
}

