using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DalObject;


namespace ConsoleUI
{
    class Program
    {
        enum MenuOptions { Add = 1, Update, Show_One, Show_List, Exit }
        enum EntityOptions { Parcel = 1, Drone, BaseStation, Customer,Exit }
        enum ListOptions { BaseStations = 1, Drones, Customers, Parcels, ParcelsWithoutDrone, AvailableChargingStation,Exit }
        enum UpdateOptions { DroneToParcel = 1, PickedUp, Delivery, Recharge, FreeDrone,Exit }

        static DalObject.DalObject mydal;

        static void Main(string[] args)
        {

            MenuOptions menuOptions;
            EntityOptions entityOptions;
            ListOptions listOptions;
            mydal = new DalObject.DalObject();
            Console.WriteLine("press 1 to add an item");
            Console.WriteLine("press 2 to update an item");
            Console.WriteLine("press 3 to view details of specific item");
            Console.WriteLine("press 4 to view a list of specific item");
            Console.WriteLine("press 0 to stop");

            menuOptions = (MenuOptions)int.Parse(Console.ReadLine());
            while (menuOptions != MenuOptions.Exit)
            {
                switch (menuOptions)
                {
                    case MenuOptions.Add:
                        Console.WriteLine("Press 1 to add parcel");
                        Console.WriteLine("Press 2 to add drone");
                        Console.WriteLine("Press 3 to add station");
                        Console.WriteLine("Press 4 to add customer");
                        entityOptions = (EntityOptions)int.Parse(Console.ReadLine());
                        switch (entityOptions)
                        {
                            case EntityOptions.Parcel:
                                AddParcel();
                                break;
                            case EntityOptions.Drone:
                                AddDrone();
                                break;
                            case EntityOptions.BaseStation:
                                AddStation();
                                break;
                            case EntityOptions.Customer:
                                AddCustomer();
                                break;
                            case EntityOptions.Exit:
                                break;
                            default:
                                break;
                        }
                        break;

                    case MenuOptions.Update:
                        Console.WriteLine("Press 1 to match drone to parcel");
                        Console.WriteLine("Press 2 to pick up parcel");
                        Console.WriteLine("Press 3 to deliver parcel to customer");
                        Console.WriteLine("Press 4 to send drone to charge");
                        Console.WriteLine("Press 5 to free drone from charging");
                        UpdateOptions updateChoice = (UpdateOptions)Enum.Parse(typeof(UpdateOptions), Console.ReadLine());
                        switch (updateChoice)
                        {
                            case UpdateOptions.DroneToParcel:
                                DroneToParcel();
                                break;
                            case UpdateOptions.PickedUp:
                                PickUpParcel();
                                break;
                            case UpdateOptions.Delivery:
                                DeliverParcel();
                                break;
                            case UpdateOptions.Recharge:
                                SendDroneToStation();
                                break;
                            case UpdateOptions.FreeDrone:
                               FreeDrone();
                                break;
                            case UpdateOptions.Exit:
                                break;
                            default:
                                break;
                        }
                        break;

                    case MenuOptions.Show_One:
                        Console.WriteLine("press 1 to view details of a specific parcel");
                        Console.WriteLine("press 2 to view details of a specific drone");
                        Console.WriteLine("press 3 to view details of a specific base station");
                        Console.WriteLine("press 4 to view details of a specific customer");
                        entityOptions = (EntityOptions)int.Parse(Console.ReadLine());
                        PrintSpecificItem(entityOptions);
                        break;

                    case MenuOptions.Show_List:
                        Console.WriteLine("press 1 to view the list of base stations");
                        Console.WriteLine("press 2 to view the list of the drones");
                        Console.WriteLine("press 3 to view the list of the cutomers");
                        Console.WriteLine("press 4 to view the list of the parcels");
                        Console.WriteLine("press 5 to view the list of the parcels without drones");
                        Console.WriteLine("press 6 to view the list of the stations with available charge slots");
                        listOptions = (ListOptions)int.Parse(Console.ReadLine());
                        PrintSpecificList(listOptions);
                        break;
                    case MenuOptions.Exit:
                        break;
                    default:
                        break;
                }
                menuOptions = (MenuOptions)int.Parse(Console.ReadLine());
            }
        }

        /// <summary>
        /// get from user the senderId, targetId, weight of the parcel, and priority 
        /// send everything to newParcel to create a new parcel with the data
        /// </summary>
        public static void AddParcel()
        {
            int senderId, targetId;
            Console.WriteLine("Enter sender ID: ");
            int.TryParse(Console.ReadLine(), out senderId);
            Console.WriteLine("Enter target ID: ");
            int.TryParse(Console.ReadLine(), out targetId);
            Console.WriteLine("Weight of the parcel: press 1 for heavy, 2 for medium and 3 for light: ");
            WeightCategories weight = (WeightCategories)Enum.Parse(typeof(WeightCategories), Console.ReadLine());
            Console.WriteLine("Priorities of the parcel: press 1 for regular, 2 for rapid and 3 for emergency: ");
            Priorities priority = (Priorities)Enum.Parse(typeof(Priorities), Console.ReadLine());

            Parcel parcel = new Parcel
            {
                SenderId = senderId,
                TargetId = targetId,
                Weight = weight,
                Priority = priority,
            };

            mydal.AddParcel(parcel);
        }
        /// <summary>
        /// get from user the drone id, model, and maximum weight 
        /// send all of them to addDrone to create a new drone with the data
        /// </summary>
        public static void AddDrone()
        {
            int id;
            string model;
            WeightCategories maxWeight;
            Console.WriteLine("Enter drone ID: ");
            int.TryParse(Console.ReadLine(), out id);
            Console.WriteLine("Enter drone model: ");
            model = Console.ReadLine();
            Console.WriteLine("Maximum weight of the parcel: press 1 for heavy, 2 for medium and 3 for light: ");
            int tmp;
            int.TryParse(Console.ReadLine(), out tmp);
            maxWeight = (WeightCategories)(tmp - 1);

            Drone drone = new Drone
            {
                Id = id,
                Model = model,
                Battery = 100,
                MaxWeight = maxWeight,
                Status = DroneStatus.Available
            };

            mydal.AddDrone(drone);
        }
        /// <summary>
        /// get from user the station id and name, the location (longitude, latitude), and the number of empty charging slots  
        /// send everything to addStation to create a new station with the data
        /// </summary>
        public static void AddStation()
        {
            int id, slots;
            string name;
            double longitude, latitude;
            Console.WriteLine("Enter station ID: ");
            int.TryParse(Console.ReadLine(), out id);
            Console.WriteLine("Enter station name: ");
            name = Console.ReadLine();
            Console.WriteLine("Enter station longitude: ");
            double.TryParse(Console.ReadLine(), out longitude);
            Console.WriteLine("Enter station latitude: ");
            double.TryParse(Console.ReadLine(), out latitude);
            Console.WriteLine("Enter number of empty charging slots: ");
            int.TryParse(Console.ReadLine(), out slots);
            Station newStation = new Station
            {
                Id = id,
                Name = name,
                Longitude = longitude,
                Latitude = latitude,
                ChargeSlots = slots
            };
            mydal.AddStation(newStation);
        }
        /// <summary>
        /// get from user the customer id, name, ohone number, and location (longitude, latitude)
        /// send everything to newCustomer to create a new cusyomer with the data
        /// </summary>
        public static void AddCustomer()
        {
            // continue to change to tryparse from here
            int id;
            string name, phone;
            double longitude, latitude;
            Console.WriteLine("Enter customer ID: ");
            int.TryParse(Console.ReadLine(), out id);
            Console.WriteLine("Enter customer name: ");
            name = Console.ReadLine();
            Console.WriteLine("Enter phone number: ");
            phone = Console.ReadLine();
            Console.WriteLine("Enter customer longitude: ");
            double.TryParse(Console.ReadLine(), out longitude);
            Console.WriteLine("Enter customer latitude: ");
            double.TryParse(Console.ReadLine(), out latitude);
            Customer customer = new Customer
            {
                Id = id,
                Name = name,
                Phone = phone,
                Longitude = longitude,
                Latitude = latitude,
            };
            mydal.AddCustomer(customer);
        }

        public static void PrintSpecificItem(EntityOptions entityOptions)
        {
            int id;
            switch (entityOptions)
            {
                case EntityOptions.Parcel:
                    {
                        Console.WriteLine("enter ID of the parcel");
                        int.TryParse(Console.ReadLine(), out id);
                        Parcel p = mydal.GetParcel(id);
                        Console.WriteLine(p);
                        break;
                    }
                case EntityOptions.Drone:
                    {
                        Console.WriteLine("enter ID of the drone");
                        int.TryParse(Console.ReadLine(), out id);
                        Drone d = mydal.GetDrone(id);
                        Console.WriteLine(d);
                        break;
                    }
                case EntityOptions.BaseStation:
                    {
                        Console.WriteLine("enter ID of the station");
                        int.TryParse(Console.ReadLine(), out id);
                        Station s = mydal.GetStation(id);
                        Console.WriteLine(s);
                        break;
                    }
                case EntityOptions.Customer:
                    {
                        Console.WriteLine("enter ID of the customer");
                        int.TryParse(Console.ReadLine(), out id);
                        Customer c = mydal.GetCustomer(id);
                        Console.WriteLine(c);
                        break;
                    }
                case EntityOptions.Exit:
                    break;
                default:
                    break;

            }
        }
        /// <summary>
        /// the function prints a specific list
        /// </summary>
        /// <param name="option"> choice of the list to print </param>
        public static void PrintSpecificList(ListOptions listOptions)
        {
            //
            switch (listOptions)
            {
                case ListOptions.BaseStations:
                    List<Station> stations = mydal.GetStations();
                    foreach (var item in stations)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case ListOptions.Drones:
                    List<Drone> drones = mydal.GetDrones();
                    foreach (var item in drones)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case ListOptions.Customers:
                    List<Customer> customers = mydal.GetCustomers();
                    foreach (var item in customers)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case ListOptions.Parcels:
                    List<Parcel> parcels = mydal.GetParcels();
                    foreach (var item in parcels)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case ListOptions.ParcelsWithoutDrone:
                    List<Parcel> parcelsWithoutDrones = mydal.GetParcelWithoutDrone();
                    foreach (var item in parcelsWithoutDrones)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case ListOptions.AvailableChargingStation:
                    List<Station> availableChargers = mydal.AvailableCharger();
                    foreach (var item in availableChargers)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case ListOptions.Exit:
                    break;
                default:
                    break;
            }
        }

        public static void DroneToParcel()
        {
            int droneId, parcelId;
            Console.WriteLine("Enter the ID of the drone you want to send: ");
            PrintSpecificList(2); //print the list of the drones for the user
            int.TryParse(Console.ReadLine(), out droneId);
            Console.WriteLine("Enter ID of the parcel you want to send: ");
            PrintSpecificList(4); //print the list of parcels for the user
            int.TryParse(Console.ReadLine(), out parcelId);
            mydal.MatchDroneToParcel(mydal.GetParcel(parcelId), mydal.GetDrone(droneId));
        }

        public static void PickUpParcel()
        {
            int id;
            Console.WriteLine("Enter the ID of the parcel you want to pick up: ");
            PrintSpecificList(4); //print the list of parcels for the user
            int.TryParse(Console.ReadLine(), out id);
            mydal.PickUpParcel(mydal.GetParcel(id));

        }

        public static void DeliverParcel()
        {
            int id;
            Console.WriteLine("Enter the ID of the parcel you want to deliver: ");
            PrintSpecificList(4); //print the list of parcels for the user
            int.TryParse(Console.ReadLine(), out id);
            mydal.ParcelDelivered(mydal.GetParcel(id));
        }

        public static void SendDroneToStation()
        {
            PrintSpecificList(6); // print the list of available charging stations (option 6 in PrintSpecificList) so the user can choose from them
            int droneId, stationId;
            Console.WriteLine("Enter the ID of the drone you want to charge: ");
            PrintSpecificList(2); //print the list of the drones
            int.TryParse(Console.ReadLine(), out droneId);
            Console.WriteLine("Enter the ID of the station you want to charge in: ");
            PrintSpecificList(1); //print the list of base stations for the user
            int.TryParse(Console.ReadLine(), out stationId);
            mydal.SendDroneToCharge(mydal.GetDrone(droneId), mydal.GetStation(stationId));
        }

        public static void FreeDrone()
        {
            int droneId;
            Console.WriteLine("Enter the ID of the drone you want to free: ");
            PrintSpecificList(2); //print the list of the drones
            droneId = int.Parse(Console.ReadLine());
            mydal.SendDroneFromStation(mydal.GetDrone(droneId));
        }
    }
}

