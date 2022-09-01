using System;
using Networknator.Networking.Packets;
using Networknator.Networking.Transports;

namespace Networknator.Networking
{
    class Client
    {

        public static string IP { get; set; } = "127.0.0.1";
        public static int Port { get; set; } = 8090;
        public static event Action OnConnected;
        public static bool IsRunning { get; set; } = false;
        public static TCP tcp;
        public static PacketHandlers packetHandlers = new PacketHandlers();

        public static void Start(string ip, int port)
        {
            tcp = new TCP();
            tcp.OnConnectedAsClient += () => OnConnected?.Invoke();
            tcp.OnData += (id, data) => packetHandlers.HandlePacket(id, data);
            tcp.ConnectClient(ip, port);
            IsRunning = true;
        }

        public static void Stop()
        {
            tcp = null;
            IsRunning = false;
        }

        public static void SendTCPData<T>(T packet) => SendTCPData(PacketSerializer.Serialize<T>(packet));

        public static void SendTCPData(byte[] data)
        {
            tcp.Send(data);
        }
    }
}