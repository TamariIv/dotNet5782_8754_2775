﻿using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        //Add functions:
        void AddStation(Station newStation);
        void AddCustomer(Customer customer);
        void AddParcel(ParcelInDelivey parcel);
        void UpdateDrone(Drone d);
        //void AddDrone(Drone newDrone);

        //Update functions:
        void UpdateCustomer(Customer newCustomer);
        void AddDrone(Drone newDrone, int stationId);
        void UpdateStation(Station newStation);
        void FreeDrone(int droneId, double timeInCharging);
        void DroneToParcel(int id);
        DroneToList GetDroneToList(int idNumber);

        void rechargeDrone(Drone drone);
        void PickUpParcel(Drone drone);
        void deliveryPackage(Drone drone, Parcel parcel);
        ParcelToList getParcelToList(int id);
    }
}