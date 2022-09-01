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
        public event Action<byte[]> OnDataReceived;


        public ServerConnection(int id)
        {
            ID = id;
            tcpBuffer = new byte[4096];
        }

        public void Connect(TcpClient tcpSocket)
        {
            this.tcpSocket = tcpSocket;
            tcpStream = tcpSocket.GetStream();
            new Thread(ReceiveTCPThread).Start();
            OnDataReceived += (data) => OnDataFromClient?.Invoke(ID, data);
        }

        public void Disconnect()
        {
            tcpBuffer = null;
            tcpSocket = null;
            tcpStream = null;
            OnDisconnected.Invoke(ID);
        }

        

        private void ReceiveTCPThread()
        {
            while (tcpStream != null)
            {
                try
                {
                    ReceiveData(data => OnDataReceived?.Invoke(data));

                }
                catch (Exception e)
                {
                    NetworknatorLogger.Log(LogType.error, e.Message);
                    Disconnect();
                }
            }
        }
    }
}
