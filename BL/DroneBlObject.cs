using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BlObject:IBL.IBL
    {
        public void rechargeDrone(IBL.BO.Drone drone)
        {
            if (drone.DroneStatus == IBL.BO.Enums.DroneStatus.Available)
            {

            }
            else throw new ImpossibleOprationException("Drone can't be sent to recharge");
        }
    }
}
