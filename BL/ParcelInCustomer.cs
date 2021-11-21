using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInCustomer
    {
        public int Id { get; set; }
        public Enums.WeightCategories Weight { get; set; }
        public Enums.Priorities Priority { get; set; }
        public Enums.ParcelStatus SarcelStatus { get; set; }
        public CustomerInParcel TargetOrSender { get; set; }
    }
}
