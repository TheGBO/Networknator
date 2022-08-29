using Networknator.Networking;
using Networknator.Networking.Packets;
using System;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();

            Console.WriteLine("initializing server");

            server.OnConnection += id =>
            {
                Console.WriteLine($"client of id {id} has connected");
                server.SendDataTo(id, new PacketBuilder()
                    .Write(0)
                    .Write($"hello client, your ID is {id}")
                    .Done());
            };

            server.OnDataReceived += (id, data) =>
            {
                Console.WriteLine($"{id} sent data");
                using (PacketReader reader = new PacketReader(data))
                {
                    Console.WriteLine($"Packet ID is: {reader.ReadInt()}");
                    Console.WriteLine($"Packet message is: {reader.ReadString()}");
                }
            };

            server.Run(8090);

            Console.ReadKey();
        }
    }
}
