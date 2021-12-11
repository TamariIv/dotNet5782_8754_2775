using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DataSource
    {
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<Station> Stations = new List<Station>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<Parcel> Parcels = new List<Parcel>();
        internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();

        internal static Random r = new Random();

        internal class Config
        {
            internal static int ParcelId = 1000000;
            internal static double WhenAvailable { get { return 10; } }
            internal static double WhenLightWeight { get { return 30; } }
            internal static double WhenMediumWeight { get { return 40; } }
            internal static double WhenHeavyWeight { get { return 50; } }
            internal static double ChargingRate { get { return 15; } }
        }
        /// <summary>
        /// initialize the data lists 
        /// </summary>
        public static void Initialize()
        {
            //initialize 2 stations:
            createStation();
            //initialize 10 customers:
            createCustomer(10);
            //initialize 10 parcels:
            createParcel(10);
            //initialize 5 drones:
            createDrone(5);
        }

        private static void createStation()
        {
            Station s = new Station
            {
                Id = r.Next(10000, 100000),
                Name = "Pisgat Zeev",
                Longitude = 31.831146,
                Latitude = 35.242632,
                AvailableChargeSlots = r.Next(11)
            };
            Stations.Add(s);

            s = new Station()
            {
                Id = r.Next(10000, 100000),
                Name = "Givat Shaul",
                Longitude = 31.790835,
                Latitude = 35.195144,
                AvailableChargeSlots = r.Next(11)
            };
            Stations.Add(s);
        }
        private static void createCustomer(int numOfCustomers)
        {
            string[] namesArray = { "Avraham Cohen", "Yitshak Levi", "Yaakov Israeli", "Sarah Shalom", "Rivka Silver", "Rahel Shushan", "Leah Yosefi", "David Dayan", "Moshe Biton", "Aharon Uzan" };
            string[] firstDigits = { "050-", "052-", "054-" };
            for (int i = 0; i < numOfCustomers; i++)
            {
                Customer c = new Customer()
                {
                    Id = r.Next(100000000, 1000000000),
                    Name = namesArray[i],
                    Phone = firstDigits[r.Next(3)] + r.Next(1000000, 10000000).ToString(),
                    Latitude = 35 + r.NextDouble(),
                    Longitude = 31 + r.NextDouble()
                };
                Customers.Add(c);
            }

        }
        private static void createParcel(int numOfParcels)
        {
            Parcel p = new Parcel()
            {
                Id = Config.ParcelId++,
                SenderId = r.Next(100000000, 1000000000),
                TargetId = r.Next(100000000, 1000000000),
                Weight = (WeightCategories)r.Next(3),
                Priority = (Priorities)r.Next(3),
                Requested = new DateTime(2020, 05, 09, 8, 10, 00),
                Scheduled = new DateTime(2020, 05, 09, 8, 30, 00),
                PickedUp = null,
                Delivered = null,
                DroneId = 0
            };
            Parcels.Add(p);

            p = new Parcel()
            {
                Id = Config.ParcelId++,
                SenderId = r.Next(100000000, 1000000000),
                TargetId = r.Next(100000000, 1000000000),
                Weight = (WeightCategories)r.Next(3),
                Priority = (Priorities)r.Next(3),
                Requested = new DateTime(2020, 09, 09, 20, 00, 00),
                Scheduled = new DateTime(2020, 09, 09, 21, 10, 00),
                PickedUp = new DateTime(2020, 09, 09, 21, 30, 00),
                Delivered = null,
                DroneId = 0
            };
            Parcels.Add(p);

            p = new Parcel()
            {
                Id = Config.ParcelId++,
                SenderId = r.Next(100000000, 1000000000),
                TargetId = r.Next(100000000, 1000000000),
                Weight = (WeightCategories)r.Next(3),
                Priority = (Priorities)r.Next(3),
                Requested = new DateTime(2020, 09, 12, 20, 00, 00),
                Scheduled = new DateTime(2020, 09, 12, 21, 10, 00),
                PickedUp = new DateTime(2020, 09, 12, 21, 30, 00),
                Delivered = new DateTime(2020, 09, 12, 22, 00, 00),
                DroneId = 0
            };
            Parcels.Add(p);

            p = new Parcel()
            {
                Id = Config.ParcelId++,
                SenderId = r.Next(100000000, 1000000000),
                TargetId = r.Next(100000000, 1000000000),
                Weight = (WeightCategories)r.Next(3),
                Priority = (Priorities)r.Next(3),
                Requested = new DateTime(2020, 11, 10, 20, 00, 00),
                Scheduled = new DateTime(2020, 11, 10, 21, 10, 00),
                PickedUp = new DateTime(2020, 11, 10, 21, 30, 00),
                Delivered = null,
                DroneId = 0
            };
            Parcels.Add(p);

            p = new Parcel()
            Parcel p = new Parcel
            {
                Id = Config.ParcelId++,
                SenderId = Customers[r.Next(Customers.Count())].Id,
                TargetId = Customers[r.Next(Customers.Count())].Id,
                Weight = (WeightCategories)r.Next(3),
                Priority = (Priorities)r.Next(3),
                Requested = new DateTime(2021, 3, 12, 15, 25, 30),
                Scheduled = null,
                PickedUp = null,
                Delivered = null,
                DroneId = 0
            };
            Parcels.Add(p);


            p = new Parcel
            {
                Id = Config.ParcelId++,
                SenderId = Customers[r.Next(Customers.Count())].Id,
                TargetId = Customers[r.Next(Customers.Count())].Id,
                Weight = (WeightCategories)r.Next(3),
                Priority = (Priorities)r.Next(3),
                Requested = new DateTime(2021, 4, 4, 12, 25, 0),
                Scheduled = null,
                PickedUp = null,
                Delivered = null,
                DroneId = 0
            };
            Parcels.Add(p);

            p = new Parcel
            {
                Id = Config.ParcelId++,
                SenderId = Customers[r.Next(Customers.Count())].Id,
                TargetId = Customers[r.Next(Customers.Count())].Id,
                Weight = (WeightCategories)r.Next(3),
                Priority = (Priorities)r.Next(3),
                Requested = new DateTime(2021, 8, 1, 1, 12, 56),
                Scheduled = new DateTime(2021, 8, 2, 12, 0, 0),
                PickedUp = new DateTime(2021, 8, 2, 16, 15, 00),
                Delivered = null,
                DroneId = 0
            };
            Parcels.Add(p);


            p = new Parcel
            {
                Id = Config.ParcelId++,
                SenderId = r.Next(100000000, 1000000000),
                TargetId = r.Next(100000000, 1000000000),
                Weight = (WeightCategories)r.Next(3),
                Priority = (Priorities)r.Next(3),
                Requested = new DateTime(2020, 10, 05, 12, 00, 00),
                Scheduled = new DateTime(2020, 10, 05, 12, 10, 00),
                PickedUp = new DateTime(2020, 10, 05, 12, 30, 00),
                Delivered = new DateTime(2020, 10, 05, 13, 00, 00),
                DroneId = 0
            };
            Parcels.Add(p);

        }
                Id = Config.ParcelId++,
                SenderId = Customers[r.Next(Customers.Count())].Id,
                TargetId = Customers[r.Next(Customers.Count())].Id,
                Weight = (WeightCategories)r.Next(3),
                Priority = (Priorities)r.Next(3),
                Requested = new DateTime(2021, 10, 13, 0, 8, 0),
                Scheduled = new DateTime(2021, 10, 2, 12, 0, 0),
                PickedUp = new DateTime(2021, 10, 2, 16, 15, 0),
                Delivered = new DateTime(2021, 10, 2, 17, 0, 0),
                DroneId = 0
            };
            Parcels.Add(p);

            //for (int i = 0; i < numOfParcels; i++)
            //{
            //    Parcel p = new Parcel();
            //    p.Id = Config.ParcelId++;
            //    p.SenderId = Customers[r.Next(Customers.Count())].Id;
            //    int tmp = Customers[r.Next(Customers.Count())].Id;
            //    while (tmp == p.SenderId)
            //        tmp = Customers[r.Next(Customers.Count())].Id;
            //    p.TargetId = tmp;
            //    p.Weight = (WeightCategories)r.Next(3);
            //    p.Priority = (Priorities)r.Next(3);
            //    p.Requested = null;
            //    p.Scheduled = null;
            //    p.PickedUp = null;
            //    p.Delivered = null;
            //    p.DroneId = 0;
            //    Parcels.Add(p);
            //}

        //for (int i = 0; i < numOfParcels; i++)
        //{
        //    Parcel p = new Parcel();
        //    p.Id = Config.ParcelId++;
        //    p.SenderId = r.Next(100000000, 1000000000);
        //    p.TargetId = r.Next(100000000, 1000000000);
        //    p.Weight = (WeightCategories)r.Next(3);
        //    p.Priority = (Priorities)r.Next(3);
        //    p.Requested = null;
        //    p.Scheduled = null;
        //    p.PickedUp = null;
        //    p.Delivered = null;
        //    p.DroneId = 0;
        //    Parcels.Add(p);
        //}

        private static void createDrone(int numOfDrones)
        {
            for (int i = 0; i < numOfDrones; i++)
            {
                Drone d = new Drone()
                {
                    Id = r.Next(1001, 10000),
                    Model = ((char)(r.Next(65, 91)) + ((char)(r.Next(65, 91)) + (r.Next(111, 999)).ToString())), // for example: SE503
                    MaxWeight = (WeightCategories)r.Next(3),                  
                };
                Drones.Add(d);
            }


        }
    }
}


