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
        enum MenuOptions {  Add = 1, Update, Show_One, Show_List }
        enum EntityOptions { Parcel = 1, Drone, BaseStation, Customer }
        enum ListOptions {  BaseStations = 1, Drones, Customers, Parcels, ParcelsWithoutDrone, AvailableChargingStation }
        enum UpdateOptions {  DroneToParcel = 1, PickedUp, Delivery, Recharge, FreeDrone }
       
        static void Main(string[] args)
        {
            DalObject.DalObject d = new DalObject.DalObject();

            Console.WriteLine("Press 1 to add an item");
            Console.WriteLine("Press 2 to update an item");
            Console.WriteLine("Press 3 to view details of specific item");
            Console.WriteLine("Press 4 to view a list of specific item");
            Console.WriteLine("Press 5 to stop");
            int choice;
            int.TryParse(Console.ReadLine(), out choice);
            while (choice != 5)
            {
                switch (choice)
                {
                    case (int)MenuOptions.Add:
                        Console.WriteLine("Press 1 to add parcel");
                        Console.WriteLine("Press 2 to add drone");
                        Console.WriteLine("Press 3 to add station");
                        Console.WriteLine("Press 4 to add customer");
                        int innerChoice;
                        int.TryParse(Console.ReadLine(), out innerChoice);
                        switch (innerChoice)
                        {
                            case (int)EntityOptions.Parcel:
                                ReceiveParcel();
                                break;
                            case (int)EntityOptions.Drone:
                                ReceiveDrone();
                                break;
                            case (int)EntityOptions.BaseStation:
                                ReceiveStation();
                                break;

                            case (int)EntityOptions.Customer:
                                ReceiveCustomer();
                                break;
                            default:
                                break;
                        }
                        break;

                    case (int)MenuOptions.Update:
                        Console.WriteLine("Press 1 to match drone to parcel");
                        Console.WriteLine("Press 2 to pick up parcel");
                        Console.WriteLine("Press 3 to deliver parcel to customer");
                        Console.WriteLine("Press 4 to send drone to charge");
                        Console.WriteLine("Press 5 to free drone from charging");
                        int.TryParse(Console.ReadLine(), out innerChoice);
                        switch (innerChoice)
                        {
                            case (int)UpdateOptions.DroneToParcel:
                                {
                                    DroneToParcel();
                                    break;
                                }
                            case (int)UpdateOptions.PickedUp:
                                {
                                    PickUpParcel();
                                    break;
                                }
                            case (int)UpdateOptions.Delivery:
                                {
                                    DeliverParcel();
                                    break;
                                }
                            case (int)UpdateOptions.Recharge:
                                {
                                    SendDroneToStation();
                                    break;
                                }
                            case (int)UpdateOptions.FreeDrone:
                                {
                                    FreeDrone();
                                    break;
                                }
                            default:
                                break;
                        }
                        break;

                    case (int)MenuOptions.Show_One:
                        Console.WriteLine("press 1 to view details of a specific parcel");
                        Console.WriteLine("press 2 to view details of a specific drone");
                        Console.WriteLine("press 3 to view details of a specific base station");
                        Console.WriteLine("press 4 to view details of a specific customer");
                        int option = int.Parse(Console.ReadLine());
                        PrintSpecificItem(option);
                        break;
                    case (int)MenuOptions.Show_List:
                            Console.WriteLine("press 1 to view the list of base stations");
                            Console.WriteLine("press 2 to view the list of the drones");
                            Console.WriteLine("press 3 to view the list of the cutomers");
                            Console.WriteLine("press 4 to view the list of the parcels");
                            Console.WriteLine("press 5 to view the list of the parcels without drones");
                            Console.WriteLine("press 6 to view the list of the stations with available charge slots");
                            int.TryParse(Console.ReadLine(), out option);
                            PrintSpecificList(option);
                            break;
                    default:
                        break;
                }
                int.TryParse(Console.ReadLine(), out choice);
            }
        }

        public static void ReceiveParcel()
        {
            int senderId, targetId;
            WeightCategories weight;
            Priorities priority;
            Console.WriteLine("Enter sender ID: ");
            int.TryParse(Console.ReadLine(), out senderId);
            Console.WriteLine("Enter target ID: ");
            int.TryParse(Console.ReadLine(), out targetId);
            Console.WriteLine("Weight of the parcel: press 1 for heavy, 2 for medium and 3 for light: ");
            int tmp;
            int.TryParse(Console.ReadLine(), out tmp);
            weight = (WeightCategories)(tmp - 1);
            Console.WriteLine("Priorities of the parcel: press 1 for regular, 2 for rapid and 3 for emergency: ");
            priority = (Priorities)(int.Parse(Console.ReadLine()) - 1);
            DalObject.DalObject.NewParcel(senderId, targetId, weight, priority);
        }

        public static void ReceiveDrone()
        {
            int id;
            string model;
            WeightCategories maxWeight;
            //DroneStatus status;
            Console.WriteLine("Enter drone ID: ");
            int.TryParse(Console.ReadLine(), out id);
            Console.WriteLine("Enter drone model: ");
            model = Console.ReadLine();
            Console.WriteLine("Maximum weight of the parcel: press 1 for heavy, 2 for medium and 3 for light: ");
            int tmp;
            int.TryParse(Console.ReadLine(), out tmp);
            maxWeight = (WeightCategories)(tmp - 1);
            DalObject.DalObject.AddDrone(id, model, maxWeight);
        }

        public static void ReceiveStation()
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
            Console.WriteLine("Enter number of open charging slots: ");
            int.TryParse(Console.ReadLine(), out slots);
            DalObject.DalObject.AddStation(id, name, longitude, latitude, slots);
        }

        public static void ReceiveCustomer()
        {
            // continue to change to tryparse from here
            int id;
            string name, phone;
            double longitude, latitude;
            Console.WriteLine("Enter customer ID: ");
            id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter customer name: ");
            name = Console.ReadLine();
            Console.WriteLine("Enter phone number: ");
            phone = Console.ReadLine();
            Console.WriteLine("Enter customer longitude: ");
            longitude = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter customer latitude: ");
            latitude = double.Parse(Console.ReadLine());
            DalObject.DalObject.NewCustomer(id, name, phone, longitude, latitude);
        }

        public static void PrintSpecificItem(int option)
        {         
            int id;
            switch (option)
            {
                case (int)EntityOptions.Parcel:
                    {
                        Console.WriteLine("enter ID of the parcel");
                        id = int.Parse(Console.ReadLine());
                        Parcel p = DalObject.DalObject.ReturnParcelData(id);
                        Console.WriteLine(p);
                        break;
                    }
                case (int)EntityOptions.Drone:
                    {
                        Console.WriteLine("enter ID of the drone");
                        id = int.Parse(Console.ReadLine());
                        Drone d = DalObject.DalObject.ReturnDroneData(id);
                        Console.WriteLine(d);
                        break;
                    }
                case (int)EntityOptions.BaseStation:
                    {
                        Console.WriteLine("enter ID of the station");
                        id = int.Parse(Console.ReadLine());
                        Station s = DalObject.DalObject.ReturnStationData(id);
                        Console.WriteLine(s);
                        break;
                    }
                case (int)EntityOptions.Customer:
                    {
                        Console.WriteLine("enter ID of the customer");
                        id = int.Parse(Console.ReadLine());
                        Customer c = DalObject.DalObject.ReturnCustomerData(id);
                        Console.WriteLine(c);
                        break;
                    }             
                default:
                    break;

            }
        }
        public static void PrintSpecificList(int option)
        {
           
            switch (option)
            {
                case (int)ListOptions.BaseStations:
                        List<Station> stations = DalObject.DalObject.GetStations();
                        foreach (var item in stations)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                case (int)ListOptions.Drones:
                        List<Drone> drones = DalObject.DalObject.GetDrones();
                        foreach (var item in drones)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                case (int)ListOptions.Customers:
                        List<Customer> customers = DalObject.DalObject.GetCustomers();
                        foreach (var item in customers)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                case (int)ListOptions.Parcels:
                        List<Parcel> parcels = DalObject.DalObject.GetParcels();
                        foreach (var item in parcels)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                case (int)ListOptions.ParcelsWithoutDrone:
                        List<Parcel> parcelsWithoutDrones = DalObject.DalObject.ParcelWithoutDrone();
                        foreach (var item in parcelsWithoutDrones)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                case (int)ListOptions.AvailableChargingStation:
                        List<Station> availableChargers = DalObject.DalObject.AvailableCharger();
                        foreach (var item in availableChargers)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                default:
                    break;
            }
        }

        public static void DroneToParcel()
        {
            int droneId, parcelId;
            Console.WriteLine("Enter the ID of the drone you want to send: ");
            droneId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter ID of the parcel you want to send: ");
            parcelId = int.Parse(Console.ReadLine());
            DalObject.DalObject.MatchDroneToParcel(DalObject.DalObject.ReturnParcelData(parcelId), DalObject.DalObject.ReturnDroneData(droneId));
        }

        public static void PickUpParcel()
        {
            PrintSpecificList(5);
            int id;
            Console.WriteLine("Enter the ID of the parcel you want to pick up: ");
            int.TryParse(Console.ReadLine(), out id);
            DalObject.DalObject.PickUpParcel(DalObject.DalObject.ReturnParcelData(id));
        }

        public static void DeliverParcel()
        {
            int id;
            Console.WriteLine("Enter the ID of the parcel you want to deliver: ");
            int.TryParse(Console.ReadLine(), out id);
            DalObject.DalObject.ParcelDelivered(DalObject.DalObject.ReturnParcelData(id));
        }

        public static void SendDroneToStation()
        {
            PrintSpecificList(6);
            int droneId, stationId;
            Console.WriteLine("Enter the ID of the drone you want to charge: ");
            droneId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the ID of the station you want to charge in: ");
            stationId = int.Parse(Console.ReadLine());
            DalObject.DalObject.SendDroneToCharge(DalObject.DalObject.ReturnDroneData(droneId), DalObject.DalObject.ReturnStationData(stationId));
        }

        public static void FreeDrone()
        {
            int droneId;
            Console.WriteLine("Enter the ID of the drone you want to free: ");
            droneId = int.Parse(Console.ReadLine());
            DalObject.DalObject.SendDroneFromStation(DalObject.DalObject.ReturnDroneData(droneId));
        }
    }
}

