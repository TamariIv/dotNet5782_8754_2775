﻿using IDAL.DO;
using System.Collections.Generic;

namespace IDAL.DO
{
    public interface IDal
    {
        #region Create part of C.R.U.D
        void AddCustomer(Customer c);
        void AddDrone(Drone d);
        int AddParcel(Parcel p);
        void AddStation(Station s);
        #endregion

        #region Request part of C.R.U.D
        Customer GetCustomer(int idNumber);
        Drone GetDrone(int idNumber);
        Station GetStation(int idNumber);
        Parcel GetParcel(int idNumber);
        DroneCharge GetDroneCharge(int idNumber);
        IEnumerable<Drone> GetDrones();
        IEnumerable<Customer> GetCustomers();
        IEnumerable<Station> GetStations();
        IEnumerable<Parcel> GetParcels();
        IEnumerable<Parcel> GetParcelWithoutDrone();
        #endregion

        #region Update part of C.R.U.D
        void MatchDroneToParcel(Parcel p, Drone d);
        void ParcelDelivered(Parcel p);
        void PickUpParcel(Parcel p);
        void SendDroneFromStation(Drone d);
        void SendDroneToCharge(Drone d, Station s);
        #endregion

        public double[] getElectricity()
        {
            double[] electricityRates = { DalObject.DataSource.Config.Available, DalObject.DataSource.Config.LightWeight, DalObject.DataSource.Config.MediumWeight, DalObject.DataSource.Config.HeavyWeight, DalObject.DataSource.Config.ChargingRate }; 
            return electricityRates;
        }
    }
}