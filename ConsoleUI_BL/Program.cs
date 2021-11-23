using System;

namespace ConsoleUI_BL
{
    public class Program
    {
        enum MenuOptions { Exit, Add, Update, Show_One, Show_List }
        enum EntityOptions { Exit, Parcel, Drone, BaseStation, Customer }
        enum ListOptions { Exit, BaseStations, Drones, Customers, Parcels, ParcelsWithoutDrone, AvailableChargingStation }
        enum UpdateOptions { Exit, Drone, Station, Customer, Recharge, FreeDrone, DroneToParcel }

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
                                case EntityOptions.Parcel:
                                    try
                                    {
                                        AddParcel();
                                    }
                                    catch (Exception exp)
                                    {
                                        Console.WriteLine(exp.Message);
                                    }
                                    break;
                                case EntityOptions.Drone:
                                    try
                                    {
                                        AddDrone();
                                    }
                                    catch
                                    {

                                    }
                                    break;
                                case EntityOptions.BaseStation:
                                    try
                                    {
                                        AddStation();
                                    }
                                    catch
                                    {

                                    }
                                    break;
                                case EntityOptions.Customer:
                                    try
                                    {
                                        AddCustomer();
                                    }
                                    catch
                                    {

                                    }
                                    break;           
                            }

                            break;
                        }
                    case MenuOptions.Update:
                        Console.WriteLine("Press 1 to update a drone");
                        Console.WriteLine("Press 2 to update a station");
                        Console.WriteLine("Press 3 to update a customer");
                        Console.WriteLine("Press 4 to send drone to charge");
                        Console.WriteLine("Press 5 to free drone from charging");
                        UpdateOptions updateChoice = (UpdateOptions)Enum.Parse(typeof(UpdateOptions), Console.ReadLine());
                        #region Update switch
                        switch (updateChoice)
                        {
                            case UpdateOptions.Drone:
                                try
                                {
                                    UpdateDrone();
                                }
                                catch
                                {

                                }
                                break;
                            case UpdateOptions.Station:
                                try
                                {
                                    UpdateStation();
                                }
                                catch
                                {

                                }
                                break;
                            case UpdateOptions.Customer:
                                try
                                {
                                    UpdateCustomer();
                                }
                                catch
                                {

                                }
                                break;
                            case UpdateOptions.Recharge:
                                try
                                {
                                    RechargeDrone();
                                }
                                catch
                                {

                                }
                                break;
                            case UpdateOptions.FreeDrone:
                                try
                                {
                                    FreeDrone();
                                }
                                catch
                                {

                                }
                                break;
                            case UpdateOptions.DroneToParcel:
                                try
                                {
                                    DroneToParcel();
                                }
                                catch
                                {

                                }
                                break;
                            case UpdateOptions.Exit:
                                break;
                            default:
                                break;
                        }
                        #endregion
                        break;
                }
            }

        }

        private static void DroneToParcel()
        {
            int id;
            Console.WriteLine("Enter drone ID: ");
            if (!int.TryParse(Console.ReadLine(), out id))
                throw new WrongInputFormatException("int was expected\n");
            mybl.DroneToParcel(id);
        }

        private static void FreeDrone()
        {
            int id;
            double timeInCharging;
            Console.WriteLine("Enter drone ID: ");
            if (!int.TryParse(Console.ReadLine(), out id))
                throw new WrongInputFormatException("int was expected\n");
            Console.WriteLine("Enter drone time in charging: ");
            if (!double.TryParse(Console.ReadLine(), out timeInCharging))
                throw new WrongInputFormatException("double was expected\n");
            mybl.FreeDrone(id, timeInCharging);

        }

        private static void UpdateStation()
        {
            int id, chargeSlots;
            string name;
            Console.WriteLine("Enter station ID: ");
            if (!int.TryParse(Console.ReadLine(), out id))
                throw new WrongInputFormatException("input was not int\n");
            Console.WriteLine("Enter customer name: ");
            name = Console.ReadLine();
            Console.WriteLine("Enter station number of charge slots: ");
            if (!int.TryParse(Console.ReadLine(), out chargeSlots))
                throw new WrongInputFormatException("input was not int\n");
            IBL.BO.Station newStation = new IBL.BO.Station
            {
                Id = id,
                Name = name,
                AvailableChargeSlots = chargeSlots
            };
            mybl.UpdateStation(newStation);
        }

        private static void UpdateCustomer()
        {
            int id;
            string name, phone;
            Console.WriteLine("Enter customer ID: ");
            if (!int.TryParse(Console.ReadLine(), out id))
                throw new WrongInputFormatException("input was not int\n");
            Console.WriteLine("Enter customer name: ");
            name = Console.ReadLine();
            Console.WriteLine("Enter phone number: ");
            phone = Console.ReadLine();

            IBL.BO.Customer newCustomer = new IBL.BO.Customer
            {
                Id = id,
                Name = name,
                Phone = phone,
            };
            mybl.UpdateCustomer(newCustomer);
        }

        private static void AddDrone()
        {
            int id, station;
            string model;
            IBL.BO.Enums.WeightCategories weight;
            Console.WriteLine("Enter drone ID: ");
            if (!int.TryParse(Console.ReadLine(), out id))
                throw new WrongInputFormatException("int was expected\n");
            Console.WriteLine("Enter drone model: ");
            model = Console.ReadLine();
            Console.WriteLine("Maximum weight of the parcel: press 1 for heavy, 2 for medium and 3 for light: ");
            int tmp;
            int.TryParse(Console.ReadLine(), out tmp);
            weight = (IBL.BO.Enums.WeightCategories)(tmp - 1);
            Console.WriteLine("Enter station to charge in ID: ");
            int.TryParse(Console.ReadLine(), out station);

            IBL.BO.Drone newDrone = new IBL.BO.Drone
            {
                Id = id,
                Model = model,
                MaxWeight = weight
            };
            mybl.AddDrone(newDrone, station);
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

        private static void AddParcel()
        {
            int senderId, targetId;
            Console.WriteLine("Enter sender ID: ");
            if (!int.TryParse(Console.ReadLine(), out senderId))
                throw new WrongInputFormatException("input was not int");
            Console.WriteLine("Enter target ID: ");
            if (!int.TryParse(Console.ReadLine(), out targetId))
                throw new WrongInputFormatException("input was not int");
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

        //-----------------Update-----------------//
       
        private static void UpdateDrone()
        {
            int id;
            string model;
            Console.WriteLine("Enter ID of the drone");
            int.TryParse(Console.ReadLine(), out id);
            Console.WriteLine("Enter model of the drone");
            model = Console.ReadLine();
            IBL.BO.Drone newDrone = new IBL.BO.Drone()
            {
                Id = id,
                Model = model
            };
            mybl.UpdateDrone(newDrone);            
        }
    }
}

