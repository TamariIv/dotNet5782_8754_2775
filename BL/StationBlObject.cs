using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        /// <summary>
        /// get station of BL and add it into the stations list of DAL
        /// </summary>
        /// <param name="newStation"></param>
        public void AddStation(IBL.BO.Station newStation)
        {     
            try
            {
                DO.Station dalStation = new DO.Station
                {
                    Id = newStation.Id,
                    Name = newStation.Name,
                    AvailableChargeSlots = newStation.AvailableChargeSlots,
                    Latitude = newStation.Location.Latitude,
                    Longitude = newStation.Location.Longitude
                };
                dal.AddStation(dalStation);
            }
            catch(DO.IdAlreadyExistsException ex)
            {
                throw new IBL.BO.IdAlreadyExistsException(ex.Message);
            }

        }

        public void UpdateStation(int id, string name, int chargeSlots)
        {
            try
            {
                DO.Station dalStation = dal.GetStation(id);
                if (name == "" && chargeSlots.ToString() == "")
                    throw new IBL.BO.NoUpdateException("no update to station was received\n");
                if (name!= "")
                    dalStation.Name = name;
                if (chargeSlots.ToString() != "")
                {
                   int unavailableChargeSlots = dal.GetDroneCharges(d => d.StationId == id).Count();
                    dalStation.AvailableChargeSlots = chargeSlots - unavailableChargeSlots;
                }
                dal.UpdateStation(dalStation);
            }
            catch (DO.NoMatchingIdException ex)
            {
                throw new IBL.BO.NoMatchingIdException(ex.Message);
            }
            catch (DO.IdAlreadyExistsException ex)
            {
                throw new IBL.BO.IdAlreadyExistsException(ex.Message);
            }
        }

        public IBL.BO.Station GetStation(int id)
        {
            try
            {
                DO.Station dalStation = dal.GetStation(id);
                List<IBL.BO.DroneInCharging> dronesCharging = new List<IBL.BO.DroneInCharging>();
                foreach (var droneCharge in dal.GetDroneCharges())
                {
                    if (droneCharge.StationId == id)
                        dronesCharging.Add(new IBL.BO.DroneInCharging { Id = droneCharge.DroneId, Battery = GetDroneToList(droneCharge.DroneId).Battery });
                }
                IBL.BO.Station station = new IBL.BO.Station
                {
                    Id = dalStation.Id,
                    Name = dalStation.Name,
                    Location = new IBL.BO.Location { Latitude = dalStation.Latitude, Longitude = dalStation.Longitude },
                    AvailableChargeSlots = dalStation.AvailableChargeSlots,
                    DronesCharging = dronesCharging
                };
                return station;
            }
            catch (DO.NoMatchingIdException ex)
            {
                throw new IBL.BO.NoMatchingIdException(ex.Message);
            }
            catch (DO.IdAlreadyExistsException ex)
            {
                throw new IBL.BO.IdAlreadyExistsException(ex.Message);
            }
        }


        private IBL.BO.StationToList convertStationToStationToList(DO.Station dalStation)
        {
            // find how many drones are charging in the station using dal droneCharge type
            int OccupiedChargeSlots = 0;
            foreach (var droneCharge in dal.GetDroneCharges())
            {
                if (droneCharge.StationId == dalStation.Id)
                    // once you find another droneCharge with the station ID add ine to the number of occupied slots
                    OccupiedChargeSlots++;
            }
            IBL.BO.StationToList blStation = new IBL.BO.StationToList
            {
                Id = dalStation.Id,
                Name = dalStation.Name,
                AvailableChargeSlots = dalStation.AvailableChargeSlots,
                OccupiedChargeSlots = OccupiedChargeSlots
            };
            return blStation;
        }

        public IEnumerable<IBL.BO.StationToList> GetListOfStationsWithAvailableChargeSlots()
        {
            List<IBL.BO.StationToList> stationsWithAvailableChargeSlots = new List<IBL.BO.StationToList>();
            foreach (var station in dal.GetStations(s => s.AvailableChargeSlots > 0)) //send a predicate to DAL
            {
                stationsWithAvailableChargeSlots.Add(convertStationToStationToList(station));
            }
            return stationsWithAvailableChargeSlots;
        }

        public IEnumerable<IBL.BO.StationToList> GetListOfStations()
        {
            List<IBL.BO.StationToList> stations = new List<IBL.BO.StationToList>();
            foreach (var s in dal.GetStations())
            {
                stations.Add(convertStationToStationToList(s));
            }
            return stations;
        }
    }
}

