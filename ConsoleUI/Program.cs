using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;
using DAL.DalObject;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            DalObject d = new DalObject();

            Console.WriteLine("Press 1 to add an item");
            Console.WriteLine("Press 2 to update an item");
            Console.WriteLine("Press 3 to view details of specific item");
            Console.WriteLine("Press 4 to view a list of specific item");
            Console.WriteLine("Press 5 to stop");

            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    {
                        Console.WriteLine("Press 1 to add parcel");
                        Console.WriteLine("Press 2 to add drone");
                        Console.WriteLine("Press 3 to add station");
                        Console.WriteLine("Press 4 to add customer");
                        int innerChoice = int.Parse(Console.ReadLine());
                        switch (innerChoice)
                        {
                            case 1:
                                {
                                    ReceiveParcel();
                                    break;
                                }
                            case 2:
                                {
                                    ReceiveDrone();
                                    break;
                                }
                            case 3:
                                {
                                    ReceiveStation();
                                    break;
                                }
                            case 4:
                                {
                                    ReceiveCustomer();
                                    break;
                                }
                                
                        }
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("Press 1 to match drone to parcel");
                        Console.WriteLine("Press 2 to pick up parcel");
                        Console.WriteLine("Press 3 to deliver parcel to customer");
                        Console.WriteLine("Press 4 to send drone to charge");
                        Console.WriteLine("Press 5 to free drone from charging");
                        int Innerhoice = int.Parse(Console.ReadLine());
                        switch (Innerhoice)
                        {
                            case 1:
                                {
                                    DroneToParcel();
                                    break;
                                }
                            case 2:
                                {
                                    PickUpParcel();
                                    break;
                                }
                            case 3:
                                {
                                    DeliverParcel();
                                    break;
                                }
                            case 4:
                                {
                                    SendDroneToStation();
                                    break;
                                }
                            case 5:
                                {
                                    FreeDrone();
                                    break;
                                }
                        }


                        break;
                    }
                case 3:
                    {
                        PrintSpecificItem();
                        break;
                    }
                case 4:
                    {
                        PrintSpecificList();
                        break;
                    }
                case 5:
                    {
                        //exit
                        break;
                    }

                default:
                    break;
            }
        }

        static void ReceiveParcel()
        {
            int senderId, targetId;
            WeightCategories weight;
            Priorities priority;
            Console.WriteLine("Enter sender ID: ");
            senderId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter target ID: ");
            targetId = int.Parse(Console.ReadLine());
            Console.WriteLine("Weight of the parcel: press 1 for heavy, 2 for medium and 3 for light: ");
            weight = (WeightCategories)(int.Parse(Console.ReadLine()) - 1);
            Console.WriteLine("Priorities of the parcel: press 1 for regular, 2 for rapid and 3 for emergency: ");
            priority = (Priorities)(int.Parse(Console.ReadLine()) - 1);
            DalObject.NewParcel(senderId, targetId, weight, priority);
        }

        static void ReceiveDrone()
        {
            int id;
            string model;
            WeightCategories maxWeight;
            //DroneStatus status;
            Console.WriteLine("Enter drone ID: ");
            id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter drone model: ");
            model = Console.ReadLine();
            Console.WriteLine("Maximum weight of the parcel: press 1 for heavy, 2 for medium and 3 for light: ");
            maxWeight = (WeightCategories)(int.Parse(Console.ReadLine()) - 1);
            DalObject.AddDrone(id, model, maxWeight);
        }

        static void ReceiveStation()
        {
            int id, slots;
            string name;
            double longitude, latitude;
            Console.WriteLine("Enter station ID: ");
            id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter station name: ");
            name = Console.ReadLine();
            Console.WriteLine("Enter station longitude: ");
            longitude = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter station latitude: ");
            latitude = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter number of open charging slots: ");
            slots = int.Parse(Console.ReadLine());
            DalObject.AddStation(id, name, longitude, latitude, slots);
        }

        static void ReceiveCustomer()
        {
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
            DalObject.NewCustomer(id, name, phone, longitude, latitude);
        }

        public static void PrintSpecificItem()
        {
            Console.WriteLine("press 1 to view details of a specific station");
            Console.WriteLine("press 2 to view details of a specific drone");
            Console.WriteLine("press 3 to view details of a specific customer");
            Console.WriteLine("press 4 to view details of a specific parcel");

            int option = int.Parse(Console.ReadLine());
            int id;
            switch (option)
            {
                case 1:
                    {
                        Console.WriteLine("enter ID of the station");
                        id = int.Parse(Console.ReadLine());
                        Station s = DalObject.ReturnStationData(id);
                        s.ToString();
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("enter ID of the drone");
                        id = int.Parse(Console.ReadLine());
                        Drone d = DalObject.ReturnDroneData(id);
                        d.ToString();
                        break;
                    }
                case 3:
                    {
                        Console.WriteLine("enter ID of the customer");
                        id = int.Parse(Console.ReadLine());
                        Customer c = DalObject.ReturnCustomerData(id);
                        c.ToString();
                        break;
                    }
                case 4:
                    {
                        Console.WriteLine("enter ID of the parcel");
                        id = int.Parse(Console.ReadLine());
                        Parcel p = DalObject.ReturnParcelData(id);
                        p.ToString();
                        break;
                    }
                default:
                    break;

            }
        }
        public static void PrintSpecificList()
        {
            Console.WriteLine("press 1 to view the list of base stations");
            Console.WriteLine("press 2 to view the list of the drones");
            Console.WriteLine("press 3 to view the list of the cutomers");
            Console.WriteLine("press 4 to view the list of the parcels");
            Console.WriteLine("press 5 to view the list of the parcels without drones");
            Console.WriteLine("press 6 to view the list of the stations with available charge slots");

            int option = int.Parse(Console.ReadLine());
            switch (option)
            {
                case 1:
                    {
                        List<Station> stations = DalObject.GetStations();
                        foreach (var item in stations)
                        {
                            item.ToString();
                        }
                        break;
                    }
                case 2:
                    {
                        List<Drone> drones = DalObject.GetDrones();
                        foreach (var item in drones)
                        {
                            item.ToString();
                        }
                        break;
                    }
                case 3:
                    {
                        List<Customer> customers = DalObject.GetCustomers();
                        foreach (var item in customers)
                        {
                            item.ToString();
                        }
                        break;
                    }
                case 4:
                    {
                        List<Parcel> parcels = DalObject.GetParcels();
                        foreach (var item in parcels)
                        {
                            item.ToString();
                        }
                        break;
                    }
                case 5:
                    {
                        List<Parcel> parcelsWithoutDrones = DalObject.ParcelWithoutDrone();
                        foreach (var item in parcelsWithoutDrones)
                        {
                            item.ToString();
                        }
                        break;
                    }
                case 6:
                    {
                        List<Station> availableChargers = DalObject.AvailableCharger();
                        foreach (var item in availableChargers)
                        {
                            item.ToString();
                        }
                        break;
                    }
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
            DalObject.MatchDroneToParcel(DalObject.ReturnParcelData(parcelId), DalObject.ReturnDroneData(droneId));
        }

        public static void PickUpParcel()
        {
            int id;
            Console.WriteLine("Enter the ID of the parcel you want to pick up: ");
            id = int.Parse(Console.ReadLine());
            DalObject.PickUpParcel(DalObject.ReturnParcelData(id));
        }

        public static void DeliverParcel()
        {
            int id;
            Console.WriteLine("Enter the ID of the parcel you want to deliver: ");
            id = int.Parse(Console.ReadLine());
            DalObject.ParcelDelivered(DalObject.ReturnParcelData(id));
        }

        public static void SendDroneToStation()
        {
             DalObject.AvailableCharger()
            int droneId, stationId;
            Console.WriteLine("Enter the ID of the drone you want to charge: ");
            droneId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the ID of the station you want to charge in: ");
            stationId = int.Parse(Console.ReadLine());
            DalObject.SendDroneToCharge(DalObject.ReturnDroneData(droneId), DalObject.ReturnStationData(stationId));
        }

        public static void FreeDrone()
        {
            int droneId;
            Console.WriteLine("Enter the ID of the drone you want to free: ");
            droneId = int.Parse(Console.ReadLine());
            DalObject.SendDroneFromStation(DalObject.ReturnDroneData(droneId));
        }
    }
}
