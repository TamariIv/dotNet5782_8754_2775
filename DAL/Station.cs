using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public int ChargeSlots { get; set; }

            public override string ToString()
            {
                return string.Format("Id is: {0}\nName of station: {1}\nLongitude is: {2}\nLatitude is: {3}\nNum of charge slots: {4}\n", Id, Name, Longitude, Latitude, ChargeSlots);
            }
        }
    }

}
