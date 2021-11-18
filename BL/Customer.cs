using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class Customer
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Phone { get; init; }
        public Location Location { get; init; }
        public List<Parcel> send;
        public List<Parcel> receive;


        public override string ToString()
        {
            return string.Format("Id is: {0}\nName of customer: {1}\nPhone number: {2}\nLocation: {4}\n", Id, Name, Phone, Location);
        }


    }

}
