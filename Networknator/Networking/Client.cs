using System;
using Networknator.Networking.Transports;

namespace Networknator.Networking
{
    class Client
    {

        public static string IP { get; set; } = "127.0.0.1";
        public static int Port { get; set; } = 8090;
        public static event Action OnConnected;
        public static event Action<byte[]> OnData;
        public static TCP tcp;

        public static void Start(string ip, int port)
        {
            tcp = new TCP();
            tcp.OnConnectedAsClient += () => OnConnected?.Invoke();
            tcp.OnData += (id, data) => OnData?.Invoke(data);
            tcp.ConnectClient(ip, port);
        }
    }
}