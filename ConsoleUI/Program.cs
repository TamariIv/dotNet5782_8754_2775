using System;
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
            switch (choice)
            {
                case 1:
                    {
                        Console.WriteLine("Press 1 to add parcel");
                        Console.WriteLine("Press 2 to add drone");
                        Console.WriteLine("Press 3 to add station");
                        Console.WriteLine("Press 4 to add parcel");
                        int Innerhoice = int.Parse(Console.ReadLine());
                        switch (Innerhoice)
                        {
                            case 1:
                                {
                                    ReceiveParcel();
                                }


                        }
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

                        break;
                    }
                case 5:
                    {
                        break;
                    }

                default:
                    break;
            }


            void ReceiveParcel()
            {
                int senderId, targetId;
                WeightCategories weight;
                Priorities priority;
                Console.WriteLine("Enter sender ID: ");
                senderId = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter target ID: ");
                targetId = int.Parse(Console.ReadLine());
                Console.WriteLine("Weight of the parcel: press 1 for heavy, 2 for medium and 3 for light: ");
                weight = (WeightCategories)(int.Parse(Console.ReadLine()) + 1);
                Console.WriteLine("Priorities of the parcel: press 1 for regular, 2 for rapid and 3 for emergency: ");
                priority = (Priorities)(int.Parse(Console.ReadLine()) + 1);
                DalObject.NewParcel(senderId, targetId, weight, priority);
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
            //Console.WriteLine("press 1 to view details of a specific station");
            //Console.WriteLine("press 2 to view details of a specific drone");
            //Console.WriteLine("press 3 to view details of a specific customer");
            //Console.WriteLine("press 4 to view details of a specific parcel");
        }


    }
}
