using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace DalObject
{
    public class DalObject
    {
        //DalObject dalObject = new DalObject();

        public DalObject() //constructor
        {
            DataSource.Initialize();
        }
        /// <summary>
        ///  search the drone by idNumber
        /// </summary>
        /// <param name="idNumber">the station id</param>
        /// <returns></returns>
        public Station GetStation(int idNumber)  //search the station by idNumber and return it
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
        /// search the drone by idNumber 
        /// </summary>
        /// <returns>the drone idNumber</returns>
        public Drone GetDrone(int idNumber) //search the drone by idNumber and return it
        {
            Drone d = new Drone();
            for (int i = 0; i < DataSource.Drones.Count(); i++)
            {
                if (DataSource.Drones[i].Id == idNumber)
                    return DataSource.Drones[i];
            }
            return d;
        }

        /// <summary>
        /// search the customer by idNumber 
        /// </summary>
        /// <returns>the customer by idNumber </returns>
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

        public List<Station> GetStations()
        {
            List<Station> copyStations = new List<Station>();
            for (int i = 0; i < DataSource.Stations.Count(); i++)
            {
                Station s = DataSource.Stations[i];
                copyStations.Add(s);
            }
            return copyStations;
        }

        public List<Drone> GetDrones()
        {
            List<Drone> copyDrones = new List<Drone>();
            for (int i = 0; i < DataSource.Drones.Count(); i++)
            {
                copyDrones.Add(DataSource.Drones[i]);
            }
            return copyDrones;
        }

        public List<Customer> GetCustomers()
        {
            List<Customer> copyCustomers = new List<Customer>();
            for (int i = 0; i < DataSource.Customers.Count(); i++)
            {
                copyCustomers.Add(DataSource.Customers[i]);
            }
            return copyCustomers;
        }

        public List<Parcel> GetParcels()
        {
            List<Parcel> copyParcels = new List<Parcel>();
            for (int i = 0; i < DataSource.Parcels.Count(); i++)
            {
                copyParcels.Add(DataSource.Parcels[i]);
            }
            return copyParcels;
        }

        /// <summary>
        /// search the parcels without drones in the Parcels list
        /// </summary>
        /// <returns> list that contains the parcels without drone </returns>
        public List<Parcel> GetParcelWithoutDrone()
        {
            List<Parcel> ParcelsWithoutDrone = new List<Parcel>();
            for (int i = 0; i < DataSource.Parcels.Count(); i++)
            {
                if (DataSource.Parcels[i].DroneId == 0)
                    ParcelsWithoutDrone.Add(DataSource.Parcels[i]);
            }
            return ParcelsWithoutDrone;
        }

        //public void toSexagesimal(double longitude, double latitude)
        //{
        //    string longResult = "";
        //    longResult += ((int)longitude).ToString() + "° ";
        //    double tmp = (longitude - (int)longitude) * 60;
        //    longResult += ((int)tmp).ToString() + "\' ";
        //    tmp = (tmp - (int)tmp) * 60;
        //    longResult += tmp.ToString() + "\"";
        //}

        public List<Station> AvailableCharger()
        {
            List<Station> AvailableChargers = new List<Station>();
            for (int i = 0; i < DataSource.Stations.Count(); i++)
            {
                if (DataSource.Stations[i].ChargeSlots != 0)
                    AvailableChargers.Add(DataSource.Stations[i]);
            }
            return AvailableChargers;
        }


        public void AddDrone(Drone drone)
        {
            DataSource.Drones.Add(drone);
        }

        public void AddCustomer(Customer c)
        {
            DataSource.Customers.Add(c);
        }

        public void AddStation(Station station)
        {
            DataSource.Stations.Add(station);
        }

        public int AddParcel(Parcel parcel)
        {
            parcel.Id = DataSource.Config.ParcelId;
            //DataSource.Config.ParcelId++;
            DataSource.Parcels.Add(parcel);
            return parcel.Id;
        }

        public void MatchDroneToParcel(Parcel p, Drone d)
        {
            Parcel newParcel = p;
            Drone newDrone = d;
            newParcel.DroneId = d.Id;
            newParcel.Scheduled = DateTime.Now;
            newDrone.Status = DroneStatus.Assigned;

            DataSource.Drones.Remove(d);
            DataSource.Drones.Add(newDrone);
            DataSource.Parcels.Remove(p);
            DataSource.Parcels.Add(newParcel);
        }

        public void PickUpParcel(Parcel p)
        {
            Parcel newParcel = p;
            newParcel.PickedUp = DateTime.Now;
            DataSource.Parcels.Remove(p);
            DataSource.Parcels.Add(newParcel);
        }

        public void ParcelDelivered(Parcel p)
        {
            // receive drone and change drone stat to assigned
            Drone newDrone = GetDrone(p.DroneId);
            newDrone.Status = DroneStatus.Delivery;
            Parcel newParcel = p;
            newParcel.Delivered = DateTime.Now;

            DataSource.Drones.Remove(GetDrone(p.DroneId));
            DataSource.Drones.Add(newDrone);
            DataSource.Parcels.Remove(p);
            DataSource.Parcels.Add(newParcel);
        }

        public void SendDroneToCharge(Drone d, Station s)
        {
            Drone newDrone = d;
            Station newStation = s;
            newDrone.Status = DroneStatus.Maintenance;
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
        /// the function creates a new drone with the old drone data, updates
        /// </summary>
        /// <param name="d"> the drone to charge </param>
        public void SendDroneFromStation(Drone d)
        {
            DroneCharge dronecharge = GetDroneCharge(d.Id);
            Station s = GetStation(dronecharge.StationId);
            s.ChargeSlots++;
            d.Status = DroneStatus.Available;
            d.Battery = 100;
            DataSource.DroneCharges.Remove(dronecharge);
        }


    }
}