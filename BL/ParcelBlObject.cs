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

        public void PickUpParcel(IBL.BO.Drone drone)
        {
            List<IDAL.DO.Parcel> parcels = dal.GetParcels().ToList();
            IDAL.DO.Parcel oldDalParcel;
            if (parcels.Exists(p => p.DroneId == drone.Id))
                oldDalParcel = parcels.Find(p => p.DroneId == drone.Id);
            else throw new NoMatchingIdException($"no parcel can be picked up by drone with id {drone.Id}");

            if (oldDalParcel.PickedUp == DateTime.MinValue) //only drone that assigned to parcel but still didnt pick up can pick up this parcel
            {
                dal.PickUpParcel(oldDalParcel);

                IDAL.DO.Customer sender = dal.GetCustomer(oldDalParcel.SenderId);
                double distance = Tools.Utils.DistanceCalculation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, sender.Latitude, sender.Longitude);
                double batteryConsumption = getBatteryConsumption(oldDalParcel.Weight);
                double battery = distance * batteryConsumption;

                IBL.BO.DroneToList newDrone = new IBL.BO.DroneToList
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = drone.MaxWeight,
                    Battery = drone.Battery - battery,
                    DroneStatus = GetDroneToList(drone.Id).DroneStatus,
                    Location = new IBL.BO.Location { Latitude = sender.Latitude, Longitude = sender.Longitude },
                    ParcelInDeliveryId = oldDalParcel.Id
                };

                IBL.BO.DroneToList oldDrone = dronesToList.Find(d => d.Id == drone.Id);
                dronesToList.Remove(oldDrone);
                dronesToList.Add(newDrone);
            }
            else throw new ImpossibleOprationException("Parcel can't be picked up");
        }

        //public void PrintListOfParcels()
        //{
        //    foreach (var parcel in GetListofParcels())
        //    {
        //        Console.WriteLine(parcel + "\n");
        //    }
        //}
      
        public IBL.BO.Parcel GetParcel(int id)
        {
            List<IDAL.DO.Parcel> parcels = dal.GetParcels().ToList();
            IDAL.DO.Parcel dalParcel = parcels.Find(p => p.Id == id);
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

        //public IBL.BO.ParcelToList GetParcelToList(int id)
        //{
        //    List<IDAL.DO.Parcel> parcels = dal.GetParcels().ToList();
        //    IDAL.DO.Parcel dalParcel = dal.GetParcel(id);

        //    // find the parcel status
        //    IBL.BO.ParcelStatus parcelStatus;
        //    if (dalParcel.Scheduled == DateTime.MinValue)
        //        parcelStatus = IBL.BO.ParcelStatus.Requested;
        //    else if (dalParcel.PickedUp == DateTime.MinValue)
        //        parcelStatus = IBL.BO.ParcelStatus.Assigned;
        //    else if (dalParcel.Delivered == DateTime.MinValue)
        //        parcelStatus = IBL.BO.ParcelStatus.PickedUp;
        //    else
        //        parcelStatus = IBL.BO.ParcelStatus.Delivered;


        //    IBL.BO.ParcelToList blParcel = new IBL.BO.ParcelToList
        //    {
        //        Id = dalParcel.Id,
        //        SenderName = dal.GetCustomer(dalParcel.SenderId).Name,
        //        TargetName = dal.GetCustomer(dalParcel.TargetId).Name,
        //        Weight = (IBL.BO.WeightCategories)dalParcel.Weight,
        //        Priority = (IBL.BO.Priorities)dalParcel.Priority,
        //        ParcelStatus = parcelStatus
        //    };
        //    return blParcel;
        //}

        /// <summary>
        /// create a list of parcelsToList and returns it
        /// </summary>
        public IEnumerable<IBL.BO.ParcelToList> GetListofParcels()
        {
            List<IBL.BO.ParcelToList> blParcels = new List<IBL.BO.ParcelToList>();
            foreach (var dalParcel in dal.GetParcels())
            {
                blParcels.Add(ConvertParcelToParcelToList(dalParcel));
            }
            return blParcels;
        }

        public IBL.BO.ParcelToList ConvertParcelToParcelToList(IDAL.DO.Parcel dalParcel)
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

        public IBL.BO.ParcelStatus getParcelStatus(IDAL.DO.Parcel parcel)
        {
            if (parcel.Scheduled == DateTime.MinValue)
                return IBL.BO.ParcelStatus.Requested;
            else if (parcel.PickedUp == DateTime.MinValue)
                return IBL.BO.ParcelStatus.Assigned;
            else if (parcel.Delivered == DateTime.MinValue)
                return IBL.BO.ParcelStatus.PickedUp;
            else return IBL.BO.ParcelStatus.Delivered;
        }

        public IDAL.DO.Parcel ConvertParcelToDal(IBL.BO.Parcel blParcel)
        {
            IDAL.DO.Parcel dalParcel = new IDAL.DO.Parcel
            {
                Id = blParcel.Id,
                SenderId = blParcel.Sender.Id,
                TargetId = blParcel.Target.Id,
                Weight = (IDAL.DO.WeightCategories)blParcel.Weight,
                Priority = (IDAL.DO.Priorities)blParcel.Priority,
                Requested = blParcel.Requested,
                Scheduled = blParcel.Scheduled,
                PickedUp = blParcel.PickedUp,
                Delivered = blParcel.Delivered,
                DroneId = blParcel.AssignedDrone.Id
            };
            return dalParcel;
        }

        /// <summary>
        /// the function receives a DAL parcel end return the parcel converted to BL parcel
        /// </summary>
        /// <param name="dalParcel"> the IDAL parcel to convert </param>
        /// <returns> the converted BL parcel </returns>
        public IBL.BO.Parcel ConvertParcelToBl(IDAL.DO.Parcel dalParcel)
        {
            IBL.BO.Parcel blParcel = new IBL.BO.Parcel
            {
                Id = dalParcel.Id,
                Sender = new IBL.BO.CustomerInParcel { Id = dalParcel.SenderId, Name = dal.GetCustomer(dalParcel.SenderId).Name, },
                Target = new IBL.BO.CustomerInParcel { Id = dalParcel.TargetId, Name = dal.GetCustomer(dalParcel.TargetId).Name, },
                Weight = (IBL.BO.WeightCategories)dalParcel.Weight,
                Priority = (IBL.BO.Priorities)dalParcel.Priority,
                Requested = dalParcel.Requested,
                Scheduled = dalParcel.Scheduled,
                PickedUp = dalParcel.PickedUp,
                Delivered = dalParcel.Delivered,
                AssignedDrone = new IBL.BO.DroneInParcel { Id = dalParcel.DroneId, Battery = GetDroneToList(dalParcel.DroneId).Battery, CurrentLocation = GetDroneToList(dalParcel.DroneId).Location }
            };
            return blParcel;
        }
        /// <summary>
        /// create a list of parcels that are not assigned to drone and returns this list
        /// </summary>
        public IEnumerable<IBL.BO.ParcelToList> GetListofParcelsWithoutDrone()
        {
            List<IBL.BO.ParcelToList> parcelsWithoutDrones = new List<IBL.BO.ParcelToList>();
            foreach (var dalParcel in dal.GetParcels())
            {
                if(dalParcel.DroneId == 0) //this parcel is not assigned to drone
                //IBL.BO.DroneToList drone = GetDroneToList(dalParcel.DroneId);                
                parcelsWithoutDrones.Add(ConvertParcelToParcelToList(dalParcel));
            }
            return parcelsWithoutDrones;
        }


    }
}
