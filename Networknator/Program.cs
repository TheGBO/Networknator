using System;
using System.Text;
using MessagePack;
using Networknator.Networking;
using Networknator.Networking.Packets;
using Networknator.Utils;

namespace Networknator
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworknatorLogger.StartLogger(Console.WriteLine);
            Server.packetHandlers.Register<CustomPacketHandler, CustomPacket>();
            Server.Start(28090);
            Server.OnClientConnected += id =>
            {
                System.Console.WriteLine(id);
            };

            Client.packetHandlers.Register<CustomPacketHandler, CustomPacket>();
            Client.Start("127.0.0.1", 28090);
            Client.SendTCPData<CustomPacket>(new CustomPacket("Hello server", 1));
            Console.ReadKey();
        }
    }
}