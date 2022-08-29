using Networknator.Networking.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Networknator.Networking
{
    public class Connection
    {
        public int ID { get; private set; }
        public TcpClient Socket { get; private set; }
        public byte[] Buffer { get; set; }

        private NetworkStream stream;
        private Thread receiveThread;

        public event Action<int, byte[]> OnDataReceived;
        public event Action<int> OnDisconnected;

        public Connection(int id)
        {
            ID = id;
            Buffer = new byte[4096];
        }

        public void Connect(TcpClient socket)
        {
            Socket = socket;
            stream = Socket.GetStream();
            receiveThread = new Thread(ReceiveThread);
            receiveThread.Start();
        }

        public void Disconnect()
        {
            Socket.Close();
            Buffer = null;
            Socket = null;
            stream = null;
            OnDisconnected.Invoke(ID);
        }

        public void Send(byte[] data)
        {
            byte[] packetData = data;
            stream.Write(packetData, 0, packetData.Length);
        }

        private void ReceiveThread()
        {
            while (stream != null)
            {
                try
                {
                    int byteLength = stream.Read(Buffer, 0, 4096);

                    byte[] data = new byte[byteLength];

                    Array.Copy(Buffer, data, byteLength);

                    OnDataReceived.Invoke(ID, data);

                    Array.Clear(Buffer, 0, 4096);
                    Array.Clear(data, 0, byteLength);
                }
                catch (Exception)
                {

                    Disconnect();
                }
            }
        }
    }
}
