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
            internal List<Object> Drones;
            internal List<Object> Station;
            internal List<Object> Customers;
            internal List<Object> Parcels;

            internal class Config
            {
                //internal static int Drone_i = 0;
                //internal static int Station_i = 0;
                //internal static int Customer_i = 0;
                //internal static int Parcel_i = 0;
                //internal static int ParcelNum = 0;
                internal int ParcelId;
            }
        }


    }
}
