using System;
using System.Collections.Generic;
using System.Text;

namespace CatFlightGCSNext.Core
{
    public class FlightParameters
    {
        public static string[]? FlightMode { get; set; }

        public static void Init()
        {
            FlightMode = Utils.YamlReader.ReadArray("FlightParameters.yaml", "FlightMode");
        }
    }
}
