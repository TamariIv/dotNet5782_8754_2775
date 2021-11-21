using System;

namespace ConsoleUI_BL
{
    public class Class1
    {
        char choice;
        enum MenuOptions { Exit, Add, Update, Show_One, Show_List }
        enum EntityOptions { Exit, Parcel, Drone, BaseStation, Customer }
        enum ListOptions { Exit, BaseStations, Drones, Customers, Parcels, ParcelsWithoutDrone, AvailableChargingStation }
        enum UpdateOptions { Exit, DroneToParcel, PickedUp, Delivery, Recharge, FreeDrone }

        static IBL.IBL mybl = new BL.BlObject();

        static void Main(string[] args)
        {
            Console.WriteLine("press 1 to add an item");
            Console.WriteLine("press 2 to update an item");
            Console.WriteLine("press 3 to view details of specific item");
            Console.WriteLine("press 4 to view a list of specific item");
            Console.WriteLine("press 0 to stop");

            MenuOptions menuOptions;
            EntityOptions entityOptions;
            menuOptions = (MenuOptions)int.Parse(Console.ReadLine());

            while (menuOptions != MenuOptions.Exit)
            {
                switch (menuOptions)
                {
                    case MenuOptions.Add:
                        {
                            Console.WriteLine("Press 1 to add parcel");
                            Console.WriteLine("Press 2 to add drone");
                            Console.WriteLine("Press 3 to add station");
                            Console.WriteLine("Press 4 to add customer");
                            entityOptions = (EntityOptions)int.Parse(Console.ReadLine());
                            try
                            {
                                AddItem(entityOptions);
                            }
                            catch (string errorMsg)
                            {
                                Console.WriteLine(errorMsg);
                            }

                        }

                }
            }

        }

        private static void AddItem(EntityOptions entityOptions)
        {

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
        }

        private static void AddStation()
        {
            int id, slots;
            string name;
            Console.WriteLine("Enter station ID: ");
            int.TryParse(Console.ReadLine(), out id);
            Console.WriteLine("Enter station name: ");
            name = Console.ReadLine();
            Console.WriteLine("Enter number of charging slots: ");
            int.TryParse(Console.ReadLine(), out slots);

            IBL.BO.Station newStation = new IBL.BO.Station
            {
                Id = id,
                Name = name,
                AvailableChargeSlots = slots
            };
            mybl.AddStation(newStation);
        }
    }
}
