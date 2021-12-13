using System;
using System.Collections.Generic;
using IBL.BO;

namespace IBL
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
        void UpdateStation(Station newStation);
        void FreeDrone(int droneId, double timeInCharging);
        void DroneToParcel(int id);
        void rechargeDrone(Drone drone);
        void PickUpParcel(Drone drone);
        void deliveryPackage(Drone drone);

        //Get functions:
        DroneToList GetDroneToList(int id);
        Drone GetDrone(int id);
        Station GetStation(int id);
        Customer GetCustomer(int id);
        Parcel GetParcel(int id);
        IEnumerable<StationToList> GetListOfStations(Func<StationToList, bool> predicate = null);
        IEnumerable<ParcelToList> GetListofParcels(Func<ParcelToList, bool> predicate = null);
        IEnumerable<DroneToList> GetListOfDrones(Func<DroneToList, bool> predicate = null);
        //IEnumerable<ParcelToList> GetListofParcelsWithoutDrone(Func<ParcelToList, bool> predicate = null);
        //IEnumerable<StationToList> GetListOfStationsWithAvailableChargeSlots(Func<StationToList, bool> predicate = null);
        IEnumerable<CustomerToList> GetListOfCustomers(Func<CustomerToList, bool> predicate = null);

    }
}