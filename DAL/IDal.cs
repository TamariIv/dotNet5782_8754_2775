using IDAL.DO;
using System;
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
        IEnumerable<Station> GetStations(Func<Station, bool> predicate = null);
        IEnumerable<Parcel> GetParcels();
        IEnumerable<DroneCharge> GetDroneCharges();
        IEnumerable<Parcel> GetParcelWithoutDrone();
        #endregion

        #region Update part of C.R.U.D
        void MatchDroneToParcel(Parcel p, Drone d);
        void ParcelDelivered(Parcel p);
        void PickUpParcel(Parcel p);
        void SendDroneFromStation(Drone d);
        void SendDroneToCharge(Drone d, Station s);
        void UpdateDrone(Drone d);
        #endregion
        //IEnumerable<Customer> GetCustomers();

        void UpdateCustomer(Customer c);
        void UpdateStation(Station s);
        //void UpdateDrone(Drone d);


        double[] GetElectricity();
        Station getClosestStation(double latitude, double longitude);
        List<Customer> GetCustomersWithParcels(List<IDAL.DO.Parcel> parcels, List<IDAL.DO.Customer> customers);
    }
}