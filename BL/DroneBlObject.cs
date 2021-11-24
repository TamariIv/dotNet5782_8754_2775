using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        public void AddDrone(IBL.BO.Drone newDrone)
        {
            IBL.BO.Drone d = newDrone;
            d.Battery = r.Next(20, 41);
            d.DroneStatus = IBL.BO.DroneStatus.Maintenance;
            IDAL.DO.Drone dalDrone = new IDAL.DO.Drone
            {
                Id = newDrone.Id,
                Model = newDrone.Model,
                MaxWeight = (IDAL.DO.WeightCategories)newDrone.MaxWeight
            };
            dal.AddDrone(dalDrone);
        }

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
                    IBL.BO.Drone newDrone = new IBL.BO.Drone
                    {
                        Id = drone.Id,
                        MaxWeight = drone.MaxWeight,
                        Model = drone.Model,
                        Battery = drone.Battery - distance * rate,
                        CurrentLocation = new IBL.BO.Location { Latitude = tempStation.Latitude, Longitude = tempStation.Longitude },
                        DroneStatus = IBL.BO.DroneStatus.Maintenance,
                    };
                    drones.Remove(drone);
                    tempStation.AvailableChargeSlots--;
                }
                
                else throw new ImpossibleOprationException("Drone can't be sent to recharge");
            }
        }
    }
}
