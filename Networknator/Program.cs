using System;
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

            Server.OnData += (id, data) =>
            {
                Console.WriteLine($"{id} sent data");
                using(PacketReader reader = new PacketReader(data))
                {
                    Console.WriteLine(reader.ReadString());
                }
            };

            Client.OnConnected += () =>
            {
                Client.tcp.Send(new PacketBuilder()
                    .Write("Hello")
                    .Done());
            };
            Server.Start(28090);


            Client.Start("127.0.0.1", 28090);
            Console.ReadKey();
        }
    }
}