using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInDelivey
    {
        public int Id { get; set; }
        public bool PickUpStatus { get; set; }
        public Enums.WeightCategories Weight { get; set; }
        public Enums.Priorities Priority { get; set; }
        CustomerInParcel Sender { get; set; }
        CustomerInParcel Target { get; set; }
        public Location PickUpLocation { get; set; }
        public Location TargetLocation { get; set; }
        public double Distance { get; set; }

    }
}
