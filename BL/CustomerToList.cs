using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerToList
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Phone { get; init; }
        public List<Parcel> SentAndDelivered { get; init; }
        public List<Parcel> SentAndNotDeliverd { get; init; }
        public List<Parcel> Recieved { get; init; }
        public List<Parcel> InDeliveryToCustomer { get; init; }
    }
}
