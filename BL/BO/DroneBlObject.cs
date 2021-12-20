using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BlObject : IBL
    {
        public void AddDrone(Drone newDrone, int stationId)
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

                DO.Drone dalDrone = new DO.Drone
                {
                    Id = newDrone.Id,
                    Model = newDrone.Model,
                    MaxWeight = (DO.WeightCategories)newDrone.MaxWeight
                };
                dal.AddDrone(dalDrone);
                DO.Station dalStation = dal.GetStation(stationId);
                dal.SendDroneToCharge(dalDrone, dalStation);
            }
            catch (DO.IdAlreadyExistsException)
            {
                throw new BO.IdAlreadyExistsException($"drone with id {newDrone.Id} already exists !!");
            }
            catch (DO.NoMatchingIdException ex)
            {
                throw new BO.NoMatchingIdException(ex.Message);
            }
        }

        /// <summary>
        /// the function receives a drone with a possible change in the model and updates the old one
        /// </summary>
        /// <param name="newDrone"> the updated drone </param>
        public void UpdateDrone(Drone newDrone)
        {
            try
            {
                DO.Drone dalDrone = dal.GetDrone(newDrone.Id);
                DroneToList blDrone = GetDroneToList(newDrone.Id);
                if (newDrone.Id.ToString() != "" && newDrone.Model != "")
                {
                    dalDrone.Model = newDrone.Model;
                    dal.UpdateDrone(dalDrone);
                    blDrone.Model = newDrone.Model;
                    UpdateBlDrone(blDrone);
                }
                else throw new NoUpdateException("no update was received\n");
            }
            catch (DO.NoMatchingIdException)
            {
                throw new NoMatchingIdException($"drone with id {newDrone.Id} doesn't exist !!");
            }
        }

        /// <summary>
        ///  the function receives an updated drone, deletes the old drone with the same id and adds the new one
        /// </summary>
        /// <param name="newDrone"> the updated drone </param>
        public void UpdateBlDrone(DroneToList newDrone)
        {
            dronesToList.Remove(GetDroneToList(newDrone.Id));
            dronesToList.Add(newDrone);
        }

        /// <summary>
        /// the function receives a drone and send it to charge in the closest station 
        /// </summary>
        /// <param name="drone"></param>
        public void rechargeDrone(int id)
        {
            Drone drone = GetDrone(id);
            try
            {
                if (drone.DroneStatus != DroneStatus.Available)
                    throw new ImpossibleOprationException("Drone can't be sent to recharge");

                DO.Station tempStation = dal.getClosestStation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude);
                double distance = Tools.Utils.DistanceCalculation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, tempStation.Latitude, tempStation.Longitude);
                double rate = whenAvailable;

                if (tempStation.AvailableChargeSlots > 0 && distance * rate < drone.Battery)
                {
                    DO.Drone dalDrone = ConvertDroneToDal(drone);
                    dal.SendDroneToCharge(dalDrone, tempStation);

                    DroneToList newDrone = new DroneToList
                    {
                        Id = drone.Id,
                        MaxWeight = drone.MaxWeight,
                        Model = drone.Model,
                        Battery = drone.Battery - distance * rate,
                        Location = new Location { Latitude = tempStation.Latitude, Longitude = tempStation.Longitude },
                        DroneStatus = DroneStatus.Maintenance,
                    };
                    DroneToList oldDrone = GetDroneToList(drone.Id);
                    dronesToList.Remove(oldDrone);
                    dronesToList.Add(newDrone);
                }
                else throw new ImpossibleOprationException("Drone can't be sent to recharge");
            }
            catch (DO.NoMatchingIdException ex)
            {
                throw new NoMatchingIdException(ex.Message);
            }

        }
        /// <summary>
        /// Release drone from charging and update the fields accordingly
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="timeInCharging"></param>
        public void FreeDrone(int droneId, double timeInCharging)
        {
            try
            {
                DroneToList blDrone = GetDroneToList(droneId);
                if (blDrone.DroneStatus == DroneStatus.Maintenance)
                {
                    dronesToList.Remove(blDrone);
                    blDrone.Battery += (int)(timeInCharging * chargeRate);
                    blDrone.DroneStatus = DroneStatus.Available;
                    dronesToList.Add(blDrone);

                    DO.Drone dalDrone = dal.GetDrone(droneId);
                    dal.SendDroneFromStation(dalDrone);

                }
                else throw new ImpossibleOprationException("drone can't be freed from chraging\n");
            }
            catch(DO.NoMatchingIdException)
            {
                throw new NoMatchingIdException($"drone with id {droneId} doesn't exist !!");
            }
        }

        /// <summary>
        /// the function receives an id and returns the bl droneToList with the same id
        /// </summary>
        /// <param name="id"> id of the drone to search </param>
        /// <returns> a bl droneToList with the id that was received </returns>
        public DroneToList GetDroneToList(int id)
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
        public Drone GetDrone(int id)
        {
            Drone d = ConvertDroneToListToDrone(GetDroneToList(id));
            return d; 
        }

        /// <summary>
        /// the function receives a bl droneToList and return the equal dal drone
        /// </summary>
        /// <param name="d"> the bl droneToList </param>
        /// <returns></returns>
        public Drone ConvertDroneToListToDrone(DroneToList d)
        {
            ParcelInDelivey parcelInDrone;
            if (d.ParcelInDeliveryId == 0)
            {
                parcelInDrone = new ParcelInDelivey();
            }
            else
            {
                try
                {
                    DO.Parcel parcel = dal.GetParcel(d.ParcelInDeliveryId);
                    parcelInDrone = new ParcelInDelivey
                    {
                        Id = parcel.Id,
                        PickUpStatus = getParcelStatus(parcel) == ParcelStatus.PickedUp || getParcelStatus(parcel) == ParcelStatus.Delivered ? true : false,
                        Weight = (WeightCategories)parcel.Weight,
                        Priority = (Priorities)parcel.Priority,
                        Sender = new CustomerInParcel { Id = dal.GetCustomer(parcel.SenderId).Id, Name = dal.GetCustomer(parcel.SenderId).Name },
                        Target = new CustomerInParcel { Id = dal.GetCustomer(parcel.TargetId).Id, Name = dal.GetCustomer(parcel.TargetId).Name },
                        PickUpLocation = new Location { Latitude = dal.GetCustomer(parcel.SenderId).Latitude, Longitude = dal.GetCustomer(parcel.SenderId).Longitude },
                        TargetLocation = new Location { Latitude = dal.GetCustomer(parcel.TargetId).Latitude, Longitude = dal.GetCustomer(parcel.TargetId).Longitude },
                        Distance = Tools.Utils.DistanceCalculation(dal.GetCustomer(parcel.SenderId).Latitude, dal.GetCustomer(parcel.SenderId).Longitude, dal.GetCustomer(parcel.TargetId).Latitude, dal.GetCustomer(parcel.TargetId).Longitude)
                    };
                }
                catch(DO.NoMatchingIdException ex)
                {
                    throw new NoMatchingIdException(ex.Message);
                }
            }

            Drone newDrone = new Drone
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
        /// the function receives a drone id and finds a parcel that can be assigned to it
        /// </summary>
        /// <param name="id"> the id of the drone </param>
        public void DroneToParcel(int id)
        {
            try
            {
                DroneToList blDrone = GetDroneToList(id);

                if (blDrone.DroneStatus == DroneStatus.Available)
                {

                // make list of parcels with highest priority possible
                List<DO.Parcel> parcels = dal.GetParcels().Where(parcel => parcel.Priority == DO.Priorities.Emergency).ToList();
                if (!parcels.Any())
                    parcels = dal.GetParcels().Where(parcel => parcel.Priority == DO.Priorities.Rapid).ToList();
                if (!parcels.Any())
                    parcels = dal.GetParcels().Where(parcel => parcel.Priority == DO.Priorities.Regular).ToList();
                if (!parcels.Any())
                    throw new EmptyListException("no parcel was found\n");

                // delete from the list parcels that are too heavy for the drone
                parcels = parcels.Where(parcel => (int)parcel.Weight >= (int)blDrone.MaxWeight).ToList();

                    // find a parcel in that is [ossible for the drone to take
                    DO.Parcel posibleDistanceParcel = parcels.First();
                    Parcel finalParcel = new BO.Parcel();
                    bool parcelWasFound = false;
                    foreach (var p in parcels)
                    {
                        DO.Customer tempSender = dal.GetCustomer(p.SenderId);
                        DO.Customer tempTarget = dal.GetCustomer(p.TargetId);
                        DO.Station closestStation = dal.getClosestStation(tempTarget.Latitude, tempTarget.Longitude);
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
                        throw new ImpossibleOprationException("there is no parcel the drone can carry\n");

                    dal.MatchDroneToParcel(dal.GetParcel(finalParcel.Id), dal.GetDrone(id)); // make the update in dal

                    blDrone.ParcelInDeliveryId = finalParcel.Id;
                    blDrone.DroneStatus = DroneStatus.Delivery;
                    UpdateBlDrone(blDrone);


                }
                else throw new ImpossibleOprationException("the drone is not available\n");
            }
            catch (DO.NoMatchingIdException ex)
            {
                throw new NoMatchingIdException(ex.Message);
            }
            catch (DO.IdAlreadyExistsException ex)
            {
                throw new IdAlreadyExistsException(ex.Message);
            }
        }
        /// <summary>
        /// pick up parcel by drone - update the drone and the parcel in accordance
        /// </summary>
        /// <param name="drone"></param>
        public void PickUpParcel(Drone drone)
        {
            try
            {
                List<DO.Parcel> parcels = dal.GetParcels().ToList();
                DO.Parcel oldDalParcel;
                int parcelIndex = parcels.FindIndex(p => p.DroneId == drone.Id);
                if (parcelIndex != -1) //this parcel is assigned
                    oldDalParcel = parcels[parcelIndex];
                else throw new NoMatchingIdException($"no parcel can be picked up by drone with id {drone.Id}");

                if (oldDalParcel.PickedUp == null) //only drone that assigned to parcel but still didnt pick up can pick up this parcel
                {
                    dal.PickUpParcel(oldDalParcel);

                    DO.Customer sender = dal.GetCustomer(oldDalParcel.SenderId);
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
            catch(DO.NoMatchingIdException ex)
            {
                throw new NoMatchingIdException(ex.Message);
            }
            catch (DO.IdAlreadyExistsException ex)
            {
                throw new IdAlreadyExistsException(ex.Message);
            }

        }

        public void deliveryPackage(Drone drone)
        {
            try
            {
                DroneToList droneToList = GetDroneToList(drone.Id);
                if (droneToList.ParcelInDeliveryId != 0)
                {
                    DO.Parcel dalParcel = dal.GetParcel(droneToList.ParcelInDeliveryId);
                    if (dalParcel.PickedUp != null && dalParcel.Delivered == null)
                    {
                        DO.Customer target = dal.GetCustomer(dalParcel.TargetId);
                        double distance = Tools.Utils.DistanceCalculation(droneToList.Location.Latitude, droneToList.Location.Longitude, target.Latitude, target.Longitude);
                        double batteryConsumption = getBatteryConsumption(dalParcel.Weight);
                        double battery = distance * batteryConsumption;

                        DroneToList newDrone = new DroneToList
                        {
                            Id = droneToList.Id,
                            Model = droneToList.Model,
                            MaxWeight = droneToList.MaxWeight,
                            Battery = droneToList.Battery - battery,
                            Location = new BO.Location { Latitude = target.Latitude, Longitude = target.Longitude },
                            DroneStatus = BO.DroneStatus.Available
                        };
                        dronesToList.Remove(droneToList);
                        dronesToList.Add(newDrone);

                        dal.ParcelDelivered(dalParcel);
                    }
                    else throw new ImpossibleOprationException("parcel can't be delivered");
                }
                else throw new ImpossibleOprationException("drone is not assigned to any parcel");
            }
            catch (DO.NoMatchingIdException ex)
            {
                throw new NoMatchingIdException(ex.Message);
            }
            catch (DO.IdAlreadyExistsException ex)
            {
                throw new IdAlreadyExistsException(ex.Message);
            }
        }
        /// <summary>
        /// returns a copy of the drones list
        /// </summary>
        public IEnumerable<DroneToList> GetListOfDrones()                  
        {
            IEnumerable<DroneToList> droneToList1 = dronesToList;
                return droneToList1;
        }
        /// <summary>
        /// the function receives a BL drone and return BL DroneToList 
        /// </summary>
        /// <param name="dalDrone"> the drone to convert </param>
        /// <returns></returns>
        private DO.Drone ConvertDroneToDal(Drone dalDrone)
        {
            DO.Drone newDrone = new DO.Drone()
            {
                Id = dalDrone.Id,
                Model = dalDrone.Model,
                MaxWeight = (DO.WeightCategories)dalDrone.MaxWeight
            };
            return newDrone;
        }
    }
}
