using System;
using System.Linq;
using IBL.BO;
using System.Collections.Generic;

namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        IDAL.DO.IDal dal;
        public List<IDAL.DO.Drone> drones = new List<IDAL.DO.Drone>();
        private double chargeRate, whenAvailable, whenHeavy, whenMedium, whenLight;
        private List<IBL.BO.DroneToList> dronesToList;

        internal static Random r = new Random();


        public BlObject()
        {
            dal = new DalObject.DalObject();
            dronesToList = new List<IBL.BO.DroneToList>();
            drones = dal.GetDrones().ToList();

            //get the electricity rate from DAL:
            double[] electricity = dal.GetElectricity();
            whenAvailable = electricity[0];
            whenLight = electricity[1];
            whenMedium = electricity[2];
            whenHeavy = electricity[3];
            chargeRate = electricity[4];

            //initializeDrones(drones);

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

                var parcelIndex = parcels.FindIndex(p => p.DroneId == droneDal.Id);
                if (parcelIndex != -1) //there is a parcel that assigned to this drone
                {
                    IDAL.DO.Parcel parcel = parcels[parcelIndex];
                    droneBl.ParcelInDeliveryId = parcel.Id;
                    //assigned but wasn't delivered:
                    if (parcel.Delivered == DateTime.MinValue)
                    {
                        droneBl.DroneStatus = DroneStatus.Delivery;

                        //assigned but wasn't picked up:
                        if (parcel.PickedUp == DateTime.MinValue)
                        {
                            //the location will be in the closest station to the sender
                            double senderLatitude = dal.GetCustomer(parcel.SenderId).Latitude;
                            double senderLongitude = dal.GetCustomer(parcel.SenderId).Longitude;
                            IDAL.DO.Station st = dal.getClosestStation(senderLatitude, senderLongitude);
                            droneBl.Location = new Location { Latitude = st.Latitude, Longitude = st.Longitude };
                        }

                        //picked up but wasn't delivered:
                        else if (parcel.Delivered == DateTime.MinValue)
                        {
                            //the location of the drone will be in the sender's location
                            double senderLatitude = dal.GetCustomer(parcel.SenderId).Latitude;
                            double senderLongitude = dal.GetCustomer(parcel.SenderId).Longitude;
                            droneBl.Location = new Location
                            {
                                Latitude = senderLatitude,
                                Longitude = senderLongitude
                            };
                        }
                        //Battery status will be raffled between a minimal charge that will allow the drone to make the shipment and arrive at the station closest to the shipment destination and a full charge
                        Customer customer = GetCustomer(parcel.TargetId);
                        IDAL.DO.Station closestStation = dal.getClosestStation(customer.Location.Latitude, customer.Location.Longitude);
                        double distance1 = Tools.Utils.DistanceCalculation(droneBl.Location.Latitude, droneBl.Location.Longitude, customer.Location.Latitude, customer.Location.Longitude);
                        double distance2 = Tools.Utils.DistanceCalculation(customer.Location.Latitude, customer.Location.Longitude, closestStation.Latitude, closestStation.Longitude);
                        double battery = getBatteryConsumption(parcel.Weight);
                        double minBattery = (distance1 + distance2) * battery;
                        droneBl.Battery = minBattery + r.NextDouble() * (100 - minBattery);

                    }


                }
                else //the drone is not assigned
                {
                    droneBl.DroneStatus = (DroneStatus)r.Next(2); //Maintenance or Available
                    droneBl.ParcelInDeliveryId = 0; //there is no parcel that assigned to this drone so the parcel id is null(0).
                    if (droneBl.DroneStatus == DroneStatus.Maintenance)
                    {
                        //battery status will be random between 0% - 20%
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
                        //the location is a random betwwen customers that get a parcel
                        List<IDAL.DO.Customer> customers = dal.GetCustomersWithParcels(parcels, dal.GetCustomers().ToList());
                        int rand = r.Next(customers.Count());
                        Location location = new Location { Latitude = customers[rand].Latitude, Longitude = customers[rand].Longitude };
                        droneBl.Location = location;

                        //Battery status will be raffled off between minimal charging that will allow it to reach the station closest to charging and full charging
                        IDAL.DO.Station closestStation = dal.getClosestStation(droneBl.Location.Latitude, droneBl.Location.Longitude);
                        double distance = Tools.Utils.DistanceCalculation(droneBl.Location.Latitude, droneBl.Location.Longitude, closestStation.Latitude, closestStation.Longitude);
                        double minBattery = distance * whenAvailable;
                        droneBl.Battery = minBattery + r.NextDouble() * (100 - minBattery);
                    }

                }

                dronesToList.Add(droneBl);
            }
        }

        //private void initializeDrones(List<IDAL.DO.Drone> drones)
        //{
           
        //}
        /// <summary>
        /// get a weight and returns the appropriate battery consumption
        /// </summary>
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


