using Example.Packets;
using Networknator;
using Networknator.Networking;
using Networknator.Networking.RelayerClient;
using Networknator.Utils;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            RelayerSetup();
            Console.ReadLine();
        }

        private static void RelayerSetup()
        {
            RelayerClient rc = new RelayerClient(7777, "127.0.0.1:8800");
            rc.CreateRoom(true, roomData =>
            {
                Console.WriteLine(roomData.JoinCode);
            });
        }

        private static void ChatApp()
        {
            NetworknatorLogger.StartLogger(Console.WriteLine);
            Console.Write("type \"s\" for server, or anything for client > ");
            string? option = Console.ReadLine();
            if(option?.ToLower() == "s")
            {

                Server.packetHandlers.AutoRegisterHandlers();
                
                Server.OnClientConnected += id =>
                {
                    Server.SendTCPDataTo(id, new WelcomePacket(id, $"noname"));
                };
                Server.OnClientDisconnected += id =>
                {
                    System.Console.WriteLine($"{id} has disconnected");
                };

                Server.Start(8800);
            }
            else
            {

                Client.packetHandlers.AutoRegisterHandlers();

                Client.Start("127.0.0.1", 8800);
                while(Client.IsRunning)
                {
                    if(!Client.IsRunning) break;
                }
            }
            Console.ReadKey();
        }
    }
}