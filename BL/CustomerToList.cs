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
        int SentAndDelivered { get; init; }
        int SentAndNotDeliverd { get; init; }
        int Recieved { get; init; }
        int InDeliveryToCustomer { get; init; }
    }
}
