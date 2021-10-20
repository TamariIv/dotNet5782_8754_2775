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
            internal static List<Object> Drones;
            internal static List<Object> Stations;
            internal static List<Object> Customers;
            internal static List<Object> Parcels;

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
                for (int i = 0; i < 5; i++)
                {
                    Drone D = new Drone();
                    D.Id = r.Next(1001, 10000); 
                    D.Model = (r.Next(11, 100)).ToString().ToUpper() + (r.Next(111,999)).ToString(); // for example: SE503
                    D.MaxWeight = (WeightCategories)r.Next(3);
                    Drones.Add(D);
                }

            }
        }
    }
}

