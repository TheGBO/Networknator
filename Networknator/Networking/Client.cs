using Networknator.Networking.Packets;
using Networknator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Networknator.Networking
{
    public class Client : Peer
    {
        public string ConnectionString { get; private set; }
        public bool IsRunning { get; private set; }

        public event Action OnConnected;
        public event Action OnConnectionFailed;
        public event Action<string> OnDisconnected;
        public event Action<byte[]> OnDataReceived;




        public void Run(string connectionString)
        {
            if (IsRunning) return;
            tcpSocket = new TcpClient
            {
                ReceiveBufferSize = 4096,
                SendBufferSize = 4096
            };
            tcpBuffer = new byte[4096];

            string[] parsedConn = connectionString.Split(':');
            try
            {
                IPEndPoint iP = new IPEndPoint(IPAddress.Parse(parsedConn[0]), int.Parse(parsedConn[1]));
                tcpSocket.Connect(iP);
                tcpStream = tcpSocket.GetStream();
            }
            catch (Exception e)
            { 
                NetworknatorLogger.Log(LogType.error, e.Message);
                OnConnectionFailed?.Invoke();
                return; 
            }
            IsRunning = true;
            OnConnected?.Invoke();
            NetworknatorLogger.Log(LogType.normal, $"Client connected successfully at {connectionString}");
            new Thread(ReceiveThread).Start();
        }

        private void ReceiveThread()
        {
            while (IsRunning)
            {
                try
                {
                    BufferUtils.ReceiveData(tcpStream, ref tcpBuffer, data => OnDataReceived.Invoke(data));
                }
                catch(Exception e)
                {
                    Stop(e.Message);
                    NetworknatorLogger.Log(LogType.error, e.ToString());
                }
                
            }
        }

        public void Stop(string reason = "no reason specified")
        {
            IsRunning = false;
            OnDisconnected?.Invoke(reason);
        }
    }
}
