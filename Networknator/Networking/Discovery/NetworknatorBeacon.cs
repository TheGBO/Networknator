using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Networknator.Networking.Discovery
{

    public class NetworknatorBeacon
    {
        private int DiscoveryPort { get; set; } 
        private bool broadcasting = false;

        private UdpClient udp;

        public NetworknatorBeacon(int discoveryPort)
        {
            DiscoveryPort = discoveryPort;
            udp = new UdpClient();
            udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }
        
        public void StartBroadCastLoop(byte[] data, int interval)
        {
            new Thread(() =>
            {
                broadcasting = true;
                while (broadcasting)
                {
                    udp.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, DiscoveryPort));
                    Thread.Sleep(interval);
                }
            }).Start();
        }

        public void Stop()
        {
            broadcasting = false;
        }
    }
}
