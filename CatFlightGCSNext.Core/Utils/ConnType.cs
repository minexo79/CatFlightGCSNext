using System;
using System.Collections.Generic;
using System.Text;

namespace CatFlightGCSNext.Core.Utils
{
    public enum ConnType
    {
        Serial,     // 0
        UDP,
        UDPClient,
        TCP,
        Unknown = 99
    };
}
