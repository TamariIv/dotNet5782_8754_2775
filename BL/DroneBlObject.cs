using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        public void UpdateDrone(IBL.BO.Drone newDrone)
        {
            IDAL.DO.Drone dalDrone = dal.GetDrone(newDrone.Id);
            if (newDrone.Id != 0)
                dalDrone.Id = newDrone.Id;
            if (newDrone.Model != "")
                dalDrone.Model = newDrone.Model;
            dal.UpdateDrone(dalDrone);
        }


        public void rechargeDrone(IBL.BO.Drone drone)
        {
            if (drone.DroneStatus == IBL.BO.DroneStatus.Available)
            {
                IDAL.DO.Station tempStation = dal.getClosestStation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude);
                double distance = Tools.Utis.DistanceCalculation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, tempStation.Latitude, tempStation.Longitude);
                double rate = whenAvailable;

                if (tempStation.AvailableChargeSlots != 0 && distance * rate < drone.Battery)
                {
                    IDAL.DO.Drone dalDrone = ConvertDroneToDal(drone);
                    dal.SendDroneToCharge(dalDrone, tempStation);

                    IBL.BO.DroneToList newDrone = new IBL.BO.DroneToList
                    {
                        Id = drone.Id,
                        MaxWeight = drone.MaxWeight,
                        Model = drone.Model,
                        Battery = drone.Battery - distance * rate,
                        Location = new IBL.BO.Location { Latitude = tempStation.Latitude, Longitude = tempStation.Longitude },
                        DroneStatus = IBL.BO.DroneStatus.Maintenance,
                    };
                    IBL.BO.DroneToList oldDrone = dronesToList.Find(d => d.Id == drone.Id);
                    dronesToList.Remove(oldDrone);
                    dronesToList.Add(newDrone);                     
                }
                
                else throw new ImpossibleOprationException("Drone can't be sent to recharge");
            }
        }
        public void CollectPackageByDrone(IBL.BO.Drone drone, IBL.BO.Parcel parcel)
        {
            List<IDAL.DO.Parcel> parcels = dal.GetParcels().ToList();
            if (parcels.Exists(p => p.DroneId == drone.Id && p.PickedUp == DateTime.MinValue))
            {
                IDAL.DO.Parcel dalParcel = ConvertParcelToDal(parcel);
                dal.PickUpParcel(dalParcel);

                IDAL.DO.Customer sender = dal.GetCustomer(parcel.Sender.Id);
                double distance = Tools.Utis.DistanceCalculation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, sender.Latitude, sender.Longitude);
                double batteryConsumption = getBatteryConsumption((IDAL.DO.WeightCategories)parcel.Weight);
                double battery = distance * batteryConsumption;

                IBL.BO.DroneToList newDrone = new IBL.BO.DroneToList
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = drone.MaxWeight,
                    Battery = drone.Battery - battery,
                    Location = new IBL.BO.Location { Latitude = sender.Latitude, Longitude = sender.Longitude }               
                };

                IBL.BO.DroneToList oldDrone = dronesToList.Find(d => d.Id == drone.Id);
                dronesToList.Remove(oldDrone);
                dronesToList.Add(newDrone);
            }
            else throw new ImpossibleOprationException("Parcel can't be picked up"); 
        }

       
        public void deliveryPackage(IBL.BO.Drone drone, IBL.BO.Parcel parcel)
        {
            List<IDAL.DO.Parcel> parcels = dal.GetParcels().ToList();
            if (parcels.Exists(p => p.DroneId==drone.Id && p.PickedUp != DateTime.MinValue && p.Delivered == DateTime.MinValue))
            {
                IDAL.DO.Parcel dalParcel = ConvertParcelToDal(parcel);
                dal.ParcelDelivered(dalParcel);

                IDAL.DO.Customer target = dal.GetCustomer(parcel.Target.Id);
                double distance = Tools.Utis.DistanceCalculation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, target.Latitude, target.Longitude);
                double batteryConsumption = getBatteryConsumption((IDAL.DO.WeightCategories)parcel.Weight);
                double battery = distance * batteryConsumption;

                IBL.BO.DroneToList newDrone = new IBL.BO.DroneToList
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = drone.MaxWeight,
                    Battery = drone.Battery - battery,
                    Location = new IBL.BO.Location { Latitude = target.Latitude, Longitude = target.Longitude },
                    DroneStatus = IBL.BO.DroneStatus.Available
                };

                IBL.BO.DroneToList oldDrone = dronesToList.Find(d => d.Id == drone.Id);
                dronesToList.Remove(oldDrone);
                dronesToList.Add(newDrone);
            }
            else throw new ImpossibleOprationException("parcel can't be delivere");
        }

        private IDAL.DO.Drone ConvertDroneToDal(IBL.BO.Drone drone)
        {
            IDAL.DO.Drone newDrone = new IDAL.DO.Drone()
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight
            };
            return newDrone;
        }
    }
}
