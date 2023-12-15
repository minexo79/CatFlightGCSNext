using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFlightGCSNext.UI.Utils
{
    public class LatLngConvert
    {
        public static void ConvertToDecimal6(PointLatLng pointLatLng, out string lat, out string lng)
        {
            lat = "";
            lng = "";

            lat += (pointLatLng.Lat > 0) ? "N" : "S";
            lng += (pointLatLng.Lng > 0) ? "E" : "W";

            lat += pointLatLng.Lat.ToString("#.000000");
            lng += pointLatLng.Lng.ToString("#.000000");
        }
    }
}
