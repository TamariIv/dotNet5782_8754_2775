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
                    IBL.BO.DroneToList tempDrone = dronesToList.Find(d => d.Id == drone.Id);
                    dronesToList.Remove(tempDrone);
                    dronesToList.Add(newDrone);                     
                }
                
                else throw new ImpossibleOprationException("Drone can't be sent to recharge");
            }
        }
        public void CollectPackageByDrone(IBL.BO.Drone drone, IBL.BO.Parcel)
        {
            if()
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
