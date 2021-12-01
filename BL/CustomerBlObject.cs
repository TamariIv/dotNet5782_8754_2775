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

        public IBL.BO.CustomerToList GetCustomerToList(int id)
        {
            IBL.BO.CustomerToList customer = new IBL.BO.CustomerToList();
            foreach (var c in dal.GetCustomers())
            {
                if (c.Id == id)
                {
                    customer = ConvertCustomerToBl(c);
                    return customer;
                }
            }
            throw new NoMatchingIdException($"customer with id {id} doesn't exist\n");
        }
        
        public IBL.BO.CustomerToList ConvertCustomerToBl(IDAL.DO.Customer dalCustomer)
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
            List<IBL.BO.Parcel> send = new List<IBL.BO.Parcel>();
            List<IBL.BO.Parcel> receive = new List<IBL.BO.Parcel>();
            foreach (var parcel in dal.GetParcels())
            {
                if (parcel.SenderId == id)
                    send.Add(ConvertParcelToBl(parcel));
                else if (parcel.TargetId == id)
                    receive.Add(ConvertParcelToBl(parcel));
            }
            IBL.BO.Customer customer = new IBL.BO.Customer
            {
                Id = dalCustomer.Id,
                Name = dalCustomer.Name,
                Phone = dalCustomer.Phone,
                Location = new IBL.BO.Location { Latitude = dalCustomer.Latitude, Longitude = dalCustomer.Longitude },
                Send = send,
                Receive = receive
            };
            return customer;
        }

        public IEnumerable<IBL.BO.CustomerToList> GetListOfCustomers()
        {
            List<IBL.BO.CustomerToList> customers = new List<IBL.BO.CustomerToList>();
            foreach (var dalCustomer in dal.GetCustomers())
            {
                customers.Add(ConvertCustomerToBl(dalCustomer));
            }
            return customers;
        }
    }
}
