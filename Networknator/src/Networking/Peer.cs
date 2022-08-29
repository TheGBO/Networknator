using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Networknator.Networking
{
    public abstract class Peer
    {
        public TcpClient socket;
        public event Action<byte[]> OnDataReceived;
        protected byte[] buffer;
        protected NetworkStream stream;

        public void ReceiveData()
        {
            int byteLength = stream.Read(buffer, 0, 4096);

            byte[] data = new byte[byteLength];

            Array.Copy(buffer, data, byteLength);

            OnDataReceived.Invoke(data);

            Array.Clear(buffer, 0, 4096);
            Array.Clear(data, 0, byteLength);
        }
    }
}
