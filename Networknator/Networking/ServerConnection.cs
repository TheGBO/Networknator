using Networknator.Networking.Packets;
using Networknator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Networknator.Networking
{
    /// <summary>
    /// Represents a connection from a <see cref="Client"/> in the <see cref="Server"/>
    /// </summary>
    public class ServerConnection : Peer
    {
        /// <summary>
        /// The ID of the connection
        /// </summary>
        public int ID { get; private set; }

        public event Action<int> OnDisconnected;
        public event Action<int, byte[]> OnDataFromClient;

        public ServerConnection(int id)
        {
            ID = id;
            buffer = new byte[4096];
        }

        public void Connect(TcpClient socket)
        {
            this.socket = socket;
            stream = socket.GetStream();
            new Thread(ReceiveThread).Start();
            OnDataReceived += (data) => OnDataFromClient?.Invoke(ID, data);
        }

        public void Disconnect()
        {
            socket.Close();
            buffer = null;
            socket = null;
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
                    ReceiveData();
                }
                catch (Exception e)
                {
                    NetworknatorLogger.Log(LogType.error, e.Message);
                    Disconnect();
                }
            }
            Disconnect();
        }
    }
}
