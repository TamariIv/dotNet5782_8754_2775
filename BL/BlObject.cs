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

                //List<IDAL.DO.Parcel> parcelList = parcels.FindAll(p => p.DroneId == droneBl.Id); //find all the parcels that were assigned to drone
                foreach (var parcel in parcels)
                {
                    if (parcel.DroneId == droneBl.Id)
                    {
                        //assigned but wasn't delivered:
                        if (parcel.Delivered == DateTime.MinValue)
                        {
                            droneBl.DroneStatus = DroneStatus.Delivery;
                            //assigned but wasn't picked up:
                            if (parcel.PickedUp == DateTime.MinValue)
                            {
                                int senderId = parcel.SenderId;
                                double senderLatitude = dal.GetCustomer(senderId).Latitude;
                                double senderLongitude = dal.GetCustomer(senderId).Longitude;
                                IDAL.DO.Station st = dal.getClosestStation(senderLatitude, senderLongitude);
                                droneBl.Location = new Location { Latitude = st.Latitude, Longitude = st.Longitude };
                            }
                            //picked up but wasn't delivered
                            else if (parcel.PickedUp != DateTime.MinValue && parcel.Delivered == DateTime.MinValue)
                            {
                                int senderId = parcel.SenderId;
                                double senderLatitude = dal.GetCustomer(senderId).Latitude;
                                double senderLongitude = dal.GetCustomer(senderId).Longitude;
                                droneBl.Location = new Location
                                {
                                    Latitude = senderLatitude,
                                    Longitude = senderLongitude
                                };
                            }
                            Customer customer = GetCustomer(parcel.TargetId);
                            IDAL.DO.Station closestStation = dal.getClosestStation(customer.Location.Latitude, customer.Location.Longitude);
                            double distance1 = Tools.Utils.DistanceCalculation(droneBl.Location.Latitude, droneBl.Location.Longitude, customer.Location.Latitude, customer.Location.Longitude);
                            double distance2 = Tools.Utils.DistanceCalculation(customer.Location.Latitude, customer.Location.Longitude, closestStation.Latitude, closestStation.Longitude);
                            double battery = getBatteryConsumption(parcel.Weight);
                            double minBattery = (distance1 + distance2) * battery;
                            droneBl.Battery = minBattery + r.NextDouble() * (100 - minBattery);
                        }
                        else //the drone is not in delivery
                        {
                            droneBl.DroneStatus = (DroneStatus)r.Next(2); //Maintenance or Available
                            
                            if (droneBl.DroneStatus == DroneStatus.Maintenance)
                            {
                                droneBl.Battery = r.Next(0, 21);

                                //the location is a random station from the stations list:
                                int index = r.Next(dal.GetStations().ToList().Count());
                                IBL.BO.Location location = new Location()
                                {
                                    Latitude = dal.GetStations().ToList()[index].Latitude,
                                    Longitude = dal.GetStations().ToList()[index].Longitude
                                };
                                droneBl.Location = location;
                            }
                            else if (droneBl.DroneStatus == DroneStatus.Available)
                            {                                
                                List<IDAL.DO.Customer> customers = dal.GetCustomersWithParcels(parcels, dal.GetCustomers().ToList());
                                int rand = r.Next(customers.Count());
                                Location location = new Location { Latitude = customers[rand].Latitude, Longitude = customers[rand].Longitude };
                                droneBl.Location = location;

                                IDAL.DO.Station closestStation = dal.getClosestStation(droneBl.Location.Latitude, droneBl.Location.Longitude);
                                double distance = Tools.Utils.DistanceCalculation(droneBl.Location.Latitude, droneBl.Location.Longitude, closestStation.Latitude, closestStation.Longitude);
                                double minBattery = distance * whenAvailable;
                                droneBl.Battery = minBattery + r.NextDouble() * (100 - minBattery);
                            }

                        }
                        break;
                    }
                }
                dronesToList.Add(droneBl);
            }
        }

        private double getBatteryConsumption(IDAL.DO.WeightCategories weight)
        {
            switch (weight)
            {
                case IDAL.DO.WeightCategories.Light:
                    return whenLight;
                case IDAL.DO.WeightCategories.Medium:
                    return whenMedium;
                case IDAL.DO.WeightCategories.Heavy:
                    return whenHeavy;
                default:
                    return whenAvailable;
            }
        }
       
    }
}


