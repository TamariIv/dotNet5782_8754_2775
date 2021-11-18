using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace DalObject
{
    public class DalObject : IDal
    {
        //DalObject dalObject = new DalObject();

        public DalObject() //constructor
        {
            DataSource.Initialize();
        }

        /// <summary>
        ///  search station by idNumber
        /// </summary>
        /// <param name="idNumber"> the station id </param>
        /// <returns> the station that was found </returns>
        public Station GetStation(int idNumber)
        {
            Station s = new Station();
            for (int i = 0; i < DataSource.Stations.Count(); i++)
            {
                if (DataSource.Stations[i].Id == idNumber)
                    return DataSource.Stations[i];
            }
            return s;
        }

        /// <summary>
        /// search drone by idNumber 
        /// </summary>
        /// <param name="idNumber"> the drone id </param>
        /// <returns> the drone that was found </returns>
        public Drone GetDrone(int idNumber)
        {
            Drone d = new Drone();
            if (DataSource.Drones.Exists(drone => drone.Id == idNumber))
            {
                d = DataSource.Drones.Find(drone => drone.Id == idNumber);
                return d;
            }
            else throw new DroneException($"id {idNumber} doesn't exist !!");
        }

        /// <summary>
        /// search customer by idNumber 
        /// </summary>
        /// <param name="idNumber"> the customer id </param>
        /// <returns> the customer that was found </returns>
        public Customer GetCustomer(int idNumber) //search the customer by idNumber and return it
        {
            Customer c = new Customer();
            for (int i = 0; i < DataSource.Customers.Count(); i++)
            {
                if (DataSource.Customers[i].Id == idNumber)
                    return DataSource.Customers[i];
            }
            return c;
        }

        /// <summary>
        /// search parcel by id number
        /// </summary>
        /// <param name="idNumber"> the parcel id </param>
        /// <returns> the parcel that was found </returns>
        public Parcel GetParcel(int idNumber) //search the parcel by idNumber and return it
        {
            Parcel p = new Parcel();
            for (int i = 0; i < DataSource.Parcels.Count(); i++)
            {
                if (DataSource.Parcels[i].Id == idNumber)
                    return DataSource.Parcels[i];
            }
            return p;
        }

        /// <summary>
        /// search droneCharge element by drone id
        /// </summary>
        /// <param name="idNumber"> the id of a drone that is charging </param>
        /// <returns> the droneCharge element with the drone id </returns>
        public DroneCharge GetDroneCharge(int idNumber)
        {
            DroneCharge d = new DroneCharge();
            for (int i = 0; i < DataSource.DroneCharges.Count(); i++)
            {
                if (DataSource.DroneCharges[i].DroneId == idNumber)
                    return DataSource.DroneCharges[i];
            }
            return d;
        }

        /// <summary>
        /// get the list of stations
        /// </summary>
        /// <returns> a copy of the list stations </returns>
        public IEnumerable<Station> GetStations()
        {
            List<Station> copyStations = new List<Station>();
            for (int i = 0; i < DataSource.Stations.Count(); i++)
            {
                Station s = DataSource.Stations[i];
                copyStations.Add(s);
            }
            return copyStations;
        }

        /// <summary>
        /// get the list of drones
        /// </summary>
        /// <returns> a copy of the list drones </returns>
        public IEnumerable<Drone> GetDrones()
        {
            List<Drone> copyDrones = new List<Drone>();
            for (int i = 0; i < DataSource.Drones.Count(); i++)
            {
                copyDrones.Add(DataSource.Drones[i]);
            }
            return copyDrones;
        }

        /// <summary>
        /// get the list of customers
        /// </summary>
        /// <returns> a copy of the list customers <</returns>
        public IEnumerable<Customer> GetCustomers()
        {
            List<Customer> copyCustomers = new List<Customer>();
            for (int i = 0; i < DataSource.Customers.Count(); i++)
            {
                copyCustomers.Add(DataSource.Customers[i]);
            }
            return copyCustomers;
        }

        /// <summary>
        /// get the list of parcels
        /// </summary>
        /// <returns> a copy of the list parcels <</returns>
        public IEnumerable<Parcel> GetParcels()
        {
            List<Parcel> copyParcels = new List<Parcel>();
            for (int i = 0; i < DataSource.Parcels.Count(); i++)
            {
                copyParcels.Add(DataSource.Parcels[i]);
            }
            return copyParcels;
        }

        /// <summary>
        /// make a list if the parcels without drones in Parcels 
        /// </summary>
        /// <returns> list that contains the parcels without drone </returns>
        public IEnumerable<Parcel> GetParcelWithoutDrone()
        {
            List<Parcel> ParcelsWithoutDrone = new List<Parcel>();
            for (int i = 0; i < DataSource.Parcels.Count(); i++)
            {
                if (DataSource.Parcels[i].DroneId == 0)
                    ParcelsWithoutDrone.Add(DataSource.Parcels[i]);
            }
            return ParcelsWithoutDrone;
        }
        /// <summary>
        /// make a list of the stations that have available chraging slots
        /// </summary>
        /// <returns> list of stations with available chraging slots </returns>
        public IEnumerable<Station> AvailableCharger()
        {
            List<Station> AvailableChargers = new List<Station>();
            for (int i = 0; i < DataSource.Stations.Count(); i++)
            {
                if (DataSource.Stations[i].ChargeSlots != 0)
                    AvailableChargers.Add(DataSource.Stations[i]);
            }
            return AvailableChargers;
        }
        /// <summary>
        /// receive drone and add it to Drones
        /// </summary>
        /// <param name="drone"> the drone to add </param>
        public void AddDrone(Drone d)
        {
            if (DataSource.Drones.Exists(drone => drone.Id == d.Id))
            {
                throw new DroneException($"id {d.Id} already exists !!");
            }
            DataSource.Drones.Add(d);
        }
        /// <summary>
        /// receive customer and add it to Customers 
        /// </summary>
        /// <param name="c"> the customer to add </param>
        public void AddCustomer(Customer c)
        {
            DataSource.Customers.Add(c);
        }
        /// <summary>
        ///  receive station and add it to Stations
        /// </summary>
        /// <param name="station"> the station to add </param>
        public void AddStation(Station station)
        {
            DataSource.Stations.Add(station);
        }
        /// <summary>
        ///  receive parcel and add it to Parcels
        /// </summary>
        /// <param name="parcel"> the parcel to add </param>
        /// <returns> the id of the next new parcel </returns> 
        public int AddParcel(Parcel parcel)
        {
            parcel.Id = DataSource.Config.ParcelId;

            DataSource.Parcels.Add(parcel);
            return ++DataSource.Config.ParcelId;
        }
        /// <summary>
        /// the function receives a drone and a parcel and assigns the drone to the parcel
        /// </summary>
        public void MatchDroneToParcel(Parcel p, Drone d)
        {
            Parcel newParcel = p;
            Drone newDrone = d;
            newParcel.DroneId = d.Id;
            newParcel.Scheduled = DateTime.Now;
            //newDrone.Status = DroneStatus.Assigned;

            DataSource.Drones.Remove(d);
            DataSource.Drones.Add(newDrone);
            DataSource.Parcels.Remove(p);
            DataSource.Parcels.Add(newParcel);
        }
        /// <summary>
        /// update the pick up time of a parcel
        /// </summary>
        /// <param name="p"> the parcel to update </param>
        public void PickUpParcel(Parcel p)
        {
            Parcel newParcel = p;
            newParcel.PickedUp = DateTime.Now;
            DataSource.Parcels.Remove(p);
            DataSource.Parcels.Add(newParcel);
        }
        /// <summary>
        /// update the delivery time of a parcel
        /// </summary>
        /// <param name="p"> the parcel to update </param>
        public void ParcelDelivered(Parcel p)
        {
            // receive drone and change drone stat to assigned
            Drone newDrone = GetDrone(p.DroneId);
            // newDrone.Status = DroneStatus.Delivery;
            Parcel newParcel = p;
            newParcel.Delivered = DateTime.Now;

            DataSource.Drones.Remove(GetDrone(p.DroneId));
            DataSource.Drones.Add(newDrone);
            DataSource.Parcels.Remove(p);
            DataSource.Parcels.Add(newParcel);
        }
        /// <summary>
        /// the function receives a drone and a station and send the drone to charge in that station 
        /// (also update the number of available chraging slots in the station)
        /// </summary>
        public void SendDroneToCharge(Drone d, Station s)
        {
            Drone newDrone = d;
            Station newStation = s;
            //newDrone.Status = DroneStatus.Maintenance;
            newStation.ChargeSlots--;
            DroneCharge dc = new DroneCharge();
            dc.DroneId = newDrone.Id;
            dc.StationId = newStation.Id;
            DataSource.DroneCharges.Add(dc);

            DataSource.Drones.Remove(d);
            DataSource.Drones.Add(newDrone);
            DataSource.Stations.Remove(s);
            DataSource.Stations.Add(newStation);
        }
        /// <summary>
        /// the function sends drone from charging 
        /// </summary>
        public void SendDroneFromStation(Drone d)
        {
            DroneCharge dronecharge = GetDroneCharge(d.Id);
            Station s = GetStation(dronecharge.StationId);
            Station newStation = s;
            newStation.ChargeSlots++;
            Drone newDrone = d;
            // newDrone.Status = DroneStatus.Available;
            // newDrone.Battery = 100;
            DataSource.DroneCharges.Remove(dronecharge);
            DataSource.Drones.Remove(d);
            DataSource.Drones.Add(newDrone);
            DataSource.Stations.Remove(s);
            DataSource.Stations.Add(newStation);
        }

        IEnumerable<Customer> IDal.GetCustomers()
        {
            throw new NotImplementedException();
        }
        public double GetChargeRate()
        {
            return DataSource.Config.ChargingRate;
        }

        public double[] GetElectricity()
        {
            double[] electricityRates =
            {
                DataSource.Config.Available,
                DataSource.Config.LightWeight,
                DataSource.Config.MediumWeight,
                DataSource.Config.HeavyWeight,
                DataSource.Config.ChargingRate
            };
            return electricityRates;
        }
    }
}