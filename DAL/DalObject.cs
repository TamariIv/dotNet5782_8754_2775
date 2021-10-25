using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;


namespace DAL
{
    namespace DalObject
    {

        public class DalObject
        {
            public DalObject() ///constructor
            {
                DataSource.Initialize();
            }
            public static Station ReturnStationData(int idNumber)  //search the station by idNumber and return it
            {
                Station s = new Station();
                for (int i = 0; i < DataSource.Stations.Count; i++)
                {
                    if (DataSource.Stations[i].Id == idNumber)
                        s = DataSource.Stations[i];
                }
                return s;
            }

            public static Drone ReturnDroneData(int idNumber) //search the drone by idNumber and return it
            {
                Drone d = new Drone();
                for (int i = 0; i < DataSource.Drones.Count; i++)
                {
                    if (DataSource.Drones[i].Id == idNumber)
                        d = DataSource.Drones[i];
                }
                return d;
            }

            public static Customer ReturnCustomerData(int idNumber) //search the customer by idNumber and return it
            {
                Customer c = new Customer();
                for (int i = 0; i < DataSource.Customers.Count; i++)
                {
                    if (DataSource.Customers[i].Id == idNumber)
                        c = DataSource.Customers[i];
                }
                return c;
            }

            public static Parcel ReturnParcelData(int idNumber) //search the parcel by idNumber and return it
            {
                Parcel p = new Parcel();
                for (int i = 0; i < DataSource.Parcels.Count; i++)
                {
                    if (DataSource.Parcels[i].Id == idNumber)
                        p = DataSource.Parcels[i];
                }
                return p;
            }

            public static List<Station> GetStations()
            {
                return DataSource.Stations;
            }

            public static List<Drone> GetDrones()
            {
                return DataSource.Drones;
            }

            public static List<Customer> GetCustomers()
            {
                return DataSource.Customers;
            }

            public static List<Parcel> GetParcels()
            {
                return DataSource.Parcels;
            }

            /// <summary>
            /// search the parcels without drones in the Parcels list
            /// </summary>
            /// <returns> list that contains the parcels without drone </returns>
            public static List<Parcel> ParcelWithoutDrone()
            {
                List<Parcel> ParcelsWithoutDrone = new List<Parcel>();
                for (int i = 0; i < DataSource.Parcels.Count; i++)
                {
                    if (DataSource.Parcels[i].DroneId == 0)
                        ParcelsWithoutDrone.Add(DataSource.Parcels[i]);
                }
                return ParcelsWithoutDrone;
            }



            public static List<Station> AvailableCharger()
            {
                List<Station> AvailableChargers = new List<Station>();
                for (int i = 0; i < DataSource.Stations.Count; i++)
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
            

            public static void AddDrone(int _id, string _model, WeightCategories _maxWeight, DroneStatus _status, double _battery)
            {
                Drone d = new Drone();
                d.Id = _id;
                d.Model = _model;
                d.MaxWeight = _maxWeight;
                d.Status = _status;
                d.Battery = _battery;
                DataSource.Drones.Add(d);
            }
        }
           
        
       


    }

    
   
}
