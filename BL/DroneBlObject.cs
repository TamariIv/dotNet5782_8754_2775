﻿using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        public void AddDrone(IBL.BO.Drone newDrone, int stationId)
        {
            try
            {
                Station station = GetStation(stationId);
                DroneToList drone = new DroneToList
                {
                    Id = newDrone.Id,
                    Model = newDrone.Model,
                    MaxWeight = newDrone.MaxWeight,
                    Battery = r.Next(20, 41),
                    DroneStatus = DroneStatus.Maintenance,
                    Location = station.Location,
                    ParcelInDeliveryId = 0
                };
                dronesToList.Add(drone);

                IDAL.DO.Drone dalDrone = new IDAL.DO.Drone
                {
                    Id = newDrone.Id,
                    Model = newDrone.Model,
                    MaxWeight = (IDAL.DO.WeightCategories)newDrone.MaxWeight
                };
                IDAL.DO.Station dalStation = dal.GetStation(stationId);
                dal.AddDrone(dalDrone);
                dal.SendDroneToCharge(dalDrone, dalStation);
            }
            catch (IDAL.DO.IdAlreadyExistsException)
            {
                throw new IdAlreadyExistsException();
            }
        }

        /// <summary>
        /// the function receives a drone with a possible change in the model and updates the old one
        /// </summary>
        /// <param name="newDrone"> the updated drone </param>
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

        /// <summary>
        ///  the function receives an updated drone, deletes the old drone with the same id and adds the new one
        /// </summary>
        /// <param name="newDrone"> the updated drone </param>
        public void UpdateBlDrone(IBL.BO.DroneToList newDrone)
        {
            dronesToList.Remove(GetDroneToList(newDrone.Id));
            dronesToList.Add(newDrone);
        }

        /// <summary>
        /// the function receives a drone and send it to charge in the closest station 
        /// </summary>
        /// <param name="drone"></param>
        public void rechargeDrone(IBL.BO.Drone drone)
        {
            if (drone.DroneStatus != IBL.BO.DroneStatus.Available)
                throw new ImpossibleOprationException("Drone can't be sent to recharge");

            IDAL.DO.Station tempStation = dal.getClosestStation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude);
            double distance = Tools.Utils.DistanceCalculation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, tempStation.Latitude, tempStation.Longitude);
            double rate = whenAvailable;

            if (tempStation.AvailableChargeSlots > 0 && distance * rate < drone.Battery)
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
                IBL.BO.DroneToList oldDrone = GetDroneToList(drone.Id);
                dronesToList.Remove(oldDrone);
                dronesToList.Add(newDrone);
            }

            else throw new ImpossibleOprationException("Drone can't be sent to recharge");
        }
        /// <summary>
        /// Release drone from charging and update the fields accordingly
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="timeInCharging"></param>
        public void FreeDrone(int droneId, double timeInCharging)
        {
            IBL.BO.DroneToList blDrone = GetDroneToList(droneId);
            if (blDrone.DroneStatus == IBL.BO.DroneStatus.Maintenance)
            {
                dronesToList.Remove(blDrone);
                blDrone.Battery += (int)(timeInCharging * chargeRate);
                blDrone.DroneStatus = IBL.BO.DroneStatus.Available;
                dronesToList.Add(blDrone);

                IDAL.DO.Drone dalDrone = dal.GetDrone(droneId);
                dal.SendDroneFromStation(dalDrone);

            }
            else throw new ImpossibleOperation("drone can't be freed from chraging\n");
        }

        /// <summary>
        /// the function receives an id and returns the bl droneToList with the same id
        /// </summary>
        /// <param name="id"> id of the drone to search </param>
        /// <returns> a bl droneToList with the id that was received </returns>
        public IBL.BO.DroneToList GetDroneToList(int id)
        {
            int droneIndex = dronesToList.FindIndex(d => d.Id == id);
            if (droneIndex != -1) //this id of drone exists
                return dronesToList[droneIndex];
            throw new NoMatchingIdException($"drone with id {id} doesn't exist !!");
        }

        /// <summary>
        /// the function receives an id and returns the bl drone with the same id
        /// </summary>
        /// <param name="id"> id of the drone to search </param>
        /// <returns> a bl drone with the id that was received </returns>
        public IBL.BO.Drone GetDrone(int id)
        {
            IBL.BO.Drone d = new IBL.BO.Drone();
            d = ConvertDroneToListToDrone(GetDroneToList(id));
            return d; 
        }

        /// <summary>
        /// the function receives a bl droneToList and return the equal dal drone
        /// </summary>
        /// <param name="d"> the bl droneToList </param>
        /// <returns></returns>
        public IBL.BO.Drone ConvertDroneToListToDrone(IBL.BO.DroneToList d)
        {
            IBL.BO.ParcelInDelivey parcelInDrone;
            if (d.ParcelInDeliveryId == 0)
            {
                parcelInDrone = new IBL.BO.ParcelInDelivey();
            }
            else
            {
                IDAL.DO.Parcel parcel = dal.GetParcel(d.ParcelInDeliveryId);
                parcelInDrone = new IBL.BO.ParcelInDelivey
                {
                    Id = parcel.Id,
                    PickUpStatus = getParcelStatus(parcel) == IBL.BO.ParcelStatus.PickedUp || getParcelStatus(parcel) == IBL.BO.ParcelStatus.Delivered ? true : false,
                    Weight = (IBL.BO.WeightCategories)parcel.Weight,
                    Priority = (IBL.BO.Priorities)parcel.Priority,
                    Sender = new IBL.BO.CustomerInParcel { Id = dal.GetCustomer(parcel.SenderId).Id, Name = dal.GetCustomer(parcel.SenderId).Name },
                    Target = new IBL.BO.CustomerInParcel { Id = dal.GetCustomer(parcel.TargetId).Id, Name = dal.GetCustomer(parcel.TargetId).Name },
                    PickUpLocation = new IBL.BO.Location { Latitude = dal.GetCustomer(parcel.SenderId).Latitude, Longitude = dal.GetCustomer(parcel.SenderId).Longitude },
                    TargetLocation = new IBL.BO.Location { Latitude = dal.GetCustomer(parcel.TargetId).Latitude, Longitude = dal.GetCustomer(parcel.TargetId).Longitude },
                    Distance = Tools.Utils.DistanceCalculation(dal.GetCustomer(parcel.SenderId).Latitude, dal.GetCustomer(parcel.SenderId).Longitude, dal.GetCustomer(parcel.TargetId).Latitude, dal.GetCustomer(parcel.TargetId).Longitude)
                };
            }

            IBL.BO.Drone newDrone = new IBL.BO.Drone
            {
                Id = d.Id,
                Model = d.Model,
                MaxWeight = d.MaxWeight,
                Battery = d.Battery,
                DroneStatus = d.DroneStatus,
                ParcelInDelivery = parcelInDrone,
                CurrentLocation = d.Location
            };
            return newDrone;
        }

        /// <summary>
        /// the function receives a drone id anf finds a parcel that can be assigned to it
        /// </summary>
        /// <param name="id"> the id of the drone </param>
        public void DroneToParcel(int id)
        {
            IBL.BO.DroneToList blDrone = GetDroneToList(id);

            if (blDrone.DroneStatus == IBL.BO.DroneStatus.Available)
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

                // find a parcel in that is [ossible for the drone to take
                IDAL.DO.Parcel posibleDistanceParcel = parcels.First();
                IBL.BO.Parcel finalParcel = new IBL.BO.Parcel();
                bool parcelWasFound = false;
                foreach (var p in parcels)
                {
                    IDAL.DO.Customer tempSender = dal.GetCustomer(p.SenderId);
                    IDAL.DO.Customer tempTarget = dal.GetCustomer(p.TargetId);
                    IDAL.DO.Station closestStation = dal.getClosestStation(tempTarget.Latitude, tempTarget.Longitude);
                    double droneToSenderBatterty = Tools.Utils.DistanceCalculation(blDrone.Location.Latitude, blDrone.Location.Longitude, tempSender.Latitude, tempSender.Longitude) * whenAvailable;
                    double senderToTargetBattery = Tools.Utils.DistanceCalculation(tempSender.Latitude, tempSender.Longitude, tempTarget.Latitude, tempTarget.Longitude) * getBatteryConsumption(p.Weight);
                    double targetToStationBattery = Tools.Utils.DistanceCalculation(tempTarget.Latitude, tempTarget.Longitude, closestStation.Latitude, closestStation.Longitude) * whenAvailable;
                    if (droneToSenderBatterty + senderToTargetBattery + targetToStationBattery <= blDrone.Battery)
                    {
                        parcelWasFound = true;
                        finalParcel = GetParcel(p.Id);
                        break;
                    }
                }

                // make the necessary updates in the parcel and drone or throw exception if no parcel matched the conditions 
                if (!parcelWasFound)
                    throw new ImpossibleOperation("there is no parcel the drone can carry\n");

                dal.MatchDroneToParcel(dal.GetParcel(finalParcel.Id), dal.GetDrone(id)); // make the update in dal

                blDrone.ParcelInDeliveryId = finalParcel.Id;
                blDrone.DroneStatus = DroneStatus.Delivery;
                UpdateBlDrone(blDrone);


            }
            else throw new ImpossibleOperation("the drone is not available\n");
        }
        /// <summary>
        /// pick up parcel by drone - update the drone and the parcel in accordance
        /// </summary>
        /// <param name="drone"></param>
        public void PickUpParcel(Drone drone)
        {
            List<IDAL.DO.Parcel> parcels = dal.GetParcels().ToList();
            IDAL.DO.Parcel oldDalParcel;
            int parcelIndex = parcels.FindIndex(p => p.DroneId == drone.Id);
            if (parcelIndex != -1) //this parcel is assigned
                oldDalParcel = parcels[parcelIndex];
            else throw new NoMatchingIdException($"no parcel can be picked up by drone with id {drone.Id}");

            if (oldDalParcel.PickedUp == null) //only drone that assigned to parcel but still didnt pick up can pick up this parcel
            {
                dal.PickUpParcel(oldDalParcel);

                IDAL.DO.Customer sender = dal.GetCustomer(oldDalParcel.SenderId);
                double distance = Tools.Utils.DistanceCalculation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, sender.Latitude, sender.Longitude);
                double batteryConsumption = getBatteryConsumption(oldDalParcel.Weight);
                double batteryWaste = distance * batteryConsumption;

                DroneToList newDrone = new DroneToList
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = drone.MaxWeight,
                    Battery = drone.Battery - batteryWaste,
                    DroneStatus = GetDroneToList(drone.Id).DroneStatus,
                    Location = new Location { Latitude = sender.Latitude, Longitude = sender.Longitude },
                    ParcelInDeliveryId = oldDalParcel.Id
                };

                DroneToList oldDrone = GetDroneToList(drone.Id);
                dronesToList.Remove(oldDrone);
                dronesToList.Add(newDrone);
            }
            else throw new ImpossibleOprationException("Parcel can't be picked up");
        }


        public void deliveryPackage(Drone drone)
        {
          
            DroneToList droneToList = GetDroneToList(drone.Id);
            if (droneToList.ParcelInDeliveryId != 0)
            {
                IDAL.DO.Parcel dalParcel = dal.GetParcel(droneToList.Id);
                if (dalParcel.PickedUp != null && dalParcel.Delivered == null)
                {
                    IDAL.DO.Customer target = dal.GetCustomer(dalParcel.TargetId);
                    double distance = Tools.Utils.DistanceCalculation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, target.Latitude, target.Longitude);
                    double batteryConsumption = getBatteryConsumption(dalParcel.Weight);
                    double battery = distance * batteryConsumption;

                    DroneToList newDrone = new DroneToList
                    {
                        Id = droneToList.Id,
                        Model = droneToList.Model,
                        MaxWeight = droneToList.MaxWeight,
                        Battery = droneToList.Battery - battery,
                        Location = new IBL.BO.Location { Latitude = target.Latitude, Longitude = target.Longitude },
                        DroneStatus = IBL.BO.DroneStatus.Available
                    };
                    dronesToList.Remove(droneToList);
                    dronesToList.Add(newDrone);

                    dal.ParcelDelivered(dalParcel);
                }
                else throw new ImpossibleOprationException("parcel can't be delivered");
            }
            else throw new ImpossibleOprationException("drone is not assigned to any parcel");
        }
        /// <summary>
        /// returns a copy of the drones list
        /// </summary>
        public IEnumerable<DroneToList> GetListOfDrones(Func<DroneToList, bool> predicate = null)
        {
            if (predicate == null)
                return dronesToList;
            return dronesToList.Where(predicate);
        }
        /// <summary>
        /// the function receives a BL drone and return BL DroneToList 
        /// </summary>
        /// <param name="dalDrone"> the drone to convert </param>
        /// <returns></returns>
        private IDAL.DO.Drone ConvertDroneToDal(Drone dalDrone)
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




//// find the longest distance the drone can fly without carrying a parcel and when carrying a parcel of a specific weight
//double possibleDistanceWithoutParcel = blDrone.Battery / whenAvailable;
//double possibleDistanceWithParcel = 0;

//// find what is the priority of the parcels in parcel
//int rate = 0;
//if (parcels.First().Weight == IDAL.DO.WeightCategories.Heavy)
//{
//    possibleDistanceWithParcel = blDrone.Battery / whenHeavy;
//    rate = (int)whenHeavy;
//}
//else if (parcels.First().Weight == IDAL.DO.WeightCategories.Medium)
//{
//    possibleDistanceWithParcel = blDrone.Battery / whenMedium;
//    rate = (int)whenMedium;
//}
//else if (parcels.First().Weight == IDAL.DO.WeightCategories.Light)
//{
//    possibleDistanceWithParcel = blDrone.Battery / whenLight;
//    rate = (int)whenLight;
//}

//double fromDroneToSender;
//double fromSenderToTarget;
//double fromTargetToStation;

//double minDistance = 0;

//// find the distances of the first parcel in the list
//IDAL.DO.Customer tmpSender = dal.GetCustomer(parcels.First().SenderId);
//IDAL.DO.Customer tmpTarget = dal.GetCustomer(parcels.First().TargetId);
//IDAL.DO.Station s = dal.getClosestStation(tmpTarget.Latitude, tmpTarget.Longitude);

//fromDroneToSender = Tools.Utils.DistanceCalculation(tmpSender.Latitude, tmpSender.Longitude, blDrone.Location.Latitude, blDrone.Location.Longitude);
//fromSenderToTarget = Tools.Utils.DistanceCalculation(tmpSender.Latitude, tmpSender.Longitude, tmpTarget.Latitude, tmpTarget.Longitude);
//fromTargetToStation = Tools.Utils.DistanceCalculation(tmpTarget.Latitude, tmpTarget.Longitude, s.Latitude, s.Longitude);
//minDistance = fromDroneToSender + fromSenderToTarget + fromTargetToStation;

//// find the parcel with the minimum distance the drone has to fly
//IDAL.DO.Parcel minDistanceParcel = new IDAL.DO.Parcel();
//bool parcelIsFound = false;
//foreach (var p in parcels)
//{
//    tmpSender = dal.GetCustomer(p.SenderId);
//    tmpTarget = dal.GetCustomer(p.TargetId);

//    fromDroneToSender = Tools.Utils.DistanceCalculation
//        (blDrone.Location.Latitude, blDrone.Location.Longitude, tmpSender.Latitude, tmpSender.Longitude);
//    fromSenderToTarget = Tools.Utils.DistanceCalculation
//        (tmpSender.Latitude, tmpSender.Longitude, tmpTarget.Latitude, tmpTarget.Longitude);
//    fromTargetToStation = Tools.Utils.DistanceCalculation
//        (tmpTarget.Latitude, tmpTarget.Longitude, s.Latitude, s.Longitude);

//    if ((fromDroneToSender * whenAvailable) + (fromSenderToTarget * rate) + (fromTargetToStation * whenAvailable) >= blDrone.Battery
//        && (fromDroneToSender + fromSenderToTarget + fromTargetToStation) <= minDistance)
//    {
//        minDistanceParcel = p;
//        minDistance = fromDroneToSender + fromSenderToTarget + fromTargetToStation;
//        parcelIsFound = true;
//    }
//}

//// make the necessary updates in the parcel and drone or throw exception if no parcel matched the conditions 
//if (!parcelIsFound)
//    throw new ImpossibleOperation("there is no parcel the drone can carry\n"); // איזה חריגה לזרוק

//dal.MatchDroneToParcel(minDistanceParcel, dal.GetDrone(id)); // make the update in dal

//blDrone.ParcelInDeliveryId = minDistanceParcel.Id;