using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        public void AddDrone(IBL.BO.DroneToList newDrone, int stationId)
        {
            newDrone.Battery = r.Next(20, 41);
            newDrone.DroneStatus = IBL.BO.DroneStatus.Maintenance;
            IDAL.DO.Station s;
            if ((dal.GetStations()).ToList().Exists(station => station.Id == stationId))
            {
                s = (dal.GetStations()).ToList().Find(station => station.Id == stationId);
            }
            else throw new NoMatchingIdException($"station with id {stationId} doesn't exist !!");
            newDrone.Location = new IBL.BO.Location
            {
                Longitude = s.Longitude,
                Latitude = s.Latitude
            };
            newDrone.ParcelInDeliveryId = 0;

            IDAL.DO.Drone dalDrone = new IDAL.DO.Drone
            {
                Id = newDrone.Id,
                Model = newDrone.Model,
                MaxWeight = (IDAL.DO.WeightCategories)newDrone.MaxWeight
            };
            dal.SendDroneToCharge(dalDrone, s);
            dal.AddDrone(dalDrone);

            dronesToList.Add(newDrone);
        }

        public void UpdateDrone(IBL.BO.Drone newDrone)
        {
            IDAL.DO.Drone dalDrone = dal.GetDrone(newDrone.Id);
            IBL.BO.DroneToList blDrone = GetDroneToList(newDrone.Id);
            if (newDrone.Id.ToString() != "" && newDrone.Model != "")
            {
                dalDrone.Model = newDrone.Model;
                dal.UpdateDrone(dalDrone);
                blDrone.Model = newDrone.Model;
                UpdateBlDrone(blDrone);
            }
            else throw new NoUpdateException("no update was received\n");
        }

        public void UpdateBlDrone(IBL.BO.DroneToList newDrone)
        {
            dronesToList.Remove(GetDroneToList(newDrone.Id));
            dronesToList.Add(newDrone);
        }

        public void FreeDrone(int droneId, double timeInCharging)
        {
            IBL.BO.DroneToList blDrone;
            blDrone = dronesToList.Find(d => d.Id == droneId);
            if (blDrone.Id == droneId)
            {
                if (blDrone.DroneStatus == IBL.BO.DroneStatus.Maintenance)
                {
                    blDrone.Battery += (int)(timeInCharging * chargeRate);
                    blDrone.DroneStatus = IBL.BO.DroneStatus.Available;

                    IDAL.DO.Drone dalDrone = dal.GetDrone(droneId);
                    //IDAL.DO.Station dalStation = dal.GetStation(blDrone.Id);
                    //dalStation.ChargeSlots++;
                    //IDAL.DO.DroneCharge droneCharge = dal.GetDroneCharge(droneId);
                    dal.SendDroneFromStation(dalDrone);

                    IBL.BO.DroneToList oldBlDrone = GetDroneToList(droneId);
                    dronesToList.Remove(oldBlDrone);
                    dronesToList.Add(blDrone);
                }
                else throw new ImpossibleOperation("drone can't be freed from chraging\n");
            }
            else throw new NoMatchingIdException($"drone with id {droneId} doesn't exist !!");
        }

        public IBL.BO.DroneToList GetDroneToList(int id)
        {
            IBL.BO.DroneToList d = new IBL.BO.DroneToList();
            if (dronesToList.Exists(drone => drone.Id == id))
            {
                d = dronesToList.Find(drone => drone.Id == id);
                return d;
            }
            else throw new NoMatchingIdException($"drone with id {id} doesn't exist !!");
        }

        public IBL.BO.Drone GetDrone(int id)
        {
            IBL.BO.Drone d = new IBL.BO.Drone();
            if (dronesToList.Exists(drone => drone.Id == id))
            {
                d = ConvertDroneToListToDrone(dronesToList.Find(drone => drone.Id == id));
                return d;
            }
            else throw new NoMatchingIdException($"drone with id {id} doesn't exist !!");
        }

        public IBL.BO.Drone ConvertDroneToListToDrone(IBL.BO.DroneToList d)
        {
            IDAL.DO.Parcel parcel = dal.GetParcel(d.ParcelInDeliveryId);
            IBL.BO.Drone newDrone = new IBL.BO.Drone
            {
                Id = d.Id,
                Model = d.Model,
                MaxWeight = d.MaxWeight,
                Battery = d.Battery,
                DroneStatus = d.DroneStatus,
                ParcelInDelivery = new IBL.BO.ParcelInDelivey
                {
                    Id = parcel.Id,
                    PickUpStatus = (getParcelStatus(parcel) == IBL.BO.ParcelStatus.PickedUp || getParcelStatus(parcel) == IBL.BO.ParcelStatus.Delivered ? true : false),
                    Weight = (IBL.BO.WeightCategories)parcel.Weight,
                    Priority = (IBL.BO.Priorities)parcel.Priority,
                    Sender = new IBL.BO.CustomerInParcel { Id = dal.GetCustomer(parcel.SenderId).Id, Name = dal.GetCustomer(parcel.SenderId).Name },
                    Target = new IBL.BO.CustomerInParcel { Id = dal.GetCustomer(parcel.TargetId).Id, Name = dal.GetCustomer(parcel.TargetId).Name },
                    PickUpLocation = new IBL.BO.Location { Latitude = dal.GetCustomer(parcel.SenderId).Latitude, Longitude = dal.GetCustomer(parcel.SenderId).Longitude },
                    TargetLocation = new IBL.BO.Location { Latitude = dal.GetCustomer(parcel.TargetId).Latitude, Longitude = dal.GetCustomer(parcel.TargetId).Longitude },
                    Distance = Tools.Utils.DistanceCalculation(dal.GetCustomer(parcel.SenderId).Latitude, dal.GetCustomer(parcel.SenderId).Longitude, dal.GetCustomer(parcel.TargetId).Latitude, dal.GetCustomer(parcel.TargetId).Longitude)
                },
                CurrentLocation = d.Location
            };
            return newDrone;
        }

        public void DroneToParcel(int id)
        {
            IBL.BO.DroneToList blDrone = GetDroneToList(id);
            if (blDrone.Id == id)
            {
                if (blDrone.DroneStatus == IBL.BO.DroneStatus.Maintenance)
                {

                    // make list of parcels with highest priority possible
                    IEnumerable<IDAL.DO.Parcel> parcels = dal.GetParcels().Where(parcel => parcel.Priority == IDAL.DO.Priorities.Emergency);
                    if (!parcels.Any())
                        parcels = dal.GetParcels().Where(parcel => parcel.Priority == IDAL.DO.Priorities.Rapid);
                    if (!parcels.Any())
                        parcels = dal.GetParcels().Where(parcel => parcel.Priority == IDAL.DO.Priorities.Regular);
                    if (!parcels.Any())
                        throw new EmptyListException("no parcel was found\n");


                    // delete from the list parcels that are too heavy for the drone
                    parcels = parcels.Where(parcel => (int)parcel.Weight >= (int)blDrone.MaxWeight);


                    // find the closest parcel

                    // find the longest distance the drone can fly without carrying a parcel and when carrying a parcel of a specific weight
                    double possibleDistanceWithoutParcel = blDrone.Battery / whenAvailable;
                    double possibleDistanceWithParcel = 0;

                    // find what is the priority of the parcels in parcel
                    int rate = 0;
                    if (parcels.First().Weight == IDAL.DO.WeightCategories.Heavy)
                    {
                        possibleDistanceWithParcel = blDrone.Battery / whenHeavy;
                        rate = (int)whenHeavy;
                    }
                    else if (parcels.First().Weight == IDAL.DO.WeightCategories.Medium)
                    {
                        possibleDistanceWithParcel = blDrone.Battery / whenMedium;
                        rate = (int)whenMedium;
                    }
                    else if (parcels.First().Weight == IDAL.DO.WeightCategories.Light)
                    {
                        possibleDistanceWithParcel = blDrone.Battery / whenLight;
                        rate = (int)whenLight;
                    }

                    double fromDroneToSender;
                    double fromSenderToTarget;
                    double fromTargetToStation;

                    double minDistance = 0;

                    // find the distances of the first parcel in the list
                    IDAL.DO.Customer tmpSender = dal.GetCustomer(parcels.First().SenderId);
                    IDAL.DO.Customer tmpTarget = dal.GetCustomer(parcels.First().TargetId);
                    IDAL.DO.Station s = dal.getClosestStation(tmpTarget.Latitude, tmpTarget.Longitude);

                    fromDroneToSender = Tools.Utils.DistanceCalculation(tmpSender.Latitude, tmpSender.Longitude, blDrone.Location.Latitude, blDrone.Location.Longitude);
                    fromSenderToTarget = Tools.Utils.DistanceCalculation(tmpSender.Latitude, tmpSender.Longitude, tmpTarget.Latitude, tmpTarget.Longitude);
                    fromTargetToStation = Tools.Utils.DistanceCalculation(tmpTarget.Latitude, tmpTarget.Longitude, s.Latitude, s.Longitude);
                    minDistance = fromDroneToSender + fromSenderToTarget + fromTargetToStation;

                    // find the parcel with the minimum distance the drone has to fly
                    IDAL.DO.Parcel minDistanceParcel = new IDAL.DO.Parcel();
                    bool parcelIsFound = false;
                    foreach (var p in parcels)
                    {
                        tmpSender = dal.GetCustomer(p.SenderId);
                        tmpTarget = dal.GetCustomer(p.TargetId);

                        fromDroneToSender = Tools.Utils.DistanceCalculation
                            (blDrone.Location.Latitude, blDrone.Location.Longitude, tmpSender.Latitude, tmpSender.Longitude);
                        fromSenderToTarget = Tools.Utils.DistanceCalculation
                            (tmpSender.Latitude, tmpSender.Longitude, tmpTarget.Latitude, tmpTarget.Longitude);
                        fromTargetToStation = Tools.Utils.DistanceCalculation
                            (tmpTarget.Latitude, tmpTarget.Longitude, s.Latitude, s.Longitude);

                        if ((fromDroneToSender * whenAvailable) + (fromSenderToTarget * rate) + (fromTargetToStation * whenAvailable) >= blDrone.Battery
                            && (fromDroneToSender + fromSenderToTarget + fromTargetToStation) <= minDistance)
                        {
                            minDistanceParcel = p;
                            minDistance = fromDroneToSender + fromSenderToTarget + fromTargetToStation;
                            parcelIsFound = true;
                        }
                    }

                    // make the necessary updates in the parcel and drone or throw exception if no parcel matched the conditions 
                    if (!parcelIsFound)
                        throw new ImpossibleOperation("there is no parcel the drone can carry\n"); // איזה חריגה לזרוק

                    dal.MatchDroneToParcel(minDistanceParcel, dal.GetDrone(id)); // make the update in dal


                    // למה בכלל צריך את זה???
                    //blDrone.DroneStatus = IBL.BO.DroneStatus.Delivery;
                    //minDistanceParcel.Scheduled = DateTime.Now;
                    //minDistanceParcel.DroneId = id;
                    //IBL.BO.CustomerInParcel sender = new IBL.BO.CustomerInParcel
                    //{
                    //    Id = minDistanceParcel.SenderId,
                    //    Name = dal.GetCustomer(minDistanceParcel.SenderId).Name
                    //};
                    //IBL.BO.CustomerInParcel target = new IBL.BO.CustomerInParcel
                    //{
                    //    Id = minDistanceParcel.TargetId,
                    //    Name = dal.GetCustomer(minDistanceParcel.TargetId).Name
                    //};
                    //tmpSender = dal.GetCustomer(minDistanceParcel.SenderId);
                    //tmpTarget = dal.GetCustomer(minDistanceParcel.TargetId);
                    //IBL.BO.Location senderLocation = new IBL.BO.Location
                    //{
                    //    Latitude = tmpSender.Latitude,
                    //    Longitude = tmpSender.Longitude
                    //};
                    //IBL.BO.Location targetLocation = new IBL.BO.Location
                    //{
                    //    Latitude = tmpTarget.Latitude,
                    //    Longitude = tmpTarget.Longitude
                    //};
                    //IBL.BO.ParcelInDelivey parcelInDrone = new IBL.BO.ParcelInDelivey
                    //{
                    //    Id = minDistanceParcel.Id,
                    //    PickUpStatus = false,
                    //    Weight = (IBL.BO.WeightCategories)minDistanceParcel.Weight,
                    //    Priority = (IBL.BO.Priorities)minDistanceParcel.Priority,
                    //    Sender = sender,
                    //    Target = target,
                    //    PickUpLocation = senderLocation,
                    //    TargetLocation = targetLocation,
                    //    Distance = Tools.utils.DistanceCalculation(tmpSender.Latitude, tmpSender.Longitude, tmpTarget.Latitude, tmpTarget.Longitude)
                    //};
                    blDrone.ParcelInDeliveryId = minDistanceParcel.Id;
                }
            }
            else throw new NoMatchingIdException($"drone with id {id} doesn't exist\n");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drone"></param>
        public void rechargeDrone(IBL.BO.Drone drone)
        {
            if (drone.DroneStatus == IBL.BO.DroneStatus.Available)
            {
                IDAL.DO.Station tempStation = dal.getClosestStation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude);
                double distance = Tools.Utils.DistanceCalculation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, tempStation.Latitude, tempStation.Longitude);
                double rate = whenAvailable;

                if (tempStation.AvailableChargeSlots != 0 && distance * rate < drone.Battery)
                {
                    IDAL.DO.Drone dalDrone = ConvertDroneToDal(drone);
                    dal.SendDroneToCharge(dalDrone, tempStation);

                    IBL.BO.DroneToList newDrone = new IBL.BO.DroneToList
                    {
                        Id = drone.Id,
                        MaxWeight = drone.MaxWeight,
                        Model = drone.Model,
                        Battery = drone.Battery - distance * rate,
                        Location = new IBL.BO.Location { Latitude = tempStation.Latitude, Longitude = tempStation.Longitude },
                        DroneStatus = IBL.BO.DroneStatus.Maintenance,
                    };
                    IBL.BO.DroneToList oldDrone = dronesToList.Find(d => d.Id == drone.Id);
                    dronesToList.Remove(oldDrone);
                    dronesToList.Add(newDrone);
                }

                else throw new ImpossibleOprationException("Drone can't be sent to recharge");
            }
        }

        public void deliveryPackage(IBL.BO.Drone drone)
        {
            List<IDAL.DO.Parcel> parcels = dal.GetParcels().ToList();
          
            if (parcels.Exists(p => p.DroneId==drone.Id && p.PickedUp != DateTime.MinValue && p.Delivered == DateTime.MinValue))
            {
                IDAL.DO.Parcel dalParcel = parcels.Find(p => p.DroneId == drone.Id);
                dal.ParcelDelivered(dalParcel);

                IDAL.DO.Customer target = dal.GetCustomer(dalParcel.TargetId);
                double distance = Tools.Utils.DistanceCalculation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, target.Latitude, target.Longitude);
                double batteryConsumption = getBatteryConsumption(dalParcel.Weight);
                double battery = distance * batteryConsumption;

                IBL.BO.DroneToList newDrone = new IBL.BO.DroneToList
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = drone.MaxWeight,
                    Battery = drone.Battery - battery,
                    Location = new IBL.BO.Location { Latitude = target.Latitude, Longitude = target.Longitude },
                    DroneStatus = IBL.BO.DroneStatus.Available
                };

                IBL.BO.DroneToList oldDrone = dronesToList.Find(d => d.Id == drone.Id);
                dronesToList.Remove(oldDrone);
                dronesToList.Add(newDrone);
            }
            else throw new ImpossibleOprationException("parcel can't be delivered");
        }

        /// <summary>
        /// returns a copy of the drones list
        /// </summary>
        public IEnumerable<IBL.BO.DroneToList> GetListOfDrones()
        {
            List<IBL.BO.DroneToList> copyDronesToList = new List<IBL.BO.DroneToList>();
            foreach (var drone in dronesToList)
            {
                copyDronesToList.Add(drone);
            }
            return copyDronesToList;
        }

        /// <summary>
        /// the function receives a BL drone and return BL DroneToList 
        /// </summary>
        /// <param name="dalDrone"> the drone to convert </param>
        /// <returns></returns>
        private IDAL.DO.Drone ConvertDroneToDal(IBL.BO.Drone dalDrone)
        {
            IDAL.DO.Drone newDrone = new IDAL.DO.Drone()
            {
                Id = dalDrone.Id,
                Model = dalDrone.Model,
                MaxWeight = (IDAL.DO.WeightCategories)dalDrone.MaxWeight
            };
            return newDrone;
        }
    }
}
