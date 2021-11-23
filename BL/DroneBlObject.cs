using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BlObject:IBL.IBL
    {
        public void AddDrone(IBL.BO.Drone newDrone, int stationId)
        {
            newDrone.Battery = r.Next(20, 41);
            newDrone.DroneStatus = IBL.BO.Enums.DroneStatus.Maintenance;
            IDAL.DO.Station s;
            if ((dal.GetStations()).ToList().Exists(station => station.Id == stationId))
            {
               s = (dal.GetStations()).ToList().Find(station => station.Id == stationId);
            }
            else throw new NoMatchingIdException($"station with id {stationId} doesn't exist !!");
            IBL.BO.Location l = new IBL.BO.Location
            {
                Longitude = s.Longitude,
                Latitude = s.Latitude
            };
            newDrone.CurrentLocation = l;
            
            IDAL.DO.Drone dalDrone = new IDAL.DO.Drone
            {
                Id = newDrone.Id,
                Model = newDrone.Model,
                MaxWeight = (IDAL.DO.WeightCategories)newDrone.MaxWeight
            };
            dal.SendDroneToCharge(dalDrone, s);
            dal.AddDrone(dalDrone);
        }



        public void UpdateDrone(IBL.BO.Drone newDrone)
        {
            IDAL.DO.Drone dalDrone = dal.GetDrone(newDrone.Id);
            if (newDrone.Id != 0)
                dalDrone.Id = newDrone.Id;
            if (newDrone.Model != "")
                dalDrone.Model = newDrone.Model;
            dal.UpdateDrone(dalDrone);
        }


        

        public void FreeDrone(int droneId, int timeInCharging)
        {
            IBL.BO.DroneToList blDrone;
            blDrone = dronesToList.Find(d => d.Id == droneId);
            if (blDrone.Id == droneId)
            {
                if (blDrone.DroneStatus == IBL.BO.Enums.DroneStatus.Maintenance)
                {
                    blDrone.Battery += (int)(timeInCharging * chargeRate);
                    blDrone.DroneStatus = IBL.BO.Enums.DroneStatus.Available;

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

        public IBL.BO.DroneToList GetDroneToList(int idNumber)
        {
            IBL.BO.DroneToList d = new IBL.BO.DroneToList();
            if (dronesToList. Exists(drone => drone.Id == idNumber))
            {
                d = dronesToList.Find(drone => drone.Id == idNumber);
                return d;
            }
            else throw new NoMatchingIdException($"drone with id {idNumber} doesn't exist !!");
        }

        public void DroneToParcel(int id)
        {
            IBL.BO.DroneToList blDrone = GetDroneToList(id);
            if (blDrone.Id == id)
            {
                if (blDrone.DroneStatus == IBL.BO.Enums.DroneStatus.Maintenance)
                {
                    IEnumerable<IDAL.DO.Parcel> parcels = dal.GetParcels().Where(parcel => parcel.Priority == IDAL.DO.Priorities.Emergency);

                    // make list of parcels with highest priority possible
                    if (!parcels.Any())
                        parcels = dal.GetParcels().Where(parcel => parcel.Priority == IDAL.DO.Priorities.Rapid);
                    if (!parcels.Any())
                        parcels = dal.GetParcels().Where(parcel => parcel.Priority == IDAL.DO.Priorities.Regular);
                    if (!parcels.Any())
                        throw new EmptyListException("no parcel was found\n");

                    // delete from the list parcels that are too heavy for the drone
                    parcels = parcels.Where(parcel => (int)parcel.Weight >= (int)blDrone.MaxWeight);


                    // find the closest parcel
                    double minDistance = 0;
                    double possibleDstance = blDrone.Battery / dal.GetElectricity()[(int)blDrone.MaxWeight];
                    IBL.BO.Location location;
                    foreach (var p in parcels)
                    {
                        location = new IBL.BO.Location
                        {
                            Longitude = dal.GetCustomer(p.SenderId).Longitude,
                            Latitude = dal.GetCustomer(p.SenderId).Latitude
                        };

                    }

                }

            }
                

        }

    }
}
