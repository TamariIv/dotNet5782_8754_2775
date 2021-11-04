using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class Tools
    {
        public static string longSaxgesimal(double longitude)
        {
            double absValOfDegree = Math.Abs(longitude);
            double minute = (absValOfDegree - (int)absValOfDegree) * 60;
            return string.Format("{0}°{1}\' {2}\"{3}", (int)longitude, (int)(minute), Math.Round((minute - (int)minute) * 60), longitude < 0 ? "S" : "N");
        }

        public static string latSaxgesimal(double latitude)
        {
            double absValOfDegree = Math.Abs(latitude);
            double minute = (absValOfDegree - (int)absValOfDegree) * 60;
            return string.Format("{0}°{1}\' {2}\"{3}", (int)latitude, (int)(minute), Math.Round((minute - (int)minute) * 60), latitude < 0 ? "W" : "E");
        }
    }
}
