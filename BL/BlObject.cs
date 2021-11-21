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

        void AddStation(Station newStation)
        {

        }

    }
}

