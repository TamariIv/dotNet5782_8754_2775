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
            Console.WriteLine("Press 1 to add an item");
            Console.WriteLine("Press 2 to update an item");
            Console.WriteLine("Press 3 to view details of specific item");
            Console.WriteLine("Press 4 to view a list of specific item");
            Console.WriteLine("Press 5 to stop");

            int choice = int.Parse(Console.ReadLine());
            while (choice != 5)
            {
                switch (choice)
                {
                    case 1:
                        {
                            //חלק של תמרי
                            //בפנים יש עוד קייס
                            break;
                        }
                    case 2:
                        {

                            //חלק של תמרי
                            //בפנים יש עוד קייס
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


    }
}
