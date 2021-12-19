using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;

namespace ConsoleUI
{
    class Program
    {
        enum MenuOptions { Exit, Add, Update, Show_One, Show_List }
        enum EntityOptions { Exit, Parcel, Drone, BaseStation, Customer }
        enum ListOptions { Exit, BaseStations, Drones, Customers, Parcels, ParcelsWithoutDrone, AvailableChargingStation }
        enum UpdateOptions { Exit, DroneToParcel, PickedUp, Delivery, Recharge, FreeDrone }

        static DalApi.IDal mydal;

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
                        #region Add switch
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
                        #endregion
                        break;

                    case MenuOptions.Update:
                        Console.WriteLine("Press 1 to match drone to parcel");
                        Console.WriteLine("Press 2 to pick up parcel");
                        Console.WriteLine("Press 3 to deliver parcel to customer");
                        Console.WriteLine("Press 4 to send drone to charge");
                        Console.WriteLine("Press 5 to free drone from charging");
                        UpdateOptions updateChoice = (UpdateOptions)Enum.Parse(typeof(UpdateOptions), Console.ReadLine());
                        #region Update switch
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
                        #endregion
                        break;

                    case MenuOptions.Show_One:
                        Console.WriteLine("press 1 to view details of a specific parcel");
                        Console.WriteLine("press 2 to view details of a specific drone");
                        Console.WriteLine("press 3 to view details of a specific base station");
                        Console.WriteLine("press 4 to view details of a specific customer");
                        entityOptions = (EntityOptions)int.Parse(Console.ReadLine());
                        int id;
                        #region get specific item switch
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
                        #endregion
                        break;

                    case MenuOptions.Show_List:
                        Console.WriteLine("press 1 to view the list of base stations");
                        Console.WriteLine("press 2 to view the list of the drones");
                        Console.WriteLine("press 3 to view the list of the cutomers");
                        Console.WriteLine("press 4 to view the list of the parcels");
                        Console.WriteLine("press 5 to view the list of the parcels without drones");
                        Console.WriteLine("press 6 to view the list of the stations with available charge slots");
                        listOptions = (ListOptions)int.Parse(Console.ReadLine());
                        #region get list switch
                        switch (listOptions)
                        {
                            case ListOptions.BaseStations:
                                IEnumerable<Station> stations = mydal.GetStations();
                                PrintBaseStationsList(stations);
                                break;
                            case ListOptions.Drones:
                                IEnumerable<Drone> drones = mydal.GetDrones();
                                PrintDronesList(drones);
                                break;
                            case ListOptions.Customers:
                                IEnumerable<Customer> customers = mydal.GetCustomers();
                                PrintCustomersList(customers);
                                break;
                            case ListOptions.Parcels:
                                IEnumerable<Parcel> parcels = mydal.GetParcels();
                                PrintParcelsList(parcels);
                                break;
                            case ListOptions.ParcelsWithoutDrone:
                                IEnumerable<Parcel> parcelsWithoutDrones = mydal.GetParcels(item => item.DroneId == 0);
                                PrintParcelsWithoutDroneList(parcelsWithoutDrones);
                                break;
                            case ListOptions.AvailableChargingStation:
                                IEnumerable<Station> s = mydal.GetStations(item => item.AvailableChargeSlots != 0);
                                PrintAvailableChargeSlotsList(s);
                                break;
                            case ListOptions.Exit:
                                break;
                            default:
                                break;
                        }
                        #endregion
                        break;
                    case MenuOptions.Exit:
                        break;
                    default:
                        break;
                }
                Console.WriteLine("press 1 to add an item");
                Console.WriteLine("press 2 to update an item");
                Console.WriteLine("press 3 to view details of specific item");
                Console.WriteLine("press 4 to view a list of specific item");
                Console.WriteLine("press 0 to stop");
                menuOptions = (MenuOptions)int.Parse(Console.ReadLine());
            }
        }


        #region print list functions
        /// <summary>
        /// the function prints each item in the list stations
        /// </summary>
        /// <param name="baseStations"> list of stations </param>
        public static void PrintBaseStationsList(IEnumerable<Station> baseStations)
        {
            foreach (var item in baseStations)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// the function prints each item in the list drones
        /// </summary>
        /// <param name="drones"> the list of drones </param>
        public static void PrintDronesList(IEnumerable<Drone> drones)
        {
            foreach (var item in drones)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// the function prints each item in the list parcels
        /// </summary>
        /// <param name="parcels"> the list of parcels </param>
        public static void PrintParcelsList(IEnumerable<Parcel> parcels)
        {
            foreach (var item in parcels)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// the function prints each item in the list customers
        /// </summary>
        /// <param name="customers"> the list of customers </param>
        public static void PrintCustomersList(IEnumerable<Customer> customers)
        {
            foreach (var item in customers)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// the function prints the list of drones that are being charged
        /// </summary>
        /// <param name="droneCharges"></param>
        public static void PrintAvailableChargeSlotsList(IEnumerable<Station> droneCharges)
        {
            foreach (var item in droneCharges)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// the function prints each item in the list of parcels that don't have a drone assighned to them
        /// </summary>
        /// <param name="parcelsWithoutDrone"> the list of parcels without drone </param>
        public static void PrintParcelsWithoutDroneList(IEnumerable<Parcel> parcelsWithoutDrone)
        {
            foreach (var item in parcelsWithoutDrone)
            {
                Console.WriteLine(item);
            }
        }

        #endregion

        #region addition functions
        /// <summary>
        /// get from user the senderId, targetId, weight of the parcel, and priority 
        /// create a new parcel with the data and send it to AddParcel in DalObject
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
                Requested = DateTime.Today,
                Scheduled = null,
                PickedUp = null,
                Delivered = null
            };
            mydal.AddParcel(parcel);
        }

        /// <summary>
        /// get from user the drone id, model, and maximum weight 
        /// create a new drone with the data and send it to AddDrone in DalObject
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
                MaxWeight = maxWeight
            };
            mydal.AddDrone(drone);
        }

        /// <summary>
        /// get from user the station id and name, the location (longitude, latitude), and the number of empty charging slots  
        /// create a new station with the data and send it to AddStation in DalObject
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
            Console.WriteLine("Enter number of charging slots: ");
            int.TryParse(Console.ReadLine(), out slots);
            Station newStation = new Station
            {
                Id = id,
                Name = name,
                Longitude = longitude,
                Latitude = latitude,
                AvailableChargeSlots = slots
            };
            mydal.AddStation(newStation);
        }

        /// <summary>
        /// get from user the customer id, name, ohone number, and location (longitude, latitude)
        /// create a new customer with the data and send it to AddCustomer in DalObject
        /// </summary>
        public static void AddCustomer()
        {
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
        #endregion

        #region updating functions

        /// <summary>
        /// the function prints to the user the list of available drones and the list of parcels without drone and recieves and
        /// assigns the chosen parcel to the chosen drone
        /// </summary>
        public static void DroneToParcel()
        {
            int droneId, parcelId;
            //IEnumerable<Drone> temp = mydal.GetDrones();
            //foreach (var item in temp) { if (item.Status == DroneStatus.Available) Console.WriteLine(item); } //print all the available drones
            Console.WriteLine("Enter the ID of the drone you want to send: ");
            int.TryParse(Console.ReadLine(), out droneId);

            Console.WriteLine("Enter ID of the parcel you want to send: ");
            int.TryParse(Console.ReadLine(), out parcelId);
            mydal.MatchDroneToParcel(mydal.GetParcel(parcelId), mydal.GetDrone(droneId));
        }

        /// <summary>
        /// the function prints the list if parcels to the user and asks to choose a parcel to be picked up by a drone
        /// </summary>
        public static void PickUpParcel()
        {
            int id;
            PrintParcelsList(mydal.GetParcels()); //print all the parcels for the user
            Console.WriteLine("Enter the ID of the parcel you want to pick up: ");
            int.TryParse(Console.ReadLine(), out id);
            mydal.PickUpParcel(mydal.GetParcel(id));
        }

        /// <summary>
        /// the function prints the list if parcels to the user and asks to choose a parcel to deliver
        /// </summary>
        public static void DeliverParcel()
        {
            int id;
            Console.WriteLine("Enter the ID of the parcel you want to deliver: ");
            PrintParcelsList(mydal.GetParcels()); //print the list of parcels for the user
            int.TryParse(Console.ReadLine(), out id);
            mydal.ParcelDelivered(mydal.GetParcel(id));
        }

        /// <summary>
        /// the function prints all the drones that are not being chraged at the moment and asks the user to 
        /// choose a drone to send to charge
        /// </summary>
        public static void SendDroneToStation()
        {
            int droneId, stationId;
            //List<Drone> temp = mydal.GetDrones();
            //foreach (var item in temp) { if (item.Status != DroneStatus.Maintenance) Console.WriteLine(item); } //print all the drones that not in charge
            Console.WriteLine("Enter the ID of the drone you want to charge: ");
            int.TryParse(Console.ReadLine(), out droneId);

            //PrintBaseStationsList(mydal.GetStations());
            Console.WriteLine("Enter the ID of the station you want to charge in: ");
            int.TryParse(Console.ReadLine(), out stationId);
            mydal.SendDroneToCharge(mydal.GetDrone(droneId), mydal.GetStation(stationId));
        }

        /// <summary>
        /// the function prints all the drones that are being charged at the moment and asks the user 
        /// to choose a drone to free from charge
        /// </summary>
        public static void FreeDrone()
        {
            int droneId;
            //IEnumerable<Drone> temp = mydal.GetDrones();
            //foreach (var item in temp) { if (item.Status == DroneStatus.Maintenance) Console.WriteLine(item); } //print all the drones that in charge
            Console.WriteLine("Enter the ID of the drone you want to free: ");
            droneId = int.Parse(Console.ReadLine());
            mydal.SendDroneFromStation(mydal.GetDrone(droneId));
        }
        #endregion
    }
}

