using System;
using System.Net;
using System.Net.Sockets;
using Networknator.Networking.Transports;
using Networknator.Utils;

namespace Networknator.Networking
{
    public class ServerConnection
    {
        public static int dataBufferSize = 8192;
        public int id;
        public TCP tcp;

        public ServerConnection(int id) 
        {
            this.id = id;
            tcp = new TCP(id);
        }

        
    }
}