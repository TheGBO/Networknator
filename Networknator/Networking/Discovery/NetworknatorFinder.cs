using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Networknator.Networking.Discovery
{
    public class NetworknatorFinder
    {
        private bool Receiving { get; set; }
        private int Port { get; set; }
        private UdpClient udp;
        public event Action<byte[]> OnReceived;


        public NetworknatorFinder(int port)
        {
            Port = port;
            udp = new UdpClient();
            udp.Client.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void StartReceiving(int interval)
        {
            IPEndPoint recv = new IPEndPoint(0, 0);
            new Thread(() => 
            {
                Receiving = true;
                while (Receiving)
                {
                    byte[] buffer = udp.Receive(ref recv);
                    OnReceived.Invoke(buffer);
                    Thread.Sleep(interval);
                }
            }).Start();
        }

        public void Stop()
        {
            Receiving = false;
        }
    }
}
