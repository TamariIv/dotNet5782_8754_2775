using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BlObject : IBL
    {
        public void AddParcel(BO.Parcel newParcel)
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
                throw new BO.IdAlreadyExistsException(ex.Message);
            }
            catch(DO.NoMatchingIdException ex)
            {
                throw new BO.NoMatchingIdException(ex.Message);
            }
        }

        public BO.Parcel GetParcel(int id)
        {
            try
            {
                DO.Parcel dalParcel = dal.GetParcel(id);
                BO.Parcel blParcel = new BO.Parcel
                {
                    Id = dalParcel.Id,
                    Sender = new BO.CustomerInParcel { Id = dalParcel.SenderId, Name = dal.GetCustomer(dalParcel.SenderId).Name },
                    Target = new BO.CustomerInParcel { Id = dalParcel.TargetId, Name = dal.GetCustomer(dalParcel.TargetId).Name },
                    Weight = (BO.WeightCategories)dalParcel.Weight,
                    Priority = (BO.Priorities)dalParcel.Priority,
                    Requested = dalParcel.Requested,
                    Scheduled = dalParcel.Scheduled,
                    PickedUp = dalParcel.PickedUp,
                    Delivered = dalParcel.Delivered,
                    AssignedDrone = new BO.DroneInParcel { Id = dalParcel.DroneId, Battery = dronesToList.Find(d => d.Id == dalParcel.DroneId).Battery, CurrentLocation = dronesToList.Find(d => d.Id == dalParcel.DroneId).Location }
                };
                return blParcel;
            }
            catch (DO.NoMatchingIdException ex)
            {
                throw new BO.NoMatchingIdException(ex.Message);
            }
            catch (DO.IdAlreadyExistsException ex)
            {
                throw new BO.IdAlreadyExistsException(ex.Message);
            }
        }

        /// <summary>
        /// create a list of parcelsToList and returns it
        /// </summary>
        public IEnumerable<BO.ParcelToList> GetListofParcels()
        {
            List<BO.ParcelToList> blParcels = new List<BO.ParcelToList>();
            foreach (var dalParcel in dal.GetParcels())
            {
                blParcels.Add(convertParcelToParcelToList(dalParcel));
            }
            return blParcels;
        }

        private BO.ParcelToList convertParcelToParcelToList(DO.Parcel dalParcel)
        {
            BO.ParcelToList blParcel = new BO.ParcelToList
            {
                Id = dalParcel.Id,
                SenderName = GetCustomer(dalParcel.SenderId).Name,
                TargetName = GetCustomer(dalParcel.TargetId).Name,
                Weight = (BO.WeightCategories)dalParcel.Weight,
                Priority = (BO.Priorities)dalParcel.Priority,
                ParcelStatus = getParcelStatus(dalParcel)
            };
            return blParcel;
        }


        private BO.ParcelStatus getParcelStatus(DO.Parcel parcel)
        {
            if (parcel.Scheduled == null)
                return BO.ParcelStatus.Requested;
            else if (parcel.PickedUp == null)
                return BO.ParcelStatus.Assigned;
            else if (parcel.Delivered == null)
                return BO.ParcelStatus.PickedUp;
            else return BO.ParcelStatus.Delivered;
        }

        // <summary>
        // create a list of parcels that are not assigned to drone and returns this list
        // </summary>
        public IEnumerable<BO.ParcelToList> GetListofParcelsWithoutDrone()
        {
            List<BO.ParcelToList> parcelsWithoutDrones = new List<BO.ParcelToList>();
            foreach (var dalParcel in dal.GetParcels(p => p.DroneId == 0)) //send a predicate to DAL
            {
                parcelsWithoutDrones.Add(convertParcelToParcelToList(dalParcel));
            }
            return parcelsWithoutDrones;
        }


    }
}
