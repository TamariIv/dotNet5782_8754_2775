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
        /// <summary>
        /// constructor
        /// </summary>
        public DalObject() 
        {
            DataSource.Initialize();
        }
        /// <summary>
        /// search the station by idNumber 
        /// </summary>
        /// <returns> the station by idNumber </returns>
        public static Station ReturnStationData(int idNumber) 
        {
            
            Station s = new Station(); //empty object to return in case the id was not found

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
        /// <returns>the drone by idNumber </returns>
        public static Drone ReturnDroneData(int idNumber) 
        {
            Drone d = new Drone(); //empty object to return in case the id was not found
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
        public static Customer ReturnCustomerData(int idNumber) 
        {

            Customer c = new Customer(); //empty object to return in case the id was not found
            for (int i = 0; i < DataSource.Customers.Count(); i++)
            {
                if (DataSource.Customers[i].Id == idNumber)
                    return DataSource.Customers[i];
            }
            return c;
        }

        public static Parcel ReturnParcelData(int idNumber) //search the parcel by idNumber and return it
        {

            foreach (var item in DataSource.Parcels)
            {
                if (item.Id == idNumber)
                    return item;
            }
            Parcel p = new Parcel();
            return p;

            //Parcel p = new Parcel();
            //for (int i = 0; i < DataSource.Parcels.Count(); i++)
            //{
            //    if (DataSource.Parcels[i].Id == idNumber)
            //        p = DataSource.Parcels[i];
            //}
            //return p;
        }
        public static DroneCharge ReturnDroneCharge(int idNumber)
        {
            DroneCharge d = new DroneCharge();
            for (int i = 0; i < DataSource.DroneCharges.Count(); i++)
            {
                if (DataSource.DroneCharges[i].DroneId == idNumber)
                    d = DataSource.DroneCharges[i];
            }
            return d;
        }

        public static List<Station> GetStations()
        {
            List<Station> copyStations = new List<Station>();
            for (int i = 0; i < DataSource.Stations.Count(); i++)
            {
                //copyStations[i] = DataSource.Stations[i];
                Station s = DataSource.Stations[i];
                copyStations.Add(s);
            }
            return copyStations;
        }

        public static List<Drone> GetDrones()
        {
            List<Drone> copyDrones = new List<Drone>();
            for (int i = 0; i < DataSource.Drones.Count(); i++)
            {
                copyDrones.Add(DataSource.Drones[i]);
            }
            return copyDrones;
        }

        public static List<Customer> GetCustomers()
        {
            List<Customer> copyCustomers = new List<Customer>();
            for (int i = 0; i < DataSource.Customers.Count(); i++)
            {
                copyCustomers.Add(DataSource.Customers[i]);
            }
            return copyCustomers;
        }

        public static List<Parcel> GetParcels()
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
        public static List<Parcel> ParcelWithoutDrone()
        {
            List<Parcel> ParcelsWithoutDrone = new List<Parcel>();
            for (int i = 0; i < DataSource.Parcels.Count(); i++)
            {
                if (DataSource.Parcels[i].DroneId == 0)
                    ParcelsWithoutDrone.Add(DataSource.Parcels[i]);
            }
            return ParcelsWithoutDrone;
        }



        public static List<Station> AvailableCharger()
        {
            List<Station> AvailableChargers = new List<Station>();
            for (int i = 0; i < DataSource.Stations.Count(); i++)
            {
                if (DataSource.Stations[i].ChargeSlots != 0)
                    AvailableChargers.Add(DataSource.Stations[i]);
            }
            return AvailableChargers;
        }


        public static void AddStation(int _id, string _name, double _longitude, double _latitude, int _chargeSlots)
        {
            Station s = new Station();
            s.Id = _id;
            s.Name = _name;
            s.Longitude = _longitude;
            s.Latitude = _latitude;
            s.ChargeSlots = _chargeSlots;
            DataSource.Stations.Add(s);
        }


        public static void AddDrone(int _id, string _model, WeightCategories _maxWeight/*, DroneStatus _status, double _battery*/)
        {
            Drone d = new Drone();
            d.Id = _id;
            d.Model = _model;
            d.MaxWeight = _maxWeight;
            d.Status = (DroneStatus)1/*_status*/;
            d.Battery = 100/*_battery*/;
            DataSource.Drones.Add(d);
        }


        public static void NewCustomer(int _id, string _name, string _phone, double _longitude, double _latitude)
        {
            Customer c = new Customer();
            c.Id = _id;
            c.Name = _name;
            c.Phone = _phone;
            c.Latitude = _latitude;
            c.Longitude = _longitude;
            DataSource.Customers.Add(c);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns> the id of the next new parcel </returns>
        public static int NewParcel(int _senderId, int _targetId, WeightCategories _maxWeight, Priorities _priority)
        {
            Parcel p = new Parcel();
            p.Id = DataSource.Config.ParcelId;
            p.SenderId = _senderId;
            p.TargetId = _targetId;
            p.Weight = _maxWeight;
            p.Priority = _priority;
            p.DroneId = 0;
            DataSource.Parcels.Add(p);
            return DataSource.Config.ParcelId++;
        }

        public static void MatchDroneToParcel(Parcel p, Drone d)
        {
            Parcel newp = p;
            Drone newd = d;
            newp.DroneId = d.Id;
            newp.Scheduled = DateTime.Now;
            newd.Status = DroneStatus.assigned;

            DataSource.Drones.Remove(d);
            DataSource.Drones.Add(newd);
            DataSource.Parcels.Remove(p);
            DataSource.Parcels.Add(newp);
        }

        public static void PickUpParcel(Parcel p)
        {
            Parcel newp = p;
            newp.PickedUp = DateTime.Now;
            DataSource.Parcels.Remove(p);
            DataSource.Parcels.Add(newp);
        }

        public static void ParcelDelivered(Parcel p)
        {
            // receive drone and change drone stat to assigned
            Drone newDrone = ReturnDroneData(p.DroneId);
            newDrone.Status = DroneStatus.delivery;
            Parcel newp = p;
            newp.Delivered = DateTime.Now;

            DataSource.Drones.Remove(ReturnDroneData(p.DroneId));
            DataSource.Drones.Add(newDrone);
            DataSource.Parcels.Remove(p);
            DataSource.Parcels.Add(newp);
        }

        public static void SendDroneToCharge(Drone d, Station s)
        {
            Drone newd = d;
            Station news = s;
            newd.Status = DroneStatus.maintenance;
            news.ChargeSlots--;
            DroneCharge dc = new DroneCharge();
            dc.DroneId = newd.Id;
            dc.StationId = news.Id;

            DataSource.Drones.Remove(d);
            DataSource.Drones.Add(newd);
            DataSource.Stations.Remove(s);
            DataSource.Stations.Add(news);
        }
        /// <summary>
        /// the function creates a new drone with the old drone data, updates
        /// </summary>
        /// <param name="d"> the drone to charge </param>
        public static void SendDroneFromStation(Drone d)
        {
            DroneCharge dc = ReturnDroneCharge(d.Id);
            Station s = ReturnStationData(dc.StationId);
            s.ChargeSlots++;
            d.Status = DroneStatus.available;
            d.Battery = 100;
            DataSource.DroneCharges.Remove(dc);
        }
    }


}
