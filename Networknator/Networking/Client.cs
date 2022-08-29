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
    public class Client
    {
        private TcpClient socket;
        public string ConnectionString { get; private set; }
        public bool IsRunning { get; private set; }
        private NetworkStream stream;
        private byte[] buffer;

        public event Action<byte[]> OnDataReceived;
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
                    int byteLength = stream.Read(buffer, 0, 4096);
                    byte[] data = new byte[byteLength];

                    Array.Copy(buffer, data, byteLength);

                    OnDataReceived?.Invoke(data);

                    Array.Clear(buffer, 0, 4096);
                    Array.Clear(data, 0, byteLength);

                }
                catch(Exception e)
                {
                    Stop(e.Message);
                }
                
            }
        }

        public void Stop(string reason)
        {
            IsRunning = false;
            OnDisconnected?.Invoke(reason);
        }
    }
}
