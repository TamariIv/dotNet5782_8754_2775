using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BlObject : IBL
    {
        public void UpdateCustomer(BO.Customer newCustomer)
        {
            try
            {
                DO.Customer dalCustomer = dal.GetCustomer(newCustomer.Id);
                if (newCustomer.Name == "" && newCustomer.Phone == "")
                    throw new BO.NoUpdateException("no update to customer was received\n");
                if (newCustomer.Name != "")
                    dalCustomer.Name = newCustomer.Name;
                if (newCustomer.Phone != "")
                    dalCustomer.Phone = newCustomer.Phone;
                dal.UpdateCustomer(dalCustomer);
            }
            catch (DO.NoMatchingIdException)
            {
                throw new BO.NoMatchingIdException($"customer with id {newCustomer.Id} doesn't exist !!");
            }
        }

        public void AddCustomer(BO.Customer newCustomer)
        {
            try
            {
                DO.Customer dalCustomer = new DO.Customer
                {
                    Id = newCustomer.Id,
                    Name = newCustomer.Name,
                    Phone = newCustomer.Phone,
                    Longitude = newCustomer.Location.Longitude,
                    Latitude = newCustomer.Location.Latitude

                };
                dal.AddCustomer(dalCustomer);
            }
            catch (DO.IdAlreadyExistsException)
            {
                throw new BO.IdAlreadyExistsException($"drone with id {newCustomer.Id} already exists !!");
            }
        }

        private BO.CustomerToList convertCustomerToCustomerToList(DO.Customer dalCustomer)
        {
            int sentAndDelivered = 0, sentAndNotDelivered = 0, received = 0, inDeliveryToCustomer = 0;
            foreach (var p in dal.GetParcels())
            {
                if (p.SenderId == dalCustomer.Id)
                {
                    if (getParcelStatus(p) == BO.ParcelStatus.Delivered)
                        sentAndDelivered++;
                    else sentAndNotDelivered++;
                }
                else if (p.TargetId == dalCustomer.Id)
                {
                    if (getParcelStatus(p) == BO.ParcelStatus.Delivered)
                        received++;
                    else
                        inDeliveryToCustomer++;
                }
            }
            BO.CustomerToList blCustomer = new BO.CustomerToList
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

        public BO.Customer GetCustomer(int id)
        {
            DO.Customer dalCustomer = dal.GetCustomer(id);
            List<DO.Parcel> parcelsOfsender = dal.GetParcels().ToList().FindAll(p => p.SenderId == id);
            List<DO.Parcel> parcelsOfreceiver = dal.GetParcels().ToList().FindAll(p => p.TargetId == id);
            BO.Customer customer = new BO.Customer
            {
                Id = dalCustomer.Id,
                Name = dalCustomer.Name,
                Phone = dalCustomer.Phone,
                Location = new BO.Location { Latitude = dalCustomer.Latitude, Longitude = dalCustomer.Longitude },
                Send = convertParcelsToParcelsCustomer(parcelsOfsender, id),
                Receive = convertParcelsToParcelsCustomer(parcelsOfreceiver, id)
            };
            return customer;
        }

        private List<BO.ParcelInCustomer> convertParcelsToParcelsCustomer(List<DO.Parcel> parcelsOfSender, int id)
        {
            List<BO.ParcelInCustomer> parcels = new List<BO.ParcelInCustomer>();
            foreach (var item in parcelsOfSender)
            {
                parcels.Add(convertParcelToParcelInCustomer(item, id));
            }
            return parcels;
        }

        private BO.ParcelInCustomer convertParcelToParcelInCustomer(DO.Parcel parcel, int id)
        {
            BO.ParcelInCustomer parcelInCustomer = new BO.ParcelInCustomer
            {
                Id = parcel.Id,
                Weight = (BO.WeightCategories)parcel.Weight,
                Priority = (BO.Priorities)parcel.Priority,
                ParcelStatus = getParcelStatus(parcel),
                TargetOrSender = new BO.CustomerInParcel
                {
                    Id = id,
                    Name = dal.GetCustomer(id).Name
                }
            };
            return parcelInCustomer;

        }
        public IEnumerable<BO.CustomerToList> GetListOfCustomers()
        {
            List<BO.CustomerToList> customers = new List<BO.CustomerToList>();
            foreach (var dalCustomer in dal.GetCustomers())
            {
                customers.Add(convertCustomerToCustomerToList(dalCustomer));
            }
            return customers;
        }
        /// <summary>
        /// help function that returns a list of customers that have parcels which still weren't delivered
        /// </summary>
        private IEnumerable<DO.Customer> GetCustomersWithParcels()
        {
            List<DO.Customer> clients = new List<DO.Customer>();
            foreach (var customer in dal.GetCustomers())
            {
                foreach (var parcel in dal.GetParcels())
                {
                    if (customer.Id == parcel.TargetId && parcel.Delivered != null)
                    {
                        DO.Customer client = dal.GetCustomer(parcel.TargetId);
                        clients.Add(client);
                    }
                }
            }
            return clients;
        }
    }
}
