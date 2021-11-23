using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        void AddStation(Station newStation);
        void AddCustomer(Customer customer);
        void AddParcel(ParcelInDelivey parcel);
        void UpdateDrone(Drone d);
        void AddDrone(Drone newDrone);

        void UpdateCustomer(Customer newCustomer);
        void rechargeDrone(Drone drone);
    }
}