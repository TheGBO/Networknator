using Networknator.Networking;
using Networknator.Networking.Packets;
using System;

namespace Networknator.Examples.Simple
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            Client client = new Client();


            server.OnDataReceived += (id, data) =>
            {
                using (PacketReader reader = new PacketReader(data))
                {
                    Console.WriteLine("[Server] :: " + reader.ReadString());
                }
                server.SendDataTo(id, new PacketBuilder()
                    .Write("pong")
                    .Done());

            };

            server.Run(29509);



            client.OnConnected += () =>
            {
                client.Send(new PacketBuilder()
                    .Write("ping")
                    .Done());
            };

            client.OnDataReceived += (data) =>
            {
                using (PacketReader reader = new PacketReader(data))
                {
                    Console.WriteLine("[Client] :: " + reader.ReadString());
                }
            };
            client.Run("127.0.0.1:29509");
            
            Console.ReadKey();
        }
    }
}
