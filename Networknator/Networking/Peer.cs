using System;
using System.Net.Sockets;

namespace Networknator.Networking
{
    public abstract class Peer
    {
        public TcpClient tcpSocket;
        public byte[] tcpBuffer;
        public NetworkStream tcpStream;

        public void SendTCP(byte[] data)
        {
            tcpStream.Write(data, 0, data.Length);
        }

        public void ReceiveData(Action<byte[]> OnData)
        {
            int byteLength = tcpStream.Read(tcpBuffer, 0, 4096);

            byte[] data = new byte[byteLength];

            Array.Copy(tcpBuffer, data, byteLength);

            OnData?.Invoke(data);

            Array.Clear(tcpBuffer, 0, 4096);
            Array.Clear(data, 0, byteLength);
        }
    }
}