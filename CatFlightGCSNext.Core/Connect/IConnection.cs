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
        int Read(byte[] buffer, int offset, int count);
        void Write(byte[] buffer, int offset, int count);
    }
}
