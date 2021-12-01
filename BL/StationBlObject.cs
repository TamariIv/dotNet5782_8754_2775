using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BlObject:IBL.IBL
    {
        public void AddStation(IBL.BO.Station newStation)
        {
            if (newStation.DronesCharging.Count == 0)
            {
                IDAL.DO.Station dalStation = new IDAL.DO.Station
                {
                    Id = newStation.Id,
                    Name = newStation.Name,
                    AvailableChargeSlots = newStation.AvailableChargeSlots,

                };
                dal.AddStation(dalStation);
            }
        }

        public void UpdateStation(IBL.BO.Station newStation)
        {
            IDAL.DO.Station dalStation =  dal.GetStation(newStation.Id);
            if (newStation.Name == "" && (newStation.AvailableChargeSlots).ToString() == "")
                throw new NoUpdateException("no update to station was received\n");
            if (newStation.Name != "")
                dalStation.Name = newStation.Name;
            if ((newStation.AvailableChargeSlots).ToString() == "")
                dalStation.AvailableChargeSlots = newStation.AvailableChargeSlots + newStation.DronesCharging.Count();

            dal.UpdateStation(dalStation);
        }
        public IBL.BO.Station GetStation(int id)
        {
            IDAL.DO.Station dalStation = dal.GetStation(id);
            IBL.BO.Station station = new IBL.BO.Station
            {
                Id = dalStation.Id,
                Name = dalStation.Name,
                Location = new IBL.BO.Location { Latitude = dalStation.Latitude, Longitude = dalStation.Longitude },
                AvailableChargeSlots = dalStation.AvailableChargeSlots,
                DronesCharging = dal.GetDroneCharges();
                /*
                  public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int AvailableChargeSlots { get; set; }
        public List<DroneInCharging> DronesCharging { get; set; }
                */
            }
        }
        //public IBL.BO.StationToList GetStationToList(int id)
        //{
        //    IBL.BO.StationToList s = new IBL.BO.StationToList();
        //    List<IDAL.DO.Station> stations = dal.GetStations().ToList();
        //    foreach (var dalStation in stations)
        //    {
        //        if (dalStation.Id == id)
        //        {
        //            s = ConvertStationToBl(dalStation);
        //            return s;
        //        }
        //    }
        //    throw new NoMatchingIdException($"station with id {id} doesn't exist");
        //}

        public IBL.BO.StationToList ConvertStationToBl(IDAL.DO.Station dalStation)
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

        public void PrintStationToList(int id)
        {
            Console.WriteLine(getParcelToList(id));
        }

        public List<IBL.BO.StationToList> GetListOfStations()
        {
            List<IBL.BO.StationToList> stations = new List<IBL.BO.StationToList>();
            foreach (var s in dal.GetStations())
            {
                stations.Add(ConvertStationToBl(s));
            }
            return stations;
        }

        public void PrintListOfStations()
        {
            foreach (var s in GetListOfStations())
            {
                Console.WriteLine(s + "\n");
            }
        }
    }
}
