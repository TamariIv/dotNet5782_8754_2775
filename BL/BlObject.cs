using System;
using IBL.BO;
using System.Collections.Generic;

public partial class BlObject : IBL.IBL
{
    IDAL.DO.IDal dal;
    private List<IBL.BO.Drone> drones;
    public BlObject()
    {
        dal = new DalObject.DalObject();
        drones = new List<IBL.BO.Drone>();
    }


}

