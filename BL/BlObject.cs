using System;
using System.Linq;
using IBL.BO;
using System.Collections.Generic;

namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        IDAL.DO.IDal dal;
        private double chargeRate, whenAvailable, whenHeavy, whenMedium, whenLight;
        private List<IBL.BO.DroneToList> dronesToList;

        internal static Random r = new Random();


        public BlObject()
        {
            dal = new DalObject.DalObject();
            dronesToList = new List<IBL.BO.DroneToList>();
            double[] electricity = dal.GetElectricity();
            whenAvailable = electricity[0];
            whenLight = electricity[1];
            whenMedium = electricity[2];
            whenHeavy = electricity[3];
            chargeRate = electricity[4];
        }

        private void droneState()
        {
            List<IDAL.DO.Drone> drones = dal.GetDrones().ToList();
            List<IDAL.DO.Parcel> parcels = dal.GetParcels().ToList();
           
            foreach(var item in parcels)
            {
                IDAL.DO.Drone d = dal.GetDrone(item.DroneId);

                if (item.DroneId != 0 && item.Delivered == DateTime.MinValue)
                {
                    IBL.BO.Drone newDrone = new Drone();
                    newDrone.Id = item.DroneId;
                    newDrone.DroneStatus = Enums.DroneStatus.Delivery;
                    if (item.DroneId != 0 && item.PickedUp == DateTime.MinValue) //the parcel has a drone
                    {
                        //המיקום יהיה בתחנה הקרובה לשולח
                    }
                    else if (item.PickedUp != DateTime.MinValue /*&& item.Delivered == DateTime.MinValue*/)
                    {
                        //מיקום הרחפן יהיה במיקום השולח
                    }
                    //מצב סוללה יוגרל בין טעינה מינימלית שתאפשר לרחפן לבצע את המשלוח ולהגיע לטעינה לתחנה הקרובה ליעד המשלוח לבין טעינה מלאה                 
                }
                else
            }
           
        }
    }
}


