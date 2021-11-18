using System;
using IBL.BO;
using System.Collections.Generic;

namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        IDAL.DO.IDal dal;
        double chargeRate;
        double available
        private List<IBL.BO.DroneToList> drones;
        public BlObject()
        {
            dal = new DalObject.DalObject();
            drones = new List<IBL.BO.DroneToList>();
            chargeRate = dal.GetElectricity()[4];

        }

    }
}

