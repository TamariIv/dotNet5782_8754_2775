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
        enum MenuOptions { Add = 1, Update, Show_One, Show_List, EXIT }
        enum EntityOptions { Parcel = 1, Drone, BaseStation, Customer }
        enum ListOptions { BaseStations = 1, Drones, Customers, Parcels, ParcelsWithoutDrone, AvailableChargingStation }
        enum UpdateOptions { DroneToParcel = 1, PickedUp, Delivery, Recharge, FreeDrone }

        static DalObject.DalObject mydal;


        static void Main(string[] args)
        {

            mydal = new DalObject.DalObject();
            Console.WriteLine("Type Add to add an item");
            Console.WriteLine("Type Update to update an item");
            Console.WriteLine("Type Show_One to view details of specific item");
            Console.WriteLine("Type Show_List to view a list of specific item");
            Console.WriteLine("Type EXIT to stop");

            MenuOptions choice = (MenuOptions)Enum.Parse(typeof(MenuOptions), Console.ReadLine());
            while (choice != MenuOptions.EXIT)
            {
                switch (choice)
                {
                    case MenuOptions.Add:
                        Console.WriteLine("Press 1 to add parcel");
                        Console.WriteLine("Press 2 to add drone");
                        Console.WriteLine("Press 3 to add station");
                        Console.WriteLine("Press 4 to add customer");
                        //int innerChoice = Int32.Parse(Console.ReadLine());
                        EntityOptions innerChoice = (EntityOptions)Enum.Parse(typeof(EntityOptions), Console.ReadLine());
                        switch (innerChoice)
                        {
                            case EntityOptions.Parcel:
                              
                                break;
                            case EntityOptions.Drone:
                                mydal.GetDrones();
                                break;
                            case EntityOptions.BaseStation:
                                ReceiveStation();
                                break;
                            case EntityOptions.Customer:
                                ReceiveCustomer();
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
                                mydal.SendDroneToStation();
                                break;
                            case UpdateOptions.FreeDrone:
                                FreeDrone();
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
                        int option = int.Parse(Console.ReadLine());
                        PrintSpecificItem(option, mydal);
                        break;

                    case MenuOptions.Show_List:
                        Console.WriteLine("press 1 to view the list of base stations");
                        Console.WriteLine("press 2 to view the list of the drones");
                        Console.WriteLine("press 3 to view the list of the cutomers");
                        Console.WriteLine("press 4 to view the list of the parcels");
                        Console.WriteLine("press 5 to view the list of the parcels without drones");
                        Console.WriteLine("press 6 to view the list of the stations with available charge slots");
                        int.TryParse(Console.ReadLine(), out option);
                        PrintSpecificList(option);
                        break;
                    case MenuOptions.EXIT:
                        break;
                    default:
                        break;
                }
                choice = (MenuOptions)Enum.Parse(typeof(MenuOptions), Console.ReadLine());
            }
        }

        /// <summary>
        /// get from user the senderId, targetId, weight of the parcel, and priority 
        /// send everything to newParcel to create a new parcel with the data
        /// </summary>
        public void AddParcel(DalObject.DalObject mydal)
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
        /// send everything to addDrone to create a new drone with the data
        /// </summary>
        public void AddDrone(DalObject.DalObject mydal)
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
                Status = DroneStatus.available
            };

            mydal.AddDrone(drone);
        }
        /// <summary>
        /// get from user the station id and name, the location (longitude, latitude), and the number of empty charging slots  
        /// send everything to addStation to create a new station with the data
        /// </summary>
        public void AddStation(DalObject.DalObject mydal)
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
        public void AddCustomer(DalObject.DalObject mydal)
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

        public void PrintSpecificItem(int option, DalObject.DalObject mydal)
        {
            int id;
            switch (option)
            {
                case (int)EntityOptions.Parcel:
                    {
                        Console.WriteLine("enter ID of the parcel");
                        int.TryParse(Console.ReadLine(), out id);
                        Parcel p = mydal.ReturnParcelData(id);
                        Console.WriteLine(p);
                        break;
                    }
                case (int)EntityOptions.Drone:
                    {
                        Console.WriteLine("enter ID of the drone");
                        int.TryParse(Console.ReadLine(), out id);
                        Drone d = mydal.ReturnDroneData(id);
                        Console.WriteLine(d);
                        break;
                    }
                case (int)EntityOptions.BaseStation:
                    {
                        Console.WriteLine("enter ID of the station");
                        int.TryParse(Console.ReadLine(), out id);
                        Station s = mydal.GetStation(id);
                        Console.WriteLine(s);
                        break;
                    }
                case (int)EntityOptions.Customer:
                    {
                        Console.WriteLine("enter ID of the customer");
                        int.TryParse(Console.ReadLine(), out id);
                        Customer c = mydal.ReturnCustomerData(id);
                        Console.WriteLine(c);
                        break;
                    }
                default:
                    break;

            }
        }
        /// <summary>
        /// the function prints a specific list
        /// </summary>
        /// <param name="option"> choice of the list to print </param>
        public static void PrintSpecificList(int option)
        {
            //
            switch (option)
            {
                case (int)ListOptions.BaseStations:
                    List<Station> stations = mydal.GetStations();
                    foreach (var item in stations)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case (int)ListOptions.Drones:
                    List<Drone> drones = mydal.GetDrones();
                    foreach (var item in drones)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case (int)ListOptions.Customers:
                    List<Customer> customers = mydal.GetCustomers();
                    foreach (var item in customers)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case (int)ListOptions.Parcels:
                    List<Parcel> parcels = mydal.GetParcels();
                    foreach (var item in parcels)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case (int)ListOptions.ParcelsWithoutDrone:
                    List<Parcel> parcelsWithoutDrones = mydal.GetParcelWithoutDrone();
                    foreach (var item in parcelsWithoutDrones)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case (int)ListOptions.AvailableChargingStation:
                    List<Station> availableChargers = mydal.AvailableCharger();
                    foreach (var item in availableChargers)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                default:
                    break;
            }
        }

        public static void DroneToParcel(DalObject.DalObject mydal)
        {
            int droneId, parcelId;
            Console.WriteLine("Enter the ID of the drone you want to send: ");
            int.TryParse(Console.ReadLine(), out droneId);
            Console.WriteLine("Enter ID of the parcel you want to send: ");
            int.TryParse(Console.ReadLine(), out parcelId);
            mydal.MatchDroneToParcel(mydal.ReturnParcelData(parcelId), mydal.ReturnDroneData(droneId));
        }

        public void PickUpParcel(DalObject.DalObject mydal)
        {
            int id;
            Console.WriteLine("Enter the ID of the parcel you want to pick up: ");
            int.TryParse(Console.ReadLine(), out id);
            mydal.PickUpParcel(mydal.ReturnParcelData(id));

        }

        public void DeliverParcel()
        {
            int id;
            Console.WriteLine("Enter the ID of the parcel you want to deliver: ");
            int.TryParse(Console.ReadLine(), out id);
            DalObject.DalObject.ParcelDelivered(DalObject.DalObject.ReturnParcelData(id));
        }

        public void SendDroneToStation()
        {
            PrintSpecificList(6); // print the list of available charging stations (option 6 in PrintSpecificList) so the user can choose from them
            int droneId, stationId;
            Console.WriteLine("Enter the ID of the drone you want to charge: ");
            int.TryParse(Console.ReadLine(), out droneId);
            Console.WriteLine("Enter the ID of the station you want to charge in: ");
            int.TryParse(Console.ReadLine(), out stationId);
            DalObject.DalObject.SendDroneToCharge(dal.ReturnDroneData(droneId), dal.GetStation(stationId));
        }

        public void FreeDrone()
        {
            int droneId;
            Console.WriteLine("Enter the ID of the drone you want to free: ");
            droneId = int.Parse(Console.ReadLine());
            mydal.SendDroneFromStation(mydal.ReturnDroneData(droneId));
        }
    }
}

