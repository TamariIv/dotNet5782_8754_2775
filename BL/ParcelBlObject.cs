using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        public void AddParcel(IBL.BO.ParcelInDelivey newParcel)
        {
            IDAL.DO.Parcel parcel = new IDAL.DO.Parcel
            {
                Id = newParcel.Id,
                SenderId = newParcel.Sender.Id,
                TargetId = newParcel.Target.Id,
                Weight = (IDAL.DO.WeightCategories)newParcel.Weight,
                Priority = (IDAL.DO.Priorities)newParcel.Priority,
                Requested = DateTime.Now,
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
                DroneId = 0
            };
            dal.AddParcel(parcel);
        }
    }
}
