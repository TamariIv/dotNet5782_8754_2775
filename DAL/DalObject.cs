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
                List<Station> copyStations = new List<Station>();
                for (int i = 0; i < DataSource.Stations.Count; i++)
                {
                    copyStations[i] = DataSource.Stations[i];
                    //copyStations.Add(DataSource.Stations[i]);
                }
                return copyStations;
            }

            public static List<Drone> GetDrones()
            {
                List<Drone> copyDrones = new List<Drone>();
                for (int i = 0; i < DataSource.Drones.Count; i++)
                {
                    copyDrones.Add(DataSource.Drones[i]);
                }
                return copyDrones;
            }

            public static List<Customer> GetCustomers()
            {
                List<Customer> copyCustomers = new List<Customer>();
                for (int i = 0; i < DataSource.Customers.Count; i++)
                {
                    copyCustomers.Add(DataSource.Customers[i]);
                }
                return copyCustomers;
            }

            public static List<Parcel> GetParcels()
            {
                List<Parcel> copyParcels = new List<Parcel>();
                for (int i = 0; i < DataSource.Parcels.Count; i++)
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
                p.DroneId = d.Id;
                p.Scheduled = DateTime.Now;
                d.Status = (DroneStatus)2;

            //    int i = 0;
            //    while (DataSource.Drones[i].Status != (DroneStatus)1 || DataSource.Drones[i].Battery == 0)
            //        i++;
            //    if (DataSource.Drones[i].Status == (DroneStatus)1 && DataSource.Drones[i].Battery != 0)
            //    {
            //        p.DroneId = DataSource.Drones[i].Id;
            //        //DataSource.Drones[i].Status = (DroneStatus)2;
            //        //date??
            //        //GetDrones()[i].Status = (DroneStatus)2;
            //        DAL.DO.Drone tmp = DataSource.Drones[i];
            //        tmp.Status = (DroneStatus)2;
            //        DataSource.Drones[i] = tmp;
            //    }
            }

            public static void PickUpParcel(Parcel p)
            {
                p.PickedUp = DateTime.Now;
            }

            public static void ParcelDelivered(Parcel p)
            {
                p.Delivered = DateTime.Now;
            }

            public static void SendDroneToCharge(Drone d, Station s)
            {
                d.Status = (DroneStatus)0;
                s.ChargeSlots--;
                DroneCharge dc = new DroneCharge();
                dc.DroneId = d.Id;
                dc.StationId = s.Id;
            }

            public static void SendDroneFromStation(Drone d, Station s)
            {
                s.ChargeSlots++;
                d.Status = (DroneStatus)1;
                d.Battery = 100;
            }
        }



    }



}
