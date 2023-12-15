using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CatFlightGCSNext.Core.FlightControllerConnection
{
    public interface IFlightControllerConnection
    {
        string[] GetPortList();
        void Open();
        void Close();
        void Dispose();
        int Read(byte[] buffer, int offset, int count);
        void Write(byte[] buffer, int offset, int count);
    }
}
