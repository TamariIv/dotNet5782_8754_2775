using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BlObject:IBL.IBL
    {
        public void AddStation(IBL.BO.Station newStation)
        {
            if (newStation.DronesCharging.Count == 0)
            {
                IDAL.DO.Station dalStation = new IDAL.DO.Station
                {
                    Id = newStation.Id,
                    Name = newStation.Name,
                    AvailableChargeSlots = newStation.AvailableChargeSlots,

                };
                dal.AddStation(dalStation);
            }
        }

        public void UpdateStation(IBL.BO.Station newStation)
        {
            IDAL.DO.Station dalStation =  dal.GetStation(newStation.Id);
            if (newStation.Name == "" && (newStation.AvailableChargeSlots).ToString() == "")
                throw new NoUpdateException("no update to station was received\n");
            if (newStation.Name != "")
                dalStation.Name = newStation.Name;
            if ((newStation.AvailableChargeSlots).ToString() == "")
                dalStation.ChargeSlots = newStation.AvailableChargeSlots + newStation.DronesCharging.Count();

            dal.UpdateStation(dalStation);
        }

    }
}
