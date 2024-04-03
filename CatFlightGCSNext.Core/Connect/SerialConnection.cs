using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;

namespace CatFlightGCSNext.Core.Connect
{

    /// <summary>
    /// The read buffer class is used to store the data received from the serial port.
    /// </summary>
    internal class ReadBuffer
    {
        internal object locker { get; set; }            // Lock object for thread safety
        internal Queue<byte> bufferReadQueue { get; }   // Queue to store the received data
        internal Queue<int> bufferLngQueue { get; }     // Queue to store the length of the received data


        /// <summary>
        /// Initializes a new instance of the <see cref="ReadBuffer"/> class.
        /// </summary>
        public ReadBuffer()
        {
            locker = new object();

            bufferReadQueue = new Queue<byte>();
            bufferLngQueue = new Queue<int>();
        }
    }

    public class SerialConnection : IConnection
    {
        SerialPort BasePort;
        BackgroundWorker bgwCheckConnected;

        private ReadBuffer readBuffer = new ReadBuffer();

        private const byte MAVLINK_STX_V2 = 0xFD;           // MAVLink 2.0 start of frame
        private const byte MAVLINK_STX = 0xFE;              // MAVLink 1.0 start of frame

        public event EventHandler DeviceDisconnected;       // Event to notify when the device is disconnected

        public Stream BaseStream => BasePort.BaseStream;
        public bool isOpened => BasePort.IsOpen;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialConnection"/> class.
        /// </summary>
        /// <param name="portName">The Name Of The Serial Port. (eg: COM3)</param>
        /// <param name="baudRate">The Speed Of Serial Port Connection. (Format: bps)</param>
        public SerialConnection(string portName, int baudRate)
        {
            BasePort = new SerialPort(portName, baudRate);
        }

        ~SerialConnection()
        {
            Close();
            Dispose();
        }

        /// <summary>
        /// Disposes the serial port and the base stream.
        /// </summary>
        public void Dispose()
        {
            if (BasePort != null)
            {
                BaseStream.Dispose();
                BasePort.Dispose();

                GC.SuppressFinalize(this);
            }
        }


        /// <summary>
        /// Background worker to check if the device is connected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgwCheckConnected_DoWork(object? sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(50);

                if (!BasePort.IsOpen)
                {
                    DeviceDisconnected?.Invoke(this, new EventArgs());
                    break;
                }
            }
        }


        /// <summary>
        /// Event handler for the data received event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BasePort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            lock (readBuffer.locker)
            {
                SerialPort sp = (SerialPort)sender;
                readBuffer.bufferLngQueue.Enqueue(sp.BytesToRead);

                for (int i = 0; i < sp.BytesToRead; i++)
                    readBuffer.bufferReadQueue.Enqueue((byte)sp.ReadByte());
            }
            
        }

        /// <summary>
        /// Opens the serial port and starts the background worker to check if the device is connected.
        /// </summary>
        public void Open()
        {
            BasePort.Open();

            bgwCheckConnected = new BackgroundWorker();
            bgwCheckConnected.DoWork += BgwCheckConnected_DoWork;


            BasePort.DataReceived += BasePort_DataReceived;
        }


        /// <summary>
        /// Reads the data from the read buffer.
        /// </summary>
        /// <returns>The Byte Data From The Read Buffers.</returns>
        public byte[] Read()
        {
            int length = readBuffer.bufferLngQueue.Dequeue();

            if (length > 0)
            {
                byte[] buffer = new byte[length];

                for (int i = 0; i < length; i++)
                    buffer[i] = readBuffer.bufferReadQueue.Dequeue();

                if (buffer[0] == MAVLINK_STX_V2 || buffer[0] == MAVLINK_STX)
                    return buffer;
            }

            return null;
        }

        /// <summary>
        /// Writes the data to the serial port.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="count"></param>
        public void Write(byte[] buffer, int count)
        {
            BasePort.Write(buffer, 0, count);
        }

        /// <summary>
        /// Closes the serial port.
        /// </summary>
        public void Close()
        {
            BaseStream.Close();
            BasePort.Close();
        }

        /// <summary>
        /// Checks if the serial port is opened.
        /// </summary>
        /// <returns></returns>
        public bool IsOpened()
        {
            return (BasePort != null) ? BasePort.IsOpen : false;
        }

        /// <summary>
        /// Get the list of available serial ports.
        /// </summary>
        /// <returns></returns>
        public static string[] GetPortList()
        {
            return SerialPort.GetPortNames();
        }
    }
}
