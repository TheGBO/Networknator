using Example.Packets;
using Networknator;
using Networknator.Networking;
using Networknator.Utils;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatApp();
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