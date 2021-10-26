using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
      public struct DroneCharge
        {
            public int DroneId;
            public int StationId;

            public override string ToString()
            {
                return string.Format("Id of drone: {0}\n Id of station: {1}\n" , DroneId , StationId);
            }
        }

    }

}
