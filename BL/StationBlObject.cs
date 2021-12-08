using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                IDAL.DO.Station dalStation = new IDAL.DO.Station
                {
                    Id = newStation.Id,
                    Name = newStation.Name,
                    AvailableChargeSlots = newStation.AvailableChargeSlots,
                    Latitude=newStation.Location.Latitude,
                    Longitude=newStation.Location.Longitude
                };
                dal.AddStation(dalStation);       
        }

        public void UpdateStation(IBL.BO.Station newStation)
        {
            IDAL.DO.Station dalStation = dal.GetStation(newStation.Id);
            if (newStation.Name == "" && (newStation.AvailableChargeSlots).ToString() == "")
                throw new NoUpdateException("no update to station was received\n");
            if (newStation.Name != "")
                dalStation.Name = newStation.Name;
            if (newStation.AvailableChargeSlots.ToString() == "")
                dalStation.AvailableChargeSlots = newStation.AvailableChargeSlots + newStation.DronesCharging.Count();
            dal.UpdateStation(dalStation);
        }

        public IBL.BO.Station GetStation(int id)
        {
            IDAL.DO.Station dalStation = dal.GetStation(id);
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


        private IBL.BO.StationToList convertStationToStationToList(IDAL.DO.Station dalStation)
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

        public IEnumerable<IBL.BO.StationToList> GetListOfStations()
        {
            List<IBL.BO.StationToList> stations = new List<IBL.BO.StationToList>();
            foreach (var s in dal.GetStations())
            {
                stations.Add(convertStationToStationToList(s));
            }
            return stations;
        }

        public IEnumerable<IBL.BO.StationToList> GetListOfStationsWithAvailableChargeSlots()
        {
            List<IBL.BO.StationToList> stationsWithAvailableChargeSlots = new List<IBL.BO.StationToList>();
            foreach (var station in GetListOfStations())
            {
                if(station.AvailableChargeSlots != 0)
                stationsWithAvailableChargeSlots.Add(station);
            }
            return stationsWithAvailableChargeSlots;
        }

    }
}

