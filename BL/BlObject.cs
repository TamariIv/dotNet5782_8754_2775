using System;
using System.Linq;
using IBL.BO;
using System.Collections.Generic;

namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        IDAL.DO.IDal dal;
        public List<IDAL.DO.Drone> drones;
        private double chargeRate, whenAvailable, whenHeavy, whenMedium, whenLight;
        private List<IBL.BO.DroneToList> dronesToList;

        internal static Random r = new Random();


        public BlObject()
        {
            dal = new DalObject.DalObject();
            dronesToList = new List<IBL.BO.DroneToList>();
            drones = dal.GetDrones().ToList();
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

                //assigned but wasn't delivered
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


       
       

   

    }
}


