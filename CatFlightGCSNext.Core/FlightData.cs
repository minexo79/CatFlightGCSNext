using System;
using System.Collections.Generic;
using System.Text;
using CatFlightGCSNext.Core.Connect;
using CatFlightGCSNext.Core.Utils;

namespace CatFlightGCSNext.Core
{
    public class FlightData
    {
        public static IConnection connection = null;

        ConnType connType = ConnType.Unknown;

        public FlightData(ConnType _type)
        {
            connType = _type;
        }
    }
}
