using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        //Add functions:
        void AddStation(Station newStation);
        void AddCustomer(Customer customer);
        void AddParcel(ParcelInDelivey parcel);
        void UpdateDrone(Drone d);
        void AddDrone(Drone newDrone);

        //Update functions:
        void UpdateCustomer(Customer newCustomer);
        void rechargeDrone(Drone drone);
        void CollectPackageByDrone(Drone drone, Parcel parcel);
        void deliveryPackage(Drone drone, Parcel parcel);
    }
}