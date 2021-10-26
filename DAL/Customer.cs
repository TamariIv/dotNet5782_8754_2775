using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DO
    {
        public struct Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }

            public override string ToString()
            {
                return string.Format("Id is: {0}\nName of customer: {1}\nPhone number: {2}\nlongitude is: {3}\nlatitude: {4}\n", Id, Name, Phone,Longitude, Latitude);
            }
        }

    }

}
