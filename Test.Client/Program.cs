using System;
using System.Collections.Generic;
using Networknator.Networking;
using Networknator.Networking.Packets;
using Networknator.Utils;

namespace TestClient
{
    class Program
    {
        public delegate void PacketHandler(PacketReader reader);

        static void Main(string[] args)
        {
            NetworknatorLogger.StartLogger(Console.WriteLine, Console.WriteLine, Console.WriteLine);

            Client client = new Client();

            client.OnConnected += () =>
            {
                Console.WriteLine("connection success");
            };

            client.OnDataReceived += data =>
            {
                using(PacketReader reader = new PacketReader(data))
                {
                    Console.WriteLine($"Packet ID is: {reader.ReadInt()}");
                    Console.WriteLine($"Packet message is: {reader.ReadString()}");
                }

                client.Send(new PacketBuilder()
                    .Write(0)
                    .Write("Hello Server!")
                    .Done());
            };

            client.Run("127.0.0.1:8090");
            Console.ReadKey();
        }
    }
}
