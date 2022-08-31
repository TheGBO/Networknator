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
    }
}