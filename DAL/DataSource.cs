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
            {   ///initialize the list of the stations:
                Station s = new Station();
                s.Id = r.Next(10000, 100000); //id number with 5 digits
                s.Name = "East Jerusalem";
                s.Longitude = 31.772683666786808; 
                s.Latitude = 35.246193383752214;
                s.CahrgeSlots = r.Next(11);
                Stations.Add(s);

                s = new Station();
                s.Id = r.Next(10000, 100000);
                s.Name = "Center Jerusalem";
                s.Longitude = 31.760491395848746;
                s.Latitude = 35.191162492863846;
                s.CahrgeSlots = r.Next(11);
                Stations.Add(s);


            }
        }

    }
}

