using System;
using System.Collections.Generic;
using System.Text;
using CatFlightGCSNext.Core.FlightControllerConnection

namespace CatFlightGCSNext.Core
{
    public class FlightData
    {
        private IFlightControllerConnection connection = null;

        public FlightData()
        {
            connection = new SerialConnection("COM3", 115200);
        }
    }
}
