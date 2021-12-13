using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BlObject:IBL.IBL
    {
        public void UpdateCustomer(IBL.BO.Customer newCustomer)
        {
            IDAL.DO.Customer dalCustomer = dal.GetCustomer(newCustomer.Id);
            if (newCustomer.Name == "" && newCustomer.Phone == "")
                throw new NoUpdateException("no update to customer was received\n");
            if (newCustomer.Name != "")
                dalCustomer.Name = newCustomer.Name;
            if (newCustomer.Phone != "")
                dalCustomer.Phone = newCustomer.Phone;
            dal.UpdateCustomer(dalCustomer);
        }

        public void AddCustomer(IBL.BO.Customer newCustomer)
        {
            IDAL.DO.Customer dalCustomer = new IDAL.DO.Customer
            {
                Id = newCustomer.Id,
                Name = newCustomer.Name,
                Phone = newCustomer.Phone,
                Longitude = newCustomer.Location.Longitude,
                Latitude = newCustomer.Location.Latitude

            };
            dal.AddCustomer(dalCustomer);
        }

        private IBL.BO.CustomerToList convertCustomerToCustomerToList(IDAL.DO.Customer dalCustomer)
        {
            int sentAndDelivered = 0, sentAndNotDelivered = 0, received = 0, inDeliveryToCustomer = 0;
            foreach (var p in dal.GetParcels())
            {
                if (p.SenderId == dalCustomer.Id)
                {
                    if (getParcelStatus(p) == IBL.BO.ParcelStatus.Delivered)
                        sentAndDelivered++;
                    else sentAndNotDelivered++;
                }
                else if (p.TargetId == dalCustomer.Id)
                {
                    if (getParcelStatus(p) == IBL.BO.ParcelStatus.Delivered)
                        received++;
                    else
                        inDeliveryToCustomer++;
                }
            }
            IBL.BO.CustomerToList blCustomer = new IBL.BO.CustomerToList
            {
                Id = dalCustomer.Id,
                Name = dalCustomer.Name,
                Phone = dalCustomer.Phone,
                SentAndDelivered = sentAndDelivered,
                SentAndNotDeliverd = sentAndNotDelivered,
                Recieved = received,
                InDeliveryToCustomer = inDeliveryToCustomer
            };
            return blCustomer;
        }

        public IBL.BO.Customer GetCustomer(int id)
        {
            IDAL.DO.Customer dalCustomer = dal.GetCustomer(id);
            List<IDAL.DO.Parcel> parcelsOfsender = dal.GetParcels().ToList().FindAll(p => p.SenderId == id);
            List<IDAL.DO.Parcel> parcelsOfreceiver = dal.GetParcels().ToList().FindAll(p => p.TargetId == id);
            IBL.BO.Customer customer = new IBL.BO.Customer
            {
                Id = dalCustomer.Id,
                Name = dalCustomer.Name,
                Phone = dalCustomer.Phone,
                Location = new IBL.BO.Location { Latitude = dalCustomer.Latitude, Longitude = dalCustomer.Longitude },
                Send = convertParcelsToParcelsCustomer(parcelsOfsender, id),
                Receive = convertParcelsToParcelsCustomer(parcelsOfreceiver, id)
        };
            return customer;
        }

        private List<IBL.BO.ParcelInCustomer> convertParcelsToParcelsCustomer(List<IDAL.DO.Parcel> parcelsOfSender, int id)
        {
            List<IBL.BO.ParcelInCustomer> parcels = new List<IBL.BO.ParcelInCustomer>();
            foreach (var item in parcelsOfSender)
            {
                parcels.Add(convertParcelToParcelInCustomer(item, id));
            }
            return parcels;
        }

        private IBL.BO.ParcelInCustomer convertParcelToParcelInCustomer(IDAL.DO.Parcel parcel, int id)
        {
            IBL.BO.ParcelInCustomer parcelInCustomer=new IBL.BO.ParcelInCustomer
            {
                Id = parcel.Id,
                Weight = (IBL.BO.WeightCategories)parcel.Weight,
                Priority = (IBL.BO.Priorities)parcel.Priority,
                ParcelStatus = getParcelStatus(parcel),
                TargetOrSender = new IBL.BO.CustomerInParcel
                {
                    Id = id,
                    Name = GetCustomer(id).Name
                }
            };
            return parcelInCustomer;

        }
        public IEnumerable<IBL.BO.CustomerToList> GetListOfCustomers(Func<IBL.BO.CustomerToList, bool> predicate=null)
        {
            List<IBL.BO.CustomerToList> customers = new List<IBL.BO.CustomerToList>();
            foreach (var dalCustomer in dal.GetCustomers())
            {
                customers.Add(convertCustomerToCustomerToList(dalCustomer));
            }
            if (predicate == null)
                return customers;
            return customers.Where(predicate);
        }
    }
}
