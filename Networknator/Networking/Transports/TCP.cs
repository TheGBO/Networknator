using System;
using System.Net.Sockets;
using Networknator.Utils;

namespace Networknator.Networking.Transports
{
    public class TCP
    {
        public TcpClient socket;
        public static int dataBufferSize = 4096;

        private readonly int id;
        private NetworkStream stream;
        private byte[] recvBuffer;
        public event Action<int, byte[]> OnData;
        public event Action OnConnectedAsClient;
        public event Action<int> OnDisconnection;

        public TCP(int id)
        {
            this.id = id;
        }

        public TCP()
        {
            this.id = 0;
        }

        public void ConnectServer(TcpClient socket)
        {
            this.socket = socket;
            this.socket.ReceiveBufferSize = dataBufferSize;
            this.socket.SendBufferSize = dataBufferSize;

            stream = this.socket.GetStream();
            recvBuffer = new byte[dataBufferSize];

            stream.BeginRead(recvBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        public void ConnectClient(string ip, int port)
        {
            socket = new TcpClient
            {
                SendBufferSize = dataBufferSize,
                ReceiveBufferSize = dataBufferSize,
            };
            recvBuffer = new byte[dataBufferSize];
            socket.BeginConnect(ip, port, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult result)
        {
            socket.EndConnect(result);

            if(!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();
            stream.BeginRead(recvBuffer, 0, dataBufferSize, ReceiveCallback, null);
            OnConnectedAsClient?.Invoke();
        }

        public void Send(byte[] data)
        {
            if(socket != null)
            {
                stream.BeginWrite(data, 0, data.Length, null, null);
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteSize = stream.EndRead(result);
                if(byteSize <= 0)
                {
                    return;
                }
                byte[] data = new byte[byteSize];

                Array.Copy(recvBuffer, data, byteSize);

                OnData?.Invoke(id, data);

                Array.Clear(recvBuffer, 0, dataBufferSize);
                
                stream.BeginRead(recvBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            socket.Close();
            stream = null;
            recvBuffer = null;
            socket = null;
            OnDisconnection?.Invoke(id);
        }

    }
}