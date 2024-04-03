using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CatFlightGCSNext.Core.Connect
{
    public interface IConnection
    {
        void Open();
        void Close();
        bool IsOpened();
        void Dispose();
        byte[] Read();
        void Write(byte[] buffer, int count);
    }
}
