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

        public void Run(string connectionString)
        {
            if (IsRunning) return;
            socket = new TcpClient
            {
                ReceiveBufferSize = 4096,
                SendBufferSize = 4096
            };
            buffer = new byte[4096];

            string[] parsedConn = connectionString.Split(':');
            try
            {
                IPEndPoint iP = new IPEndPoint(IPAddress.Parse(parsedConn[0]), int.Parse(parsedConn[1]));
                socket.Connect(iP);
                stream = socket.GetStream();
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

        public void Send(byte[] data)
        {
            try
            {
                stream.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                Stop(e.Message);
            }
        }

        private void ReceiveThread()
        {
            while (IsRunning)
            {
                try
                {
                    ReceiveData();
                }
                catch(Exception e)
                {
                    Stop(e.Message);
                    NetworknatorLogger.Log(LogType.error, e.Message);
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
