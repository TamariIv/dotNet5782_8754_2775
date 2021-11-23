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
            List<IDAL.DO.Drone> drones = dal.GetDrones().ToList();

            double[] electricity = dal.GetElectricity();
            whenAvailable = electricity[0];
            whenLight = electricity[1];
            whenMedium = electricity[2];
            whenHeavy = electricity[3];
            chargeRate = electricity[4];
            initializeDrones(drones);
        }

        private void initializeDrones(List<IDAL.DO.Drone> drones)
        {
            List<IDAL.DO.Parcel> parcels = dal.GetParcels().ToList();
            DroneToList droneBl;
            foreach (var droneDal in drones)
            {
                droneBl = new DroneToList()
                {
                    Id = droneDal.Id,
                    Model = droneDal.Model,
                    MaxWeight = (IBL.BO.WeightCategories)droneDal.MaxWeight
                };

                List<IDAL.DO.Parcel> parcelList = parcels.FindAll(p => p.DroneId == droneBl.Id);

                // shuycha ach lo supka
                if (parcelList.Exists(p => p.Delivered == DateTime.MinValue)) 
                {
                    droneBl.DroneStatus = DroneStatus.Delivery;
                    foreach (var item in parcelList.Where(item => item.PickedUp == DateTime.MinValue))
                    {                   
                        int senderId = item.SenderId;
                        double senderLatitude = dal.GetCustomer(senderId).Latitude;
                        double senderLongitude = dal.GetCustomer(senderId).Longitude;
                        IDAL.DO.Station st = dal.getClosestStation(senderLatitude, senderLongitude);
                        droneBl.Location = new Location { Latitude = st.Latitude, Longitude = st.Longitude };
                    }  
                     //picked up but wasn't delivered
                    foreach (var item in parcelList.Where(item => item.PickedUp != DateTime.MinValue && item.Delivered == DateTime.MinValue))
                    {
                        int senderId = item.SenderId;
                        double senderLatitude = dal.GetCustomer(senderId).Latitude;
                        double senderLongitude = dal.GetCustomer(senderId).Longitude;
                        droneBl.Location = new Location
                        {
                            Latitude = senderLatitude,
                            Longitude = senderLongitude
                        };
                    }
                    //יש לעדכן מצב סוללה:
                    //מצב סוללה יוגרל בין טעינה מינימלית שתאפשר לרחפן לבצע את המשלוח ולהגיע לטעינה לתחנה הקרובה ליעד המשלוח לבין טעינה מלאה
                }
                else //the drone is not in delivery
                {
                    droneBl.DroneStatus = (DroneStatus)r.Next(2); //Maintenance or Available
                    if (droneBl.DroneStatus == DroneStatus.Maintenance)
                    {
                        //מיקומו יוגרל בין תחנות התחנות הקיימות
                        droneBl.Battery = r.Next(0, 21);
                    }
                    else if (droneBl.DroneStatus == DroneStatus.Available)
                    {
                        //מיקומו יוגרל בין לקוחות שיש חבילות שסופקו להם
                        // מצב סוללה יוגרל בין טעינה מינימלית שתאפשר לו להגיע לתחנה הקרובה לטעינה לבין טעינה מלאה

                    }

                }

                dronesToList.Add(droneBl);
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
            d.DroneStatus = DroneStatus.Maintenance;
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


        public void UpdateDrone(Drone newDrone)
        {
            IDAL.DO.Drone dalDrone = dal.GetDrone(newDrone.Id);
            if (newDrone.Id != 0)
                dalDrone.Id = newDrone.Id;
            if (newDrone.Model != "")
                dalDrone.Model = newDrone.Model;
            dal.UpdateDrone(dalDrone);
        }


        public void UpdateCustomer(Customer newCustomer)
        {
            IDAL.DO.Customer dalCustomer = dal.GetCustomer(newCustomer.Id);
            if (newCustomer.Name != "")
                dalCustomer.Name = newCustomer.Name;
            if (newCustomer.Phone != "")
                dalCustomer.Phone = newCustomer.Phone;
            dal.UpdateCustomer(dalCustomer);
        }

    }
}


