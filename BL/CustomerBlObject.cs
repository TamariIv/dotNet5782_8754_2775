using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BlObject:IBL.IBL
    {
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

        public void UpdateCustomer(IBL.BO.Customer newCustomer)
        {
            IDAL.DO.Customer dalCustomer = dal.GetCustomer(newCustomer.Id);
            if (newCustomer.Name != "")
                dalCustomer.Name = newCustomer.Name;
            if (newCustomer.Phone != "")
                dalCustomer.Phone = newCustomer.Phone;
            dal.UpdateCustomer(dalCustomer);
        }
    }
}
