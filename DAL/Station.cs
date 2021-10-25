using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DO
    {
        public struct Station
        {
            public int Id;
            public string Name;
            public double Longitude;
            public double Latitude;
            public int ChargeSlots;

            public override string ToString()
            {
                return string.Format("Id is: {0}\n Name of station: {1}\n Longitude is: {2}\n Latitude is: {3}\n" +
                    " Num of charge slots: {4}\n", Id, Name, Longitude, Latitude, ChargeSlots);
            }
        }
    }

}
