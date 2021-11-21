using System;

namespace ConsoleUI_BL
{
    public class Program
    {
        enum MenuOptions { Exit, Add, Update, Show_One, Show_List }
        enum EntityOptions { Exit, Parcel, Drone, BaseStation, Customer }
        enum ListOptions { Exit, BaseStations, Drones, Customers, Parcels, ParcelsWithoutDrone, AvailableChargingStation }
        enum UpdateOptions { Exit, DroneToParcel, PickedUp, Delivery, Recharge, FreeDrone }

        public static IBL.IBL mybl = new BL.BlObject();

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
                            switch (entityOptions)
                            {
                                case EntityOptions.BaseStation:
                                    try
                                    {
                                        AddItem(entityOptions);
                                    }
                                    catch (Exception exp)
                                    {
                                        Console.WriteLine(exp.Message);
                                    }
                                    break;
                                case EntityOptions.Drone:
                                    try
                                    {

                                    }
                                    catch
                                    {

                                    }
                                    break;
                                case EntityOptions.Customer:
                                    try
                                    {
                                        AddItem(entityOptions);
                                    }
                                    catch
                                    {

                                    }
                                    break;
                            }

                            break;
                        }

                }
            }

        }

        private static void AddItem(EntityOptions entityOptions)
        {

            try
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
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }

        }

        private static void AddDrone()
        {
            throw new NotImplementedException();
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
                AvailableChargeSlots = slots,
            };
            mybl.AddStation(newStation);
        }
        private static void AddCustomer()
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

            IBL.BO.Location location = new IBL.BO.Location
            {
                Longitude = longitude,
                Latitude = latitude
            };

            IBL.BO.Customer newCustomer = new IBL.BO.Customer
            {
                Id = id,
                Name = name,
                Phone = phone,
                Location = location
            };
            mybl.AddCustomer(newCustomer);
        }
        private static void addParcel()
        {
            int senderId, targetId;
            Console.WriteLine("Enter sender ID: ");
            int.TryParse(Console.ReadLine(), out senderId);
            Console.WriteLine("Enter target ID: ");
            int.TryParse(Console.ReadLine(), out targetId);
            Console.WriteLine("Weight of the parcel: press 1 for heavy, 2 for medium and 3 for light: ");
            IBL.BO.Enums.WeightCategories weight = (IBL.BO.Enums.WeightCategories)Enum.Parse(typeof(IBL.BO.Enums.WeightCategories), Console.ReadLine());
            Console.WriteLine("Priorities of the parcel: press 1 for regular, 2 for rapid and 3 for emergency: ");
            IBL.BO.Enums.Priorities priority = (IBL.BO.Enums.Priorities)Enum.Parse(typeof(IBL.BO.Enums.Priorities), Console.ReadLine());

            IBL.BO.CustomerInParcel sender = new IBL.BO.CustomerInParcel
            {
                Id = senderId
            };
            IBL.BO.CustomerInParcel target = new IBL.BO.CustomerInParcel
            {
                Id = targetId
            };
            IBL.BO.ParcelInDelivey parcel = new IBL.BO.ParcelInDelivey
            {
                Sender = sender,
                Target = target,
                Weight = weight,
                Priority = priority,
            };
            mybl.AddParcel(parcel);
        }

    }
}

