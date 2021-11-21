using System;
using IBL.BO;
using System.Collections.Generic;

namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        IDAL.DO.IDal dal;
        double chargeRate, whenAvailable, whenHeavy, whenMedium, whenLight;
        private List<IBL.BO.DroneToList> dronesToList;

        internal static Random r = new Random();


        public BlObject()
        {
            dal = new DalObject.DalObject();
            dronesToList = new List<IBL.BO.DroneToList>();
            whenAvailable = dal.GetElectricity()[0];
            whenLight = dal.GetElectricity()[1];
            whenMedium = dal.GetElectricity()[2];
            whenHeavy = dal.GetElectricity()[5];
            chargeRate = dal.GetElectricity()[4];
            IEnumerable<IDAL.DO.Drone> drones = dal.GetDrones();
        }

        public void AddStation(Station newStation)
        {
            if (newStation.DronesCharging.Count == 0)
            {
                IDAL.DO.Station dalStation = new IDAL.DO.Station
                {
                    Id = newStation.Id,
                    Name = newStation.Name,
                    ChargeSlots = newStation.AvailableChargeSlots,
                    
                };
                dal.AddStation(dalStation);
            }
        }

        public void AddDrone(Drone newDrone)
        {
            Drone d = newDrone;
            d.Battery = r.Next(20, 41);
            d.DroneStatus = Enums.DroneStatus.Maintenance;
            IDAL.DO.Drone dalDrone = new IDAL.DO.Drone
            {
                Id = newDrone.Id,
                Model = newDrone.Model,
                MaxWeight = (IDAL.DO.WeightCategories)newDrone.MaxWeight
            };
            dal.AddDrone(dalDrone);
        }

    }
}

