using System;
using System.Collections.Generic;
using BO;

namespace BlApi
{
    public interface IBL
    {
        //Add functions:
        void AddStation(Station newStation);
        void AddCustomer(Customer customer);
        void AddParcel(Parcel parcel);
        void AddDrone(Drone newDrone, int stationId);

        //Update functions:
        void UpdateCustomer(Customer newCustomer);
        void UpdateDrone(Drone d);
        void UpdateStation(int id, string name, int chargeSlots);
        void FreeDrone(int droneId);
        void DroneToParcel(int id);
        void RechargeDrone(int id);
        void PickUpParcel(Drone drone);
        void deliveryPackage(Drone drone);

        //Get functions:
        DroneToList GetDroneToList(int id);
        StationToList GetStationToList(int id);
        Drone GetDrone(int id);
        Station GetStation(int id);
        Customer GetCustomer(int id);
        Parcel GetParcel(int id);
        IEnumerable<StationToList> GetListOfStations();
        IEnumerable<ParcelToList> GetListofParcels();
        IEnumerable<DroneToList> GetListOfDrones();
        IEnumerable<ParcelToList> GetListofParcelsWithoutDrone();
        IEnumerable<StationToList> GetListOfStationsWithAvailableChargeSlots();
        IEnumerable<CustomerToList> GetListOfCustomers();


        //Delete functions:
        void DeleteStation(int id);
        void DeleteDrone(int id);
        void DeleteCustomer(int id);
        void DeleteParcel(int id);

        void StartDroneSimulator(int id, Action update, Func<bool> checkStop);

    }
}