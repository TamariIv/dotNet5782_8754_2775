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
        enum MenuOptions { Exit, Add, Update, Show_One, Show_List }
        enum EntityOptions { Exit, Parcel, Drone, BaseStation, Customer }
        enum ListOptions { Exit, BaseStations, Drones, Customers, Parcels, ParcelsWithoutDrone, AvailableChargingStation }
        enum UpdateOptions { Exit, DroneToParcel, PickedUp, Delivery, Recharge, FreeDrone }

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
                                IEnumerable<Parcel> parcelsWithoutDrones = mydal.GetParcelWithoutDrone();
                                PrintParcelsWithoutDroneList(parcelsWithoutDrones);
                                break;
                            case ListOptions.AvailableChargingStation:
                                IEnumerable<Station> availableChargers = mydal.AvailableCharger();
                                PrintAvailableChargeSlotsList(availableChargers);
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
                Scheduled = DateTime.Today,
                Requested = DateTime.Today,
                PickedUp = DateTime.Today,
                Delivered = DateTime.Today
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
        /// create a new customer with the data and send it to AddCustomer in DalObject
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

            PrintParcelsList(mydal.GetParcelWithoutDrone()); //print the list of parcels without drones for the user
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



/* EXAMPLE
press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
1
Press 1 to add parcel
Press 2 to add drone
Press 3 to add station
Press 4 to add customer
1
Enter sender ID:
123456789
Enter target ID:
987654321
Weight of the parcel: press 1 for heavy, 2 for medium and 3 for light:
2
Priorities of the parcel: press 1 for regular, 2 for rapid and 3 for emergency:
2
press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
1
Press 1 to add parcel
Press 2 to add drone
Press 3 to add station
Press 4 to add customer
1
Enter sender ID:
543216789
Enter target ID:
123459876
Weight of the parcel: press 1 for heavy, 2 for medium and 3 for light:
1
Priorities of the parcel: press 1 for regular, 2 for rapid and 3 for emergency:
3
press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
4
press 1 to view the list of base stations
press 2 to view the list of the drones
press 3 to view the list of the cutomers
press 4 to view the list of the parcels
press 5 to view the list of the parcels without drones
press 6 to view the list of the stations with available charge slots
4
Id is: 1000000
Id of the sender: 613499860
Id of the target: 909150364
Weight is: Medium
priority: Rapid
requested date: 23/10/2021
Drone id: 0
scheduled date: 23/10/2021 07:00:00
pickedUp date: 23/10/2021 08:53:00
delivered date: 23/10/2021 10:18:00

Id is: 1000001
Id of the sender: 162572110
Id of the target: 133389807
Weight is: Heavy
priority: Regular
requested date: 25/01/2021
Drone id: 0
scheduled date: 25/01/2021 01:00:00
pickedUp date: 25/01/2021 01:45:00
delivered date: 25/01/2021 02:29:00

Id is: 1000002
Id of the sender: 384225131
Id of the target: 262796614
Weight is: Heavy
priority: Regular
requested date: 04/08/2021
Drone id: 0
scheduled date: 04/08/2021 03:00:00
pickedUp date: 04/08/2021 05:35:00
delivered date: 04/08/2021 06:38:00

Id is: 1000003
Id of the sender: 888567412
Id of the target: 536096226
Weight is: Light
priority: Regular
requested date: 03/07/2021
Drone id: 0
scheduled date: 03/07/2021 07:00:00
pickedUp date: 03/07/2021 08:01:00
delivered date: 03/07/2021 08:27:00

Id is: 1000004
Id of the sender: 349758986
Id of the target: 204442976
Weight is: Heavy
priority: Rapid
requested date: 24/06/2021
Drone id: 0
scheduled date: 24/06/2021 06:00:00
pickedUp date: 24/06/2021 08:30:00
delivered date: 24/06/2021 09:10:00

Id is: 1000005
Id of the sender: 934319758
Id of the target: 894585850
Weight is: Heavy
priority: Regular
requested date: 22/10/2021
Drone id: 0
scheduled date: 22/10/2021 04:00:00
pickedUp date: 22/10/2021 06:14:00
delivered date: 22/10/2021 07:27:00

Id is: 1000006
Id of the sender: 212853641
Id of the target: 185173292
Weight is: Light
priority: Regular
requested date: 12/01/2021
Drone id: 0
scheduled date: 12/01/2021 07:00:00
pickedUp date: 12/01/2021 09:12:00
delivered date: 12/01/2021 09:46:00

Id is: 1000007
Id of the sender: 146419259
Id of the target: 877801406
Weight is: Medium
priority: Rapid
requested date: 14/02/2021
Drone id: 0
scheduled date: 14/02/2021 04:00:00
pickedUp date: 14/02/2021 05:14:00
delivered date: 14/02/2021 05:46:00

Id is: 1000008
Id of the sender: 802405470
Id of the target: 957722204
Weight is: Heavy
priority: Emergency
requested date: 11/01/2021
Drone id: 0
scheduled date: 11/01/2021 05:00:00
pickedUp date: 11/01/2021 07:24:00
delivered date: 11/01/2021 08:20:00

Id is: 1000009
Id of the sender: 431375881
Id of the target: 893436073
Weight is: Medium
priority: Emergency
requested date: 12/01/2021
Drone id: 0
scheduled date: 12/01/2021 06:00:00
pickedUp date: 12/01/2021 08:05:00
delivered date: 12/01/2021 08:54:00

Id is: 1000010
Id of the sender: 123456789
Id of the target: 987654321
Weight is: Light
priority: Emergency
requested date: 03/11/2021
Drone id: 0
scheduled date: 03/11/2021 00:00:00
pickedUp date: 03/11/2021 00:00:00
delivered date: 03/11/2021 00:00:00

Id is: 1000011
Id of the sender: 543216789
Id of the target: 123459876
Weight is: Medium
priority: 3
requested date: 03/11/2021
Drone id: 0
scheduled date: 03/11/2021 00:00:00
pickedUp date: 03/11/2021 00:00:00
delivered date: 03/11/2021 00:00:00

press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
1
Press 1 to add parcel
Press 2 to add drone
Press 3 to add station
Press 4 to add customer
2
Enter drone ID:
4335
Enter drone model:
SE455
Maximum weight of the parcel: press 1 for heavy, 2 for medium and 3 for light:
3
press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
4
press 1 to view the list of base stations
press 2 to view the list of the drones
press 3 to view the list of the cutomers
press 4 to view the list of the parcels
press 5 to view the list of the parcels without drones
press 6 to view the list of the stations with available charge slots
2
Id is: 7926
model: ZX558
max weight is: Light
status is: Available
battery: 100

Id is: 9937
model: BH478
max weight is: Light
status is: Available
battery: 100

Id is: 5340
model: SG406
max weight is: Heavy
status is: Available
battery: 100

Id is: 4223
model: OD410
max weight is: Heavy
status is: Available
battery: 100

Id is: 4949
model: TT555
max weight is: Heavy
status is: Available
battery: 100

Id is: 4335
model: SE455
max weight is: Light
status is: Available
battery: 100

press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
1
Press 1 to add parcel
Press 2 to add drone
Press 3 to add station
Press 4 to add customer
3
Enter station ID:
12345
Enter station name:
harnof
Enter station longitude:
21.0987654
Enter station latitude:
35.098762
Enter number of empty charging slots:
10
press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
4
press 1 to view the list of base stations
press 2 to view the list of the drones
press 3 to view the list of the cutomers
press 4 to view the list of the parcels
press 5 to view the list of the parcels without drones
press 6 to view the list of the stations with available charge slots
1
Id is: 91146
Name of station: Pisgat Zeev
Longitude is: 31°49' 52"N
Latitude is: 35°14' 33"E
Num of charge slots: 1

Id is: 13076
Name of station: Givat Shaul
Longitude is: 31°47' 27"N
Latitude is: 35°11' 43"E
Num of charge slots: 2

Id is: 12345
Name of station: harnof
Longitude is: 21°5' 56"N
Latitude is: 35°5' 56"E
Num of charge slots: 10

press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
1
Press 1 to add parcel
Press 2 to add drone
Press 3 to add station
Press 4 to add customer
4
Enter customer ID:
123456789
Enter customer name:
shlomo
Enter phone number:
0501234567
Enter customer longitude:
31.5675645645679
Enter customer latitude:
35.90878553
press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
4
press 1 to view the list of base stations
press 2 to view the list of the drones
press 3 to view the list of the cutomers
press 4 to view the list of the parcels
press 5 to view the list of the parcels without drones
press 6 to view the list of the stations with available charge slots
3
Id is: 564676898
Name of customer: Avraham Cohen
Phone number: 054-9837623
longitude is: 31°44' 18"N
latitude: 35°15' 46"E

Id is: 415598608
Name of customer: Yitshak Levi
Phone number: 054-6406055
longitude is: 31°35' 28"N
latitude: 35°40' 57"E

Id is: 466706423
Name of customer: Yaakov Israeli
Phone number: 052-9625561
longitude is: 31°49' 37"N
latitude: 35°23' 17"E

Id is: 572822759
Name of customer: Sarah Shalom
Phone number: 054-1023164
longitude is: 31°36' 45"N
latitude: 35°37' 40"E

Id is: 350354745
Name of customer: Rivka Silver
Phone number: 052-3666108
longitude is: 31°11' 35"N
latitude: 35°2' 37"E

Id is: 731045631
Name of customer: Rahel Shushan
Phone number: 050-6305269
longitude is: 31°36' 33"N
latitude: 35°31' 19"E

Id is: 489572931
Name of customer: Leah Yosefi
Phone number: 050-2981215
longitude is: 31°12' 29"N
latitude: 35°46' 39"E

Id is: 557658949
Name of customer: David Dayan
Phone number: 054-4485183
longitude is: 31°0' 19"N
latitude: 35°9' 18"E

Id is: 762657625
Name of customer: Moshe Biton
Phone number: 054-8570872
longitude is: 31°26' 43"N
latitude: 35°17' 1"E

Id is: 370788837
Name of customer: Aharon Uzan
Phone number: 052-2949273
longitude is: 31°24' 22"N
latitude: 35°18' 43"E

Id is: 123456789
Name of customer: shlomo
Phone number: 0501234567
longitude is: 31°34' 3"N
latitude: 35°54' 32"E

press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
2
Press 1 to match drone to parcel
Press 2 to pick up parcel
Press 3 to deliver parcel to customer
Press 4 to send drone to charge
Press 5 to free drone from charging
1
Id is: 7926
model: ZX558
max weight is: Light
status is: Available
battery: 100

Id is: 9937
model: BH478
max weight is: Light
status is: Available
battery: 100

Id is: 5340
model: SG406
max weight is: Heavy
status is: Available
battery: 100

Id is: 4223
model: OD410
max weight is: Heavy
status is: Available
battery: 100

Id is: 4949
model: TT555
max weight is: Heavy
status is: Available
battery: 100

Id is: 4335
model: SE455
max weight is: Light
status is: Available
battery: 100

Enter the ID of the drone you want to send:
4949
Id is: 1000000
Id of the sender: 613499860
Id of the target: 909150364
Weight is: Medium
priority: Rapid
requested date: 23/10/2021
Drone id: 0
scheduled date: 23/10/2021 07:00:00
pickedUp date: 23/10/2021 08:53:00
delivered date: 23/10/2021 10:18:00

Id is: 1000001
Id of the sender: 162572110
Id of the target: 133389807
Weight is: Heavy
priority: Regular
requested date: 25/01/2021
Drone id: 0
scheduled date: 25/01/2021 01:00:00
pickedUp date: 25/01/2021 01:45:00
delivered date: 25/01/2021 02:29:00

Id is: 1000002
Id of the sender: 384225131
Id of the target: 262796614
Weight is: Heavy
priority: Regular
requested date: 04/08/2021
Drone id: 0
scheduled date: 04/08/2021 03:00:00
pickedUp date: 04/08/2021 05:35:00
delivered date: 04/08/2021 06:38:00

Id is: 1000003
Id of the sender: 888567412
Id of the target: 536096226
Weight is: Light
priority: Regular
requested date: 03/07/2021
Drone id: 0
scheduled date: 03/07/2021 07:00:00
pickedUp date: 03/07/2021 08:01:00
delivered date: 03/07/2021 08:27:00

Id is: 1000004
Id of the sender: 349758986
Id of the target: 204442976
Weight is: Heavy
priority: Rapid
requested date: 24/06/2021
Drone id: 0
scheduled date: 24/06/2021 06:00:00
pickedUp date: 24/06/2021 08:30:00
delivered date: 24/06/2021 09:10:00

Id is: 1000005
Id of the sender: 934319758
Id of the target: 894585850
Weight is: Heavy
priority: Regular
requested date: 22/10/2021
Drone id: 0
scheduled date: 22/10/2021 04:00:00
pickedUp date: 22/10/2021 06:14:00
delivered date: 22/10/2021 07:27:00

Id is: 1000006
Id of the sender: 212853641
Id of the target: 185173292
Weight is: Light
priority: Regular
requested date: 12/01/2021
Drone id: 0
scheduled date: 12/01/2021 07:00:00
pickedUp date: 12/01/2021 09:12:00
delivered date: 12/01/2021 09:46:00

Id is: 1000007
Id of the sender: 146419259
Id of the target: 877801406
Weight is: Medium
priority: Rapid
requested date: 14/02/2021
Drone id: 0
scheduled date: 14/02/2021 04:00:00
pickedUp date: 14/02/2021 05:14:00
delivered date: 14/02/2021 05:46:00

Id is: 1000008
Id of the sender: 802405470
Id of the target: 957722204
Weight is: Heavy
priority: Emergency
requested date: 11/01/2021
Drone id: 0
scheduled date: 11/01/2021 05:00:00
pickedUp date: 11/01/2021 07:24:00
delivered date: 11/01/2021 08:20:00

Id is: 1000009
Id of the sender: 431375881
Id of the target: 893436073
Weight is: Medium
priority: Emergency
requested date: 12/01/2021
Drone id: 0
scheduled date: 12/01/2021 06:00:00
pickedUp date: 12/01/2021 08:05:00
delivered date: 12/01/2021 08:54:00

Id is: 1000010
Id of the sender: 123456789
Id of the target: 987654321
Weight is: Light
priority: Emergency
requested date: 03/11/2021
Drone id: 0
scheduled date: 03/11/2021 00:00:00
pickedUp date: 03/11/2021 00:00:00
delivered date: 03/11/2021 00:00:00

Id is: 1000011
Id of the sender: 543216789
Id of the target: 123459876
Weight is: Medium
priority: 3
requested date: 03/11/2021
Drone id: 0
scheduled date: 03/11/2021 00:00:00
pickedUp date: 03/11/2021 00:00:00
delivered date: 03/11/2021 00:00:00

Enter ID of the parcel you want to send:
1000011
press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
2
Press 1 to match drone to parcel
Press 2 to pick up parcel
Press 3 to deliver parcel to customer
Press 4 to send drone to charge
Press 5 to free drone from charging
2
Id is: 1000000
Id of the sender: 613499860
Id of the target: 909150364
Weight is: Medium
priority: Rapid
requested date: 23/10/2021
Drone id: 0
scheduled date: 23/10/2021 07:00:00
pickedUp date: 23/10/2021 08:53:00
delivered date: 23/10/2021 10:18:00

Id is: 1000001
Id of the sender: 162572110
Id of the target: 133389807
Weight is: Heavy
priority: Regular
requested date: 25/01/2021
Drone id: 0
scheduled date: 25/01/2021 01:00:00
pickedUp date: 25/01/2021 01:45:00
delivered date: 25/01/2021 02:29:00

Id is: 1000002
Id of the sender: 384225131
Id of the target: 262796614
Weight is: Heavy
priority: Regular
requested date: 04/08/2021
Drone id: 0
scheduled date: 04/08/2021 03:00:00
pickedUp date: 04/08/2021 05:35:00
delivered date: 04/08/2021 06:38:00

Id is: 1000003
Id of the sender: 888567412
Id of the target: 536096226
Weight is: Light
priority: Regular
requested date: 03/07/2021
Drone id: 0
scheduled date: 03/07/2021 07:00:00
pickedUp date: 03/07/2021 08:01:00
delivered date: 03/07/2021 08:27:00

Id is: 1000004
Id of the sender: 349758986
Id of the target: 204442976
Weight is: Heavy
priority: Rapid
requested date: 24/06/2021
Drone id: 0
scheduled date: 24/06/2021 06:00:00
pickedUp date: 24/06/2021 08:30:00
delivered date: 24/06/2021 09:10:00

Id is: 1000005
Id of the sender: 934319758
Id of the target: 894585850
Weight is: Heavy
priority: Regular
requested date: 22/10/2021
Drone id: 0
scheduled date: 22/10/2021 04:00:00
pickedUp date: 22/10/2021 06:14:00
delivered date: 22/10/2021 07:27:00

Id is: 1000006
Id of the sender: 212853641
Id of the target: 185173292
Weight is: Light
priority: Regular
requested date: 12/01/2021
Drone id: 0
scheduled date: 12/01/2021 07:00:00
pickedUp date: 12/01/2021 09:12:00
delivered date: 12/01/2021 09:46:00

Id is: 1000007
Id of the sender: 146419259
Id of the target: 877801406
Weight is: Medium
priority: Rapid
requested date: 14/02/2021
Drone id: 0
scheduled date: 14/02/2021 04:00:00
pickedUp date: 14/02/2021 05:14:00
delivered date: 14/02/2021 05:46:00

Id is: 1000008
Id of the sender: 802405470
Id of the target: 957722204
Weight is: Heavy
priority: Emergency
requested date: 11/01/2021
Drone id: 0
scheduled date: 11/01/2021 05:00:00
pickedUp date: 11/01/2021 07:24:00
delivered date: 11/01/2021 08:20:00

Id is: 1000009
Id of the sender: 431375881
Id of the target: 893436073
Weight is: Medium
priority: Emergency
requested date: 12/01/2021
Drone id: 0
scheduled date: 12/01/2021 06:00:00
pickedUp date: 12/01/2021 08:05:00
delivered date: 12/01/2021 08:54:00

Id is: 1000010
Id of the sender: 123456789
Id of the target: 987654321
Weight is: Light
priority: Emergency
requested date: 03/11/2021
Drone id: 0
scheduled date: 03/11/2021 00:00:00
pickedUp date: 03/11/2021 00:00:00
delivered date: 03/11/2021 00:00:00

Id is: 1000011
Id of the sender: 543216789
Id of the target: 123459876
Weight is: Medium
priority: 3
requested date: 03/11/2021
Drone id: 4949
scheduled date: 03/11/2021 13:09:20
pickedUp date: 03/11/2021 00:00:00
delivered date: 03/11/2021 00:00:00

Enter the ID of the parcel you want to pick up:
1000011
press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
4
press 1 to view the list of base stations
press 2 to view the list of the drones
press 3 to view the list of the cutomers
press 4 to view the list of the parcels
press 5 to view the list of the parcels without drones
press 6 to view the list of the stations with available charge slots
4
Id is: 1000000
Id of the sender: 613499860
Id of the target: 909150364
Weight is: Medium
priority: Rapid
requested date: 23/10/2021
Drone id: 0
scheduled date: 23/10/2021 07:00:00
pickedUp date: 23/10/2021 08:53:00
delivered date: 23/10/2021 10:18:00

Id is: 1000001
Id of the sender: 162572110
Id of the target: 133389807
Weight is: Heavy
priority: Regular
requested date: 25/01/2021
Drone id: 0
scheduled date: 25/01/2021 01:00:00
pickedUp date: 25/01/2021 01:45:00
delivered date: 25/01/2021 02:29:00

Id is: 1000002
Id of the sender: 384225131
Id of the target: 262796614
Weight is: Heavy
priority: Regular
requested date: 04/08/2021
Drone id: 0
scheduled date: 04/08/2021 03:00:00
pickedUp date: 04/08/2021 05:35:00
delivered date: 04/08/2021 06:38:00

Id is: 1000003
Id of the sender: 888567412
Id of the target: 536096226
Weight is: Light
priority: Regular
requested date: 03/07/2021
Drone id: 0
scheduled date: 03/07/2021 07:00:00
pickedUp date: 03/07/2021 08:01:00
delivered date: 03/07/2021 08:27:00

Id is: 1000004
Id of the sender: 349758986
Id of the target: 204442976
Weight is: Heavy
priority: Rapid
requested date: 24/06/2021
Drone id: 0
scheduled date: 24/06/2021 06:00:00
pickedUp date: 24/06/2021 08:30:00
delivered date: 24/06/2021 09:10:00

Id is: 1000005
Id of the sender: 934319758
Id of the target: 894585850
Weight is: Heavy
priority: Regular
requested date: 22/10/2021
Drone id: 0
scheduled date: 22/10/2021 04:00:00
pickedUp date: 22/10/2021 06:14:00
delivered date: 22/10/2021 07:27:00

Id is: 1000006
Id of the sender: 212853641
Id of the target: 185173292
Weight is: Light
priority: Regular
requested date: 12/01/2021
Drone id: 0
scheduled date: 12/01/2021 07:00:00
pickedUp date: 12/01/2021 09:12:00
delivered date: 12/01/2021 09:46:00

Id is: 1000007
Id of the sender: 146419259
Id of the target: 877801406
Weight is: Medium
priority: Rapid
requested date: 14/02/2021
Drone id: 0
scheduled date: 14/02/2021 04:00:00
pickedUp date: 14/02/2021 05:14:00
delivered date: 14/02/2021 05:46:00

Id is: 1000008
Id of the sender: 802405470
Id of the target: 957722204
Weight is: Heavy
priority: Emergency
requested date: 11/01/2021
Drone id: 0
scheduled date: 11/01/2021 05:00:00
pickedUp date: 11/01/2021 07:24:00
delivered date: 11/01/2021 08:20:00

Id is: 1000009
Id of the sender: 431375881
Id of the target: 893436073
Weight is: Medium
priority: Emergency
requested date: 12/01/2021
Drone id: 0
scheduled date: 12/01/2021 06:00:00
pickedUp date: 12/01/2021 08:05:00
delivered date: 12/01/2021 08:54:00

Id is: 1000010
Id of the sender: 123456789
Id of the target: 987654321
Weight is: Light
priority: Emergency
requested date: 03/11/2021
Drone id: 0
scheduled date: 03/11/2021 00:00:00
pickedUp date: 03/11/2021 00:00:00
delivered date: 03/11/2021 00:00:00

Id is: 1000011
Id of the sender: 543216789
Id of the target: 123459876
Weight is: Medium
priority: 3
requested date: 03/11/2021
Drone id: 4949
scheduled date: 03/11/2021 13:09:20
pickedUp date: 03/11/2021 13:12:44
delivered date: 03/11/2021 00:00:00

press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
2
Press 1 to match drone to parcel
Press 2 to pick up parcel
Press 3 to deliver parcel to customer
Press 4 to send drone to charge
Press 5 to free drone from charging
2
Id is: 1000000
Id of the sender: 613499860
Id of the target: 909150364
Weight is: Medium
priority: Rapid
requested date: 23/10/2021
Drone id: 0
scheduled date: 23/10/2021 07:00:00
pickedUp date: 23/10/2021 08:53:00
delivered date: 23/10/2021 10:18:00

Id is: 1000001
Id of the sender: 162572110
Id of the target: 133389807
Weight is: Heavy
priority: Regular
requested date: 25/01/2021
Drone id: 0
scheduled date: 25/01/2021 01:00:00
pickedUp date: 25/01/2021 01:45:00
delivered date: 25/01/2021 02:29:00

Id is: 1000002
Id of the sender: 384225131
Id of the target: 262796614
Weight is: Heavy
priority: Regular
requested date: 04/08/2021
Drone id: 0
scheduled date: 04/08/2021 03:00:00
pickedUp date: 04/08/2021 05:35:00
delivered date: 04/08/2021 06:38:00

Id is: 1000003
Id of the sender: 888567412
Id of the target: 536096226
Weight is: Light
priority: Regular
requested date: 03/07/2021
Drone id: 0
scheduled date: 03/07/2021 07:00:00
pickedUp date: 03/07/2021 08:01:00
delivered date: 03/07/2021 08:27:00

Id is: 1000004
Id of the sender: 349758986
Id of the target: 204442976
Weight is: Heavy
priority: Rapid
requested date: 24/06/2021
Drone id: 0
scheduled date: 24/06/2021 06:00:00
pickedUp date: 24/06/2021 08:30:00
delivered date: 24/06/2021 09:10:00

Id is: 1000005
Id of the sender: 934319758
Id of the target: 894585850
Weight is: Heavy
priority: Regular
requested date: 22/10/2021
Drone id: 0
scheduled date: 22/10/2021 04:00:00
pickedUp date: 22/10/2021 06:14:00
delivered date: 22/10/2021 07:27:00

Id is: 1000006
Id of the sender: 212853641
Id of the target: 185173292
Weight is: Light
priority: Regular
requested date: 12/01/2021
Drone id: 0
scheduled date: 12/01/2021 07:00:00
pickedUp date: 12/01/2021 09:12:00
delivered date: 12/01/2021 09:46:00

Id is: 1000007
Id of the sender: 146419259
Id of the target: 877801406
Weight is: Medium
priority: Rapid
requested date: 14/02/2021
Drone id: 0
scheduled date: 14/02/2021 04:00:00
pickedUp date: 14/02/2021 05:14:00
delivered date: 14/02/2021 05:46:00

Id is: 1000008
Id of the sender: 802405470
Id of the target: 957722204
Weight is: Heavy
priority: Emergency
requested date: 11/01/2021
Drone id: 0
scheduled date: 11/01/2021 05:00:00
pickedUp date: 11/01/2021 07:24:00
delivered date: 11/01/2021 08:20:00

Id is: 1000009
Id of the sender: 431375881
Id of the target: 893436073
Weight is: Medium
priority: Emergency
requested date: 12/01/2021
Drone id: 0
scheduled date: 12/01/2021 06:00:00
pickedUp date: 12/01/2021 08:05:00
delivered date: 12/01/2021 08:54:00

Id is: 1000010
Id of the sender: 123456789
Id of the target: 987654321
Weight is: Light
priority: Emergency
requested date: 03/11/2021
Drone id: 0
scheduled date: 03/11/2021 00:00:00
pickedUp date: 03/11/2021 00:00:00
delivered date: 03/11/2021 00:00:00

Id is: 1000011
Id of the sender: 543216789
Id of the target: 123459876
Weight is: Medium
priority: 3
requested date: 03/11/2021
Drone id: 4949
scheduled date: 03/11/2021 13:09:20
pickedUp date: 03/11/2021 13:12:44
delivered date: 03/11/2021 00:00:00

Enter the ID of the parcel you want to pick up:
1000011
press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
2
Press 1 to match drone to parcel
Press 2 to pick up parcel
Press 3 to deliver parcel to customer
Press 4 to send drone to charge
Press 5 to free drone from charging
3
Enter the ID of the parcel you want to deliver:
Id is: 1000000
Id of the sender: 613499860
Id of the target: 909150364
Weight is: Medium
priority: Rapid
requested date: 23/10/2021
Drone id: 0
scheduled date: 23/10/2021 07:00:00
pickedUp date: 23/10/2021 08:53:00
delivered date: 23/10/2021 10:18:00

Id is: 1000001
Id of the sender: 162572110
Id of the target: 133389807
Weight is: Heavy
priority: Regular
requested date: 25/01/2021
Drone id: 0
scheduled date: 25/01/2021 01:00:00
pickedUp date: 25/01/2021 01:45:00
delivered date: 25/01/2021 02:29:00

Id is: 1000002
Id of the sender: 384225131
Id of the target: 262796614
Weight is: Heavy
priority: Regular
requested date: 04/08/2021
Drone id: 0
scheduled date: 04/08/2021 03:00:00
pickedUp date: 04/08/2021 05:35:00
delivered date: 04/08/2021 06:38:00

Id is: 1000003
Id of the sender: 888567412
Id of the target: 536096226
Weight is: Light
priority: Regular
requested date: 03/07/2021
Drone id: 0
scheduled date: 03/07/2021 07:00:00
pickedUp date: 03/07/2021 08:01:00
delivered date: 03/07/2021 08:27:00

Id is: 1000004
Id of the sender: 349758986
Id of the target: 204442976
Weight is: Heavy
priority: Rapid
requested date: 24/06/2021
Drone id: 0
scheduled date: 24/06/2021 06:00:00
pickedUp date: 24/06/2021 08:30:00
delivered date: 24/06/2021 09:10:00

Id is: 1000005
Id of the sender: 934319758
Id of the target: 894585850
Weight is: Heavy
priority: Regular
requested date: 22/10/2021
Drone id: 0
scheduled date: 22/10/2021 04:00:00
pickedUp date: 22/10/2021 06:14:00
delivered date: 22/10/2021 07:27:00

Id is: 1000006
Id of the sender: 212853641
Id of the target: 185173292
Weight is: Light
priority: Regular
requested date: 12/01/2021
Drone id: 0
scheduled date: 12/01/2021 07:00:00
pickedUp date: 12/01/2021 09:12:00
delivered date: 12/01/2021 09:46:00

Id is: 1000007
Id of the sender: 146419259
Id of the target: 877801406
Weight is: Medium
priority: Rapid
requested date: 14/02/2021
Drone id: 0
scheduled date: 14/02/2021 04:00:00
pickedUp date: 14/02/2021 05:14:00
delivered date: 14/02/2021 05:46:00

Id is: 1000008
Id of the sender: 802405470
Id of the target: 957722204
Weight is: Heavy
priority: Emergency
requested date: 11/01/2021
Drone id: 0
scheduled date: 11/01/2021 05:00:00
pickedUp date: 11/01/2021 07:24:00
delivered date: 11/01/2021 08:20:00

Id is: 1000009
Id of the sender: 431375881
Id of the target: 893436073
Weight is: Medium
priority: Emergency
requested date: 12/01/2021
Drone id: 0
scheduled date: 12/01/2021 06:00:00
pickedUp date: 12/01/2021 08:05:00
delivered date: 12/01/2021 08:54:00

Id is: 1000010
Id of the sender: 123456789
Id of the target: 987654321
Weight is: Light
priority: Emergency
requested date: 03/11/2021
Drone id: 0
scheduled date: 03/11/2021 00:00:00
pickedUp date: 03/11/2021 00:00:00
delivered date: 03/11/2021 00:00:00

Id is: 1000011
Id of the sender: 543216789
Id of the target: 123459876
Weight is: Medium
priority: 3
requested date: 03/11/2021
Drone id: 4949
scheduled date: 03/11/2021 13:09:20
pickedUp date: 03/11/2021 13:16:00
delivered date: 03/11/2021 00:00:00

1000011
press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
2
Press 1 to match drone to parcel
Press 2 to pick up parcel
Press 3 to deliver parcel to customer
Press 4 to send drone to charge
Press 5 to free drone from charging
4
Id is: 7926
model: ZX558
max weight is: Light
status is: Available
battery: 100

Id is: 9937
model: BH478
max weight is: Light
status is: Available
battery: 100

Id is: 5340
model: SG406
max weight is: Heavy
status is: Available
battery: 100

Id is: 4223
model: OD410
max weight is: Heavy
status is: Available
battery: 100

Id is: 4335
model: SE455
max weight is: Light
status is: Available
battery: 100

Id is: 4949
model: TT555
max weight is: Heavy
status is: Delivery
battery: 100

Enter the ID of the drone you want to charge:
4949
Enter the ID of the station you want to charge in:
12345
press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
2
Press 1 to match drone to parcel
Press 2 to pick up parcel
Press 3 to deliver parcel to customer
Press 4 to send drone to charge
Press 5 to free drone from charging
4
Id is: 7926
model: ZX558
max weight is: Light
status is: Available
battery: 100

Id is: 9937
model: BH478
max weight is: Light
status is: Available
battery: 100

Id is: 5340
model: SG406
max weight is: Heavy
status is: Available
battery: 100

Id is: 4223
model: OD410
max weight is: Heavy
status is: Available
battery: 100

Id is: 4335
model: SE455
max weight is: Light
status is: Available
battery: 100

Enter the ID of the drone you want to charge:
5340
Enter the ID of the station you want to charge in:
12345
press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
4
press 1 to view the list of base stations
press 2 to view the list of the drones
press 3 to view the list of the cutomers
press 4 to view the list of the parcels
press 5 to view the list of the parcels without drones
press 6 to view the list of the stations with available charge slots
2
Id is: 7926
model: ZX558
max weight is: Light
status is: Available
battery: 100

Id is: 9937
model: BH478
max weight is: Light
status is: Available
battery: 100

Id is: 4223
model: OD410
max weight is: Heavy
status is: Available
battery: 100

Id is: 4335
model: SE455
max weight is: Light
status is: Available
battery: 100

Id is: 4949
model: TT555
max weight is: Heavy
status is: Maintenance
battery: 100

Id is: 5340
model: SG406
max weight is: Heavy
status is: Maintenance
battery: 100

press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
4
press 1 to view the list of base stations
press 2 to view the list of the drones
press 3 to view the list of the cutomers
press 4 to view the list of the parcels
press 5 to view the list of the parcels without drones
press 6 to view the list of the stations with available charge slots
1
Id is: 91146
Name of station: Pisgat Zeev
Longitude is: 31°49' 52"N
Latitude is: 35°14' 33"E
Num of charge slots: 1

Id is: 13076
Name of station: Givat Shaul
Longitude is: 31°47' 27"N
Latitude is: 35°11' 43"E
Num of charge slots: 2

Id is: 12345
Name of station: harnof
Longitude is: 21°5' 56"N
Latitude is: 35°5' 56"E
Num of charge slots: 8

press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
2
Press 1 to match drone to parcel
Press 2 to pick up parcel
Press 3 to deliver parcel to customer
Press 4 to send drone to charge
Press 5 to free drone from charging
5
Id is: 4949
model: TT555
max weight is: Heavy
status is: Maintenance
battery: 100

Id is: 5340
model: SG406
max weight is: Heavy
status is: Maintenance
battery: 100

Enter the ID of the drone you want to free:
4949
press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
5340
press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
4
press 1 to view the list of base stations
press 2 to view the list of the drones
press 3 to view the list of the cutomers
press 4 to view the list of the parcels
press 5 to view the list of the parcels without drones
press 6 to view the list of the stations with available charge slots
2
Id is: 7926
model: ZX558
max weight is: Light
status is: Available
battery: 100

Id is: 9937
model: BH478
max weight is: Light
status is: Available
battery: 100

Id is: 4223
model: OD410
max weight is: Heavy
status is: Available
battery: 100

Id is: 4335
model: SE455
max weight is: Light
status is: Available
battery: 100

Id is: 4949
model: TT555
max weight is: Heavy
status is: Maintenance
battery: 100

Id is: 5340
model: SG406
max weight is: Heavy
status is: Maintenance
battery: 100

press 1 to add an item
press 2 to update an item
press 3 to view details of specific item
press 4 to view a list of specific item
press 0 to stop
0
*/