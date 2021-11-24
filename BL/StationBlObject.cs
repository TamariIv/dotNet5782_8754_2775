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
                    AvailableChargeSlots = newStation.AvailableAvailableChargeSlots,

                };
                dal.AddStation(dalStation);
            }
        }

    }
}
