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

            if (oldDalParcel.PickedUp == DateTime.MinValue)
            {
                dal.PickUpParcel(oldDalParcel);

                IDAL.DO.Customer sender = dal.GetCustomer(oldDalParcel.SenderId);
                double distance = Tools.Utis.DistanceCalculation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, sender.Latitude, sender.Longitude);
                double batteryConsumption = getBatteryConsumption((IDAL.DO.WeightCategories)oldDalParcel.Weight);
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

        public void PrintListOfParcels()
        {
            foreach (var parcel in GetListofParcels())
            {
                Console.WriteLine(parcel + "\n");
            }
        }

        public void PrintParcelToList(int id)
        {
            Console.WriteLine(getParcelToList(id));
        }

        public IBL.BO.ParcelToList getParcelToList(int id)
        {
            List<IDAL.DO.Parcel> parcels = dal.GetParcels().ToList();
            IDAL.DO.Parcel dalParcel = dal.GetParcel(id);

            // find the parcel status
            IBL.BO.ParcelStatus parcelStatus;
            if (dalParcel.Scheduled == DateTime.MinValue)
                parcelStatus = IBL.BO.ParcelStatus.Requested;
            else if (dalParcel.PickedUp == DateTime.MinValue)
                parcelStatus = IBL.BO.ParcelStatus.Assigned;
            else if (dalParcel.Delivered == DateTime.MinValue)
                parcelStatus = IBL.BO.ParcelStatus.PickedUp;
            else
                parcelStatus = IBL.BO.ParcelStatus.Delivered;


            IBL.BO.ParcelToList blParcel = new IBL.BO.ParcelToList
            {
                Id = dalParcel.Id,
                SenderName = dal.GetCustomer(dalParcel.SenderId).Name,
                TargetName = dal.GetCustomer(dalParcel.TargetId).Name,
                Weight = (IBL.BO.WeightCategories)dalParcel.Weight,
                Priority = (IBL.BO.Priorities)dalParcel.Priority,
                ParcelStatus = parcelStatus
            };
            return blParcel;
        }

        public List<IBL.BO.ParcelToList> GetListofParcels()
        {
            List<IDAL.DO.Parcel> dalParcels = dal.GetParcels().ToList();
            List<IBL.BO.ParcelToList> blParcels = new List<IBL.BO.ParcelToList>();
            foreach (var dalParcel in dal.GetParcels())
            {
                blParcels.Add(ConvertParcelToBl(dalParcel));
            }
            return blParcels;
        }

        public IBL.BO.ParcelToList ConvertParcelToBl(IDAL.DO.Parcel dalParcel)
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

        public IDAL.DO.Parcel ConvertParcelToDal(IBL.BO.Parcel dalParcel)
        {
            IDAL.DO.Parcel blParcel = new IDAL.DO.Parcel
            {
                Id = dalParcel.Id,
                SenderId = dalParcel.Sender.Id,
                TargetId = dalParcel.Target.Id,
                Weight = (IDAL.DO.WeightCategories)dalParcel.Weight,
                Priority = (IDAL.DO.Priorities)dalParcel.Priority,
                Requested = dalParcel.Requested,
                Scheduled = dalParcel.Scheduled,
                PickedUp = dalParcel.PickedUp,
                Delivered = dalParcel.Delivered,
                DroneId = dalParcel.AssignedDrone.Id
            };
            return blParcel;
        }

    }
}
