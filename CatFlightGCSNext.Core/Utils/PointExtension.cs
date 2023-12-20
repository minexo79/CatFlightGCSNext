using System;
using System.Collections.Generic;
using System.Text;
using GMap.NET;

namespace CatFlightGCSNext.Core.Utils
{
    public class PointExtension
    {
        /// <summary>
        /// Calc Distance in M
        /// https://github.com/ArduPilot/MissionPlanner/blob/master/ExtLibs/Utilities/PointLatLngAlt.cs
        /// </summary>
        /// <param name="p1">Marker Point 1</param>
        /// <param name="p2">Marker Point 2</param>
        /// <returns>Distance in M</returns>
        public static double GetDistance(PointLatLng p1, PointLatLng p2)
        {
            // TODO: Move To PointLatLngAlt.cs
            double d = p1.Lat * 0.017453292519943295;
            double num2 = p1.Lng * 0.017453292519943295;
            double num3 = p2.Lat * 0.017453292519943295;
            double num4 = p2.Lng * 0.017453292519943295;
            double num5 = num4 - num2;
            double num6 = num3 - d;
            double num7 =
                Math.Pow(Math.Sin(num6 / 2.0), 2.0) + ((Math.Cos(d) * Math.Cos(num3)) * Math.Pow(Math.Sin(num5 / 2.0), 2.0));
            double num8 = 2.0 * Math.Atan2(Math.Sqrt(num7), Math.Sqrt(1.0 - num7));
            return (6371 * num8) * 1000.0; // M
        }
    }
}
