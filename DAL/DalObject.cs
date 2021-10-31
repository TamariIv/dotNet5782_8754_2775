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
        DalObject dalObject = new DalObject();

        public DalObject() //constructor
        {
            DataSource.Initialize();
        }
        public Station ReturnStationData(int idNumber)  //search the station by idNumber and return it
        {
            Station s = new Station(); 
            for (int i = 0; i < DataSource.Stations.Count(); i++)
            {
                if (DataSource.Stations[i].Id == idNumber)
                    return DataSource.Stations[i];
            }
            return s;

        }

        public static Drone ReturnDroneData(int idNumber) //search the drone by idNumber and return it
        {
            Drone d = new Drone();
            for (int i = 0; i < DataSource.Drones.Count(); i++)
            {
                if (DataSource.Drones[i].Id == idNumber)
                    return DataSource.Drones[i];
            }
            return d;
        }

        public static Customer ReturnCustomerData(int idNumber) //search the customer by idNumber and return it
        {
            Customer c = new Customer();
            for (int i = 0; i < DataSource.Customers.Count(); i++)
            {
                if (DataSource.Customers[i].Id == idNumber)
                    return DataSource.Customers[i];
            }
            return c;
        }

        public static Parcel ReturnParcelData(int idNumber) //search the parcel by idNumber and return it
        {
            Parcel p = new Parcel();
            for (int i = 0; i < DataSource.Parcels.Count(); i++)
            {
                if (DataSource.Parcels[i].Id == idNumber)
                    return DataSource.Parcels[i];
            }
            return p;
        }

        public static DroneCharge ReturnDroneCharge(int idNumber)
        {
            DroneCharge d = new DroneCharge();
            for (int i = 0; i < DataSource.DroneCharges.Count(); i++)
            {
                if (DataSource.DroneCharges[i].DroneId == idNumber)
                    return DataSource.DroneCharges[i];
            }
            return d;
        }

        public static List<Station> GetStations()
        {
            List<Station> copyStations = new List<Station>();
            for (int i = 0; i < DataSource.Stations.Count(); i++)
            {
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
            Station s = new Station()
            {
                Id = _id,
                Name = _name,
                Longitude = _longitude,
                Latitude = _latitude,
                ChargeSlots = _chargeSlots
            };
            DataSource.Stations.Add(s);
        }


        public static void AddDrone(int _id, string _model, WeightCategories _maxWeight)
        {
            Drone d = new Drone()
            {
                Id = _id,
                Model = _model,
                MaxWeight = _maxWeight,
                Status = DroneStatus.available,
                Battery = 100
            };
            DataSource.Drones.Add(d);
        }

        public static void NewCustomer(int _id, string _name, string _phone, double _longitude, double _latitude)
        {
            Customer c = new Customer()
            {
                Id = _id,
                Name = _name,
                Phone = _phone,
                Latitude = _latitude,
                Longitude = _longitude
            };
            DataSource.Customers.Add(c);
        }

        public static int NewParcel(int _senderId, int _targetId, WeightCategories _maxWeight, Priorities _priority)
        {
            Parcel p = new Parcel()
            {
                Id = DataSource.Config.ParcelId,
                SenderId = _senderId,
                TargetId = _targetId,
                Weight = _maxWeight,
                Priority = _priority,
                DroneId = 0
            };         
            DataSource.Parcels.Add(p);
            return DataSource.Config.ParcelId++;
        }

        public static void MatchDroneToParcel(Parcel p, Drone d)
        {
            Parcel newParcel = p;
            Drone newDrone = d;
            newParcel.DroneId = d.Id;
            newParcel.Scheduled = DateTime.Now;
            newDrone.Status = DroneStatus.delivery;

            DataSource.Drones.Remove(d);
            DataSource.Drones.Add(newDrone);
            DataSource.Parcels.Remove(p);
            DataSource.Parcels.Add(newParcel);
        }

        public static void PickUpParcel(Parcel p)
        {
            Parcel newParcel = p;
            newParcel.PickedUp = DateTime.Now;
            DataSource.Parcels.Remove(p);
            DataSource.Parcels.Add(newParcel);
        }

        public static void ParcelDelivered(Parcel p)
        {
            // receive drone and change drone stat to assigned
            Parcel newParcel = p;
            newParcel.Delivered = DateTime.Now;
            DataSource.Parcels.Remove(p);
            DataSource.Parcels.Add(newParcel);
        }

        public static void SendDroneToCharge(Drone d, Station s)
        {
            Drone newDrone = d;
            Station newStation = s;
            newDrone.Status = DroneStatus.maintenance;
            newStation.ChargeSlots--;
            DroneCharge dc = new DroneCharge();
            dc.DroneId = newDrone.Id;
            dc.StationId = newStation.Id;

            DataSource.Drones.Remove(d);
            DataSource.Drones.Add(newDrone);
            DataSource.Stations.Remove(s);
            DataSource.Stations.Add(newStation);
        }

        public static void SendDroneFromStation(Drone d)
        {
            DalObject dal = new DalObject();
            DroneCharge dronecharge = ReturnDroneCharge(d.Id);
            Station s =  dal.ReturnStationData(dronecharge.StationId);
            s.ChargeSlots++;
            d.Status = DroneStatus.available;
            d.Battery = 100;
            DataSource.DroneCharges.Remove(dronecharge);
        }
    }


}
