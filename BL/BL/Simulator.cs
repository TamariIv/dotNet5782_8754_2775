using System;
using BO;
using System.Threading;
using static BL.BlObject;
using System.Linq;
using static System.Math;

namespace BL
{
    class Simulator
    {
        internal const int DELAY = 500;
        internal const double DRONE_SPEED = 1.0;
        public Simulator(BlObject bl, int droneId, Action update, Func<bool> checkStop)
        {
            var AccessIbl = bl;
            var dal = bl.dal;
            DroneToList drone = bl.GetDroneToList(droneId);
            int parcelId = 0;
            double electricity = 0.0;
            double distance = 0.0;
            bool pickedUp = false;
            Customer customer = null;
            DO.Parcel parcel = new DO.Parcel();

            void delivery(int id)
            {
                parcel = dal.GetParcel(id);
                electricity = AccessIbl.getBatteryConsumption(parcel.Weight);
                pickedUp = parcel.PickedUp == null ? false : true;
                customer = bl.GetCustomer(parcel.PickedUp == null ? parcel.TargetId : parcel.SenderId);
            }



            while (!checkStop())
            {
                switch (drone.DroneStatus)
                {
                    case DroneStatus.Available:
                        if (!sleepSimulator()) break;

                        lock (bl) lock (dal)
                            {
                                AccessIbl.DroneToParcel(drone.Id);

                            }
                        break;



                    case DroneStatus.Maintenance:

                        break;



                    case DroneStatus.Delivery:
                        lock (bl) lock (dal)
                            {
                                //try { if (parcelId == 0) delivery(drone.ParcelInDeliveryId); }
                                //catch (DO.IdAlreadyExistsException ex) { throw new BadStatusException("Internal error getting parcel", ex); }
                                distance = Tools.Utils.DistanceCalculation(drone.Location.Latitude, drone.Location.Longitude, customer.Location.Latitude, customer.Location.Longitude);
                            }

                        if (distance < 0.01 || drone.Battery == 0.0)
                            lock (bl) lock (dal)
                                {
                                    drone.Location = customer.Location;
                                    if (pickedUp)
                                    {
                                        dal.ParcelDelivered(parcel);
                                        drone.DroneStatus = DroneStatus.Available;

                                    }
                                    else
                                    {
                                        dal.PickUpParcel(parcel);
                                        customer = bl.GetCustomer(parcel.TargetId);
                                        pickedUp = true;
                                    }
                                }
                        //else
                        //{
                        //    if (!sleepSimulator()) break;
                        //    lock (bl)
                        //    {
                        //        double delta = distance < STEP ? distance : STEP;
                        //        double proportion = delta / distance;
                        //        drone.Battery = Max(0.0, drone.Battery - delta * bl.BatteryUsages[pickedUp ? batteryUsage : DRONE_FREE]);
                        //        double lat = drone.Location.Latitude + (customer.Location.Latitude - drone.Location.Latitude) * proportion;
                        //        double lon = drone.Location.Longitude + (customer.Location.Longitude - drone.Location.Longitude) * proportion;
                        //        drone.Location = new() { Latitude = lat, Longitude = lon };
                        //    }
                        //}
                        break;

                    default:
                        //throw new BadStatusException("Internal error: not available after Delivery...");
                        break;

                }

                update();
            }

        }

        private static bool sleepSimulator()
        {
            try { Thread.Sleep(DELAY); } catch (ThreadInterruptedException) { return false; }
            return true;
        }
    }
}


