using System;
using System.Linq;
using IBL.BO;
using System.Collections.Generic;

namespace BL
{
    public partial class BlObject : /*ParcelBlObject,*/ IBL.IBL
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
                    else if (item.PickedUp != DateTime.MinValue && item.Delivered == DateTime.MinValue)
                    {
                        //מיקום הרחפן יהיה במיקום השולח
                    }
                    //מצב סוללה יוגרל בין טעינה מינימלית שתאפשר לרחפן לבצע את המשלוח ולהגיע לטעינה לתחנה הקרובה ליעד המשלוח לבין טעינה מלאה                 
                }
            }
           
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

        public void AddCustomer(IBL.BO.Customer newCustomer)
        {
            IDAL.DO.Customer dalCustomer = new IDAL.DO.Customer
            {
                Id = newCustomer.Id,
                Name = newCustomer.Name,
                Phone = newCustomer.Phone,
                Longitude = newCustomer.Location.Longitude,
                Latitude = newCustomer.Location.Latitude

            };
            dal.AddCustomer(dalCustomer);
        }

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
    }
}


