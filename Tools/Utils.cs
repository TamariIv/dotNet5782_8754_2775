using System;
namespace Tools
{
    public static class Utils
    {
        /// <summary>
        /// https://stackoverflow.com/questions/18883601/function-to-calculate-distance-between-two-coordinates
        /// </summary>
        /// <param name="currentLocation"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static double DistanceCalculation(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371.0; // Radius of the earth in km
            var dLat = deg2rad(lat2 - lat1);  // deg2rad below
            var dLon = deg2rad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }

        private static double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}
