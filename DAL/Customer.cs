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
                return string.Format("Id is: {0}\n Name of customer: {1}\n Phone number: {2}\n longitude is: {3}\n" +
                    " latitude: {4}\n", Id, Name, Phone,Longitude, Latitude);
            }
        }

    }

}
