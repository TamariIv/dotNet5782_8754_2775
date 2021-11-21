using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        void AddStation(Station newStation);
        void AddCustomer(Customer customer);
        void AddParcel(ParcelInDelivey parcel);
    }
}