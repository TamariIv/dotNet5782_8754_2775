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
            try
            {
                DO.Parcel parcel = new DO.Parcel
                {
                    SenderId = newParcel.Sender.Id,
                    TargetId = newParcel.Target.Id,
                    Weight = (DO.WeightCategories)newParcel.Weight,
                    Priority = (DO.Priorities)newParcel.Priority,
                    Requested = DateTime.Now,
                    Scheduled = null,
                    PickedUp = null,
                    Delivered = null,
                    DroneId = 0
                };
                dal.AddParcel(parcel);
            }
            catch (DO.IdAlreadyExistsException ex)
            {
                throw new IBL.BO.IdAlreadyExistsException(ex.Message);
            }
            catch(DO.NoMatchingIdException ex)
            {
                throw new IBL.BO.NoMatchingIdException(ex.Message);
            }
        }

        public IBL.BO.Parcel GetParcel(int id)
        {
            try
            {
                DO.Parcel dalParcel = dal.GetParcel(id);
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
            catch (DO.NoMatchingIdException ex)
            {
                throw new IBL.BO.NoMatchingIdException(ex.Message);
            }
            catch (DO.IdAlreadyExistsException ex)
            {
                throw new IBL.BO.IdAlreadyExistsException(ex.Message);
            }
        }

        /// <summary>
        /// create a list of parcelsToList and returns it
        /// </summary>
        public IEnumerable<IBL.BO.ParcelToList> GetListofParcels()
        {
            List<IBL.BO.ParcelToList> blParcels = new List<IBL.BO.ParcelToList>();
            foreach (var dalParcel in dal.GetParcels())
            {
                blParcels.Add(convertParcelToParcelToList(dalParcel));
            }
            return blParcels;
        }

        private IBL.BO.ParcelToList convertParcelToParcelToList(DO.Parcel dalParcel)
        {
            IBL.BO.ParcelToList blParcel = new IBL.BO.ParcelToList
            {
                Id = dalParcel.Id,
                SenderName = GetCustomer(dalParcel.SenderId).Name,
                TargetName = GetCustomer(dalParcel.TargetId).Name,
                Weight = (IBL.BO.WeightCategories)dalParcel.Weight,
                Priority = (IBL.BO.Priorities)dalParcel.Priority,
                ParcelStatus = getParcelStatus(dalParcel)
            };
            return blParcel;
        }


        private IBL.BO.ParcelStatus getParcelStatus(DO.Parcel parcel)
        {
            if (parcel.Scheduled == null)
                return IBL.BO.ParcelStatus.Requested;
            else if (parcel.PickedUp == null)
                return IBL.BO.ParcelStatus.Assigned;
            else if (parcel.Delivered == null)
                return IBL.BO.ParcelStatus.PickedUp;
            else return IBL.BO.ParcelStatus.Delivered;
        }

        // <summary>
        // create a list of parcels that are not assigned to drone and returns this list
        // </summary>
        public IEnumerable<IBL.BO.ParcelToList> GetListofParcelsWithoutDrone()
        {
            List<IBL.BO.ParcelToList> parcelsWithoutDrones = new List<IBL.BO.ParcelToList>();
            foreach (var dalParcel in dal.GetParcels(p => p.DroneId == 0)) //send a predicate to DAL
            {
                parcelsWithoutDrones.Add(convertParcelToParcelToList(dalParcel));
            }
            return parcelsWithoutDrones;
        }


    }
}
