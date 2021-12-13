using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        public void AddParcel(IBL.BO.Parcel newParcel)
        {
            IDAL.DO.Parcel parcel = new IDAL.DO.Parcel
            {
                SenderId = newParcel.Sender.Id,
                TargetId = newParcel.Target.Id,
                Weight = (IDAL.DO.WeightCategories)newParcel.Weight,
                Priority = (IDAL.DO.Priorities)newParcel.Priority,
                Requested = DateTime.Now,
                Scheduled = null,
                PickedUp = null,
                Delivered = null,
                DroneId = 0
            };
            dal.AddParcel(parcel);
        }

        public IBL.BO.Parcel GetParcel(int id)
        {
            IDAL.DO.Parcel dalParcel = dal.GetParcel(id);
            IBL.BO.Parcel blParcel = new IBL.BO.Parcel
            {
                Id = dalParcel.Id,
                Sender = new IBL.BO.CustomerInParcel { Id = dalParcel.SenderId, Name = dal.GetCustomer(dalParcel.SenderId).Name },
                Target = new IBL.BO.CustomerInParcel { Id = dalParcel.TargetId, Name = dal.GetCustomer(dalParcel.TargetId).Name },
                Weight = (IBL.BO.WeightCategories)dalParcel.Weight,
                Priority = (IBL.BO.Priorities)dalParcel.Priority,
                Requested = dalParcel.Requested,
                Scheduled = dalParcel.Scheduled,
                PickedUp = dalParcel.PickedUp,
                Delivered = dalParcel.Delivered,
                AssignedDrone = new IBL.BO.DroneInParcel { Id = dalParcel.DroneId, Battery = dronesToList.Find(d => d.Id == dalParcel.DroneId).Battery, CurrentLocation = dronesToList.Find(d => d.Id == dalParcel.DroneId).Location }
            };
            return blParcel;
        }

        /// <summary>
        /// create a list of parcelsToList and returns it
        /// </summary>
        public IEnumerable<IBL.BO.ParcelToList> GetListofParcels(Func<IBL.BO.ParcelToList, bool> predicate=null)
        {
            List<IBL.BO.ParcelToList> blParcels = new List<IBL.BO.ParcelToList>();
            foreach (var dalParcel in dal.GetParcels())
            {
                blParcels.Add(convertParcelToParcelToList(dalParcel));
            }
            if (predicate == null)
                return blParcels;
            return blParcels.Where(predicate);
        }

        private IBL.BO.ParcelToList convertParcelToParcelToList(IDAL.DO.Parcel dalParcel)
        {
            IBL.BO.ParcelToList blParcel = new IBL.BO.ParcelToList
            {
                Id = dalParcel.Id,
                SenderName = dal.GetCustomer(dalParcel.SenderId).Name,
                TargetName = dal.GetCustomer(dalParcel.TargetId).Name,
                Weight = (IBL.BO.WeightCategories)dalParcel.Weight,
                Priority = (IBL.BO.Priorities)dalParcel.Priority,
                ParcelStatus = getParcelStatus(dalParcel)
            };
            return blParcel;
        }


        private IBL.BO.ParcelStatus getParcelStatus(IDAL.DO.Parcel parcel)
        {
            if (parcel.Scheduled == null)
                return IBL.BO.ParcelStatus.Requested;
            else if (parcel.PickedUp == null)
                return IBL.BO.ParcelStatus.Assigned;
            else if (parcel.Delivered == null)
                return IBL.BO.ParcelStatus.PickedUp;
            else return IBL.BO.ParcelStatus.Delivered;
        }

        /// <summary>
        /// create a list of parcels that are not assigned to drone and returns this list
        /// </summary>
        //public IEnumerable<IBL.BO.ParcelToList> GetListofParcelsWithoutDrone()
        //{
        //    List<IBL.BO.ParcelToList> parcelsWithoutDrones = new List<IBL.BO.ParcelToList>();
        //    foreach (var dalParcel in dal.GetParcels())
        //    {
        //        if (dalParcel.DroneId == 0) //this parcel is not assigned to drone
        //            parcelsWithoutDrones.Add(convertParcelToParcelToList(dalParcel));
        //    }
        //    return parcelsWithoutDrones;
        //}


    }
}
