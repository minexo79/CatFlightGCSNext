using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;

namespace CatFlightGCSNext.Core.FlightControllerConnection
{
    public class SerialConnection : IFlightControllerConnection
    {
        SerialPort BasePort;
        public Stream BaseStream => BasePort.BaseStream;
        public bool isOpened => BasePort.IsOpen;

        public SerialConnection(string portName, int baudRate)
        {
            BasePort = new SerialPort(portName, baudRate);
        }

        ~SerialConnection()
        {
            Close();
            Dispose();
        }

        public void Dispose()
        {
            BaseStream.Dispose();
            BasePort.Dispose();
        }

        public void Open()
        {
            BasePort.Open();
        }

        public void Close()
        {
            BaseStream.Close();
            BasePort.Close();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, offset, count);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            BaseStream.Write(buffer, offset, count);
        }

        public string[] GetPortList()
        {
            return SerialPort.GetPortNames();
        }
    }
}
