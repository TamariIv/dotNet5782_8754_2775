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
            internal List<Drone> Drones;
            internal static List<Station> Stations;
            internal List<Customer> Customers;
            internal List<Parcel> Parcels;


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
                    s.Lonitude = r.NextDouble() + r.Next(-180, 80);
                    s.Lattitude = r.NextDouble() + r.Next(-90, 90);
                    s.CahrgeSlots = r.Next(1, 6);
                    Stations.Add(s);
                }
               
            }
        }


    }
}
