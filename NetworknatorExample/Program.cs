using Networknator.Networking;
using Networknator.Networking.Packets;
using Networknator.Utils;
using System;

namespace NetworknatorExample
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworknatorLogger.StartLogger(Console.WriteLine, Console.WriteLine, Console.WriteLine);

            Server server = new Server();
            server.Run(8900);

            server.OnConnection += id =>
            {
                server.SendDataTo(new PacketBuilder()
                    .Write(0)
                    .Write($"you are connected: {id}")
                    .Done(), id);
            };

            server.OnDataReceived += (id, data) =>
            {
                using (PacketReader reader = new PacketReader(data))
                {
                    Console.WriteLine($"[server] client of {id} sent data");
                    Console.WriteLine($"[server] packet id: {reader.ReadInt()}");
                    Console.WriteLine($"[server] packet message: {reader.ReadString()}");
                }
            };


            Client client = new Client();
            client.OnDataReceived += (data) =>
            {
                using (PacketReader reader = new PacketReader(data))
                {
                    Console.WriteLine("[client] server sent data");
                    Console.WriteLine($"[client] packet id: {reader.ReadInt()}");
                    Console.WriteLine($"[client] packet message: {reader.ReadString()}");
                }
            };
            client.OnConnected += () =>
            {
                client.Send(new PacketBuilder()
                    .Write(0)
                    .Write("Hello :)")
                    .Done());
            };

            client.Run("127.0.0.1:8900");


            Console.ReadLine();
        }
    }
}
