using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace DalObject
{
    public partial class DalObject : IDal
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
            if (DataSource.Drones.Exists(station => station.Id == idNumber))
            {
                s = DataSource.Stations.Find(station => station.Id == idNumber);
                return s;
            }
            else throw new NoMatchingIdException($"station with id {idNumber} doesn't exist !!");
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
            else throw new NoMatchingIdException($"drone with id {idNumber} doesn't exist !!");
        }

        /// <summary>
        /// search customer by idNumber 
        /// </summary>
        /// <param name="idNumber"> the customer id </param>
        /// <returns> the customer that was found </returns>
        public Customer GetCustomer(int idNumber) //search the customer by idNumber and return it
        {
            Customer c = new Customer();
            if (DataSource.Customers.Exists(customer => customer.Id == idNumber))
            {
                c = DataSource.Customers.Find(customer => customer.Id == idNumber);
                return c;
            }
            else throw new NoMatchingIdException($"customer with id {idNumber} doesn't exist !!");
        }

        /// <summary>
        /// search parcel by id number
        /// </summary>
        /// <param name="idNumber"> the parcel id </param>
        /// <returns> the parcel that was found </returns>
        public Parcel GetParcel(int idNumber) //search the parcel by idNumber and return it
        {
            Parcel p = new Parcel();
            if (DataSource.Parcels.Exists(parcel => parcel.Id == idNumber))
            {
                p = DataSource.Parcels.Find(parcel => parcel.Id == idNumber);
                return p;
            }
            else throw new NoMatchingIdException($"parcel with id {idNumber} doesn't exist !!");
        }


        /// <summary>
        /// search droneCharge element by drone id
        /// </summary>
        /// <param name="idNumber"> the id of a drone that is charging </param>
        /// <returns> the droneCharge element with the drone id </returns>
        public DroneCharge GetDroneCharge(int idNumber)
        {
            DroneCharge d = new DroneCharge();
            if (DataSource.DroneCharges.Exists(drone => drone.DroneId == idNumber))
            {
                d = DataSource.DroneCharges.Find(drone => drone.DroneId == idNumber);
                return d;
            }
            else throw new NoMatchingIdException($"drone being charged with id {idNumber} doesn't exist !!");
        }

        /// <summary>
        /// get the list of stations
        /// </summary>
        /// <returns> a copy of the list stations </returns>
        public IEnumerable<Station> GetStations()
        {
            List<Station> copyStations = new List<Station>(DataSource.Stations);
            //for (int i = 0; i < DataSource.Stations.Count(); i++)
            //{
            //    Station s = DataSource.Stations[i];
            //    copyStations.Add(s);
            //}
            return copyStations;
        }

        /// <summary>
        /// get the list of drones
        /// </summary>
        /// <returns> a copy of the list drones </returns>
        public IEnumerable<Drone> GetDrones()
        {
            List<Drone> copyDrones = new List<Drone>(DataSource.Drones);
            //for (int i = 0; i < DataSource.Drones.Count(); i++)
            //{
            //    copyDrones.Add(DataSource.Drones[i]);
            //}
            return copyDrones;
        }

        /// <summary>
        /// get the list of customers
        /// </summary>
        /// <returns> a copy of the list customers <</returns>
        public IEnumerable<Customer> GetCustomers()
        {
            List<Customer> copyCustomers = new List<Customer>(DataSource.Customers);
            //for (int i = 0; i < DataSource.Customers.Count(); i++)
            //{
            //    copyCustomers.Add(DataSource.Customers[i]);
            //}
            return copyCustomers;
        }

        /// <summary>
        /// get the list of parcels
        /// </summary>
        /// <returns> a copy of the list parcels <</returns>
        public IEnumerable<Parcel> GetParcels()
        {
            List<Parcel> copyParcels = new List<Parcel>(DataSource.Parcels);
            //for (int i = 0; i < DataSource.Parcels.Count(); i++)
            //{
            //    copyParcels.Add(DataSource.Parcels[i]);
            //}
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
                throw new IdAlreadyExistsException($"drone with id {d.Id} already exists !!");
            }
            DataSource.Drones.Add(d);
        }

        /// <summary>
        /// receive customer and add it to Customers 
        /// </summary>
        /// <param name="c"> the customer to add </param>
        public void AddCustomer(Customer c)
        {
            if (DataSource.Customers.Exists(customer => customer.Id == c.Id))
            {
                throw new IdAlreadyExistsException($"customer with id {c.Id} already exists !!");
            }
            DataSource.Customers.Add(c);
        }

        /// <summary>
        ///  receive station and add it to Stations
        /// </summary>
        /// <param name="station"> the station to add </param>
        public void AddStation(Station s)
        {
            if (DataSource.Stations.Exists(station => station.Id == s.Id))
            {
                throw new IdAlreadyExistsException($"station with id {s.Id} already exists !!");
            }
            DataSource.Stations.Add(s);
        }

        /// <summary>
        ///  receive parcel and add it to Parcels
        /// </summary>
        /// <param name="parcel"> the parcel to add </param>
        /// <returns> the id of the next new parcel </returns> 
        public int AddParcel(Parcel p)
        {
            if (DataSource.Customers.Exists(parcel => parcel.Id == p.Id))
            {
                throw new IdAlreadyExistsException($"parcel with id {p.Id} already exists !!");
            }
            p.Id = DataSource.Config.ParcelId;
            DataSource.Parcels.Add(p);
            return ++DataSource.Config.ParcelId;
        }

        /// <summary>
        /// the function receives a drone and a parcel and assigns the drone to the parcel
        /// </summary>
        public void MatchDroneToParcel(Parcel p, Drone d)
        {
            if (DataSource.Parcels.Exists(parcel => p.Id == parcel.Id))
            {
                if (DataSource.Drones.Exists(drone => d.Id == drone.Id))
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
                else throw new NoMatchingIdException($"drone with id {d.Id} doesn't exists !!");
            }
            else throw new NoMatchingIdException($"parcel with id {d.Id} doesn't exists !!");
        }

        /// <summary>
        /// update the pick up time of a parcel
        /// </summary>
        /// <param name="p"> the parcel to update </param>
        public void PickUpParcel(Parcel p)
        {
            if (DataSource.Parcels.Exists(parcel => p.Id == parcel.Id))
            {
                Parcel newParcel = p;
                newParcel.PickedUp = DateTime.Now;
                DataSource.Parcels.Remove(p);
                DataSource.Parcels.Add(newParcel);
            }
            else throw new NoMatchingIdException($"parcel with id {p.Id} doesn't exists !!");
        }

        /// <summary>
        /// update the delivery time of a parcel
        /// </summary>
        /// <param name="p"> the parcel to update </param>
        public void ParcelDelivered(Parcel p)
        {
            if (DataSource.Parcels.Exists(parcel => p.Id == parcel.Id))
            {
                Drone newDrone = GetDrone(p.DroneId);
                Parcel newParcel = p;
                newParcel.Delivered = DateTime.Now;

                DataSource.Drones.Remove(GetDrone(p.DroneId));
                DataSource.Drones.Add(newDrone);
                DataSource.Parcels.Remove(p);
                DataSource.Parcels.Add(newParcel);
            }
            else throw new NoMatchingIdException($"parcel with id {p.Id} doesn't exists !!");
        }

        /// <summary>
        /// the function receives a drone and a station and send the drone to charge in that station 
        /// (also update the number of available chraging slots in the station)
        /// </summary>
        public void SendDroneToCharge(Drone d, Station s)
        {
            if (DataSource.Drones.Exists(drone => drone.Id == d.Id))
            {
                if (DataSource.Stations.Exists(station => station.Id == s.Id))
                {
                    Drone newDrone = d;
                    Station newStation = s;
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
                else throw new NoMatchingIdException($"station with id {s.Id} doesn't exists !!");
            }
            else throw new NoMatchingIdException($"drone with id {d.Id} doesn't exists !!");
        }

        /// <summary>
        /// the function sends drone from charging 
        /// </summary>
        public void SendDroneFromStation(Drone d)
        {
            if (DataSource.Drones.Exists(drone => drone.Id == d.Id))
            {
                DroneCharge dronecharge = GetDroneCharge(d.Id);
                Station s = GetStation(dronecharge.StationId);
                Station newStation = s;
                newStation.ChargeSlots++;
                Drone newDrone = d;
                DataSource.DroneCharges.Remove(dronecharge);
                DataSource.Drones.Remove(d);
                DataSource.Drones.Add(newDrone);
                DataSource.Stations.Remove(s);
                DataSource.Stations.Add(newStation);
            }
            else throw new NoMatchingIdException($"drone with id {d.Id} doesn't exists !!");
        }


        public void UpdateCustomer(Customer c)
        {
            Customer newCustomer =  GetCustomer(c.Id);
            DataSource.Customers.Remove(newCustomer);
            DataSource.Customers.Add(c);
        }

        public void UpdateDrone(Drone d)
        {
            Drone newDrone = GetDrone(d.Id);
            DataSource.Drones.Remove(d);
            
        }

        public double[] GetElectricity()
        {
            double[] electricityRates =
            {
                DataSource.Config.WhenAvailable,
                DataSource.Config.WhenLightWeight,
                DataSource.Config.WhenMediumWeight,
                DataSource.Config.WhenHeavyWeight,
                DataSource.Config.ChargingRate
            };
            return electricityRates;
        }
    }
}




