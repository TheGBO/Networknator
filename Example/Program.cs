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
            NetworknatorLogger.StartLogger(Console.WriteLine);

            Server.packetHandlers.Register<WelcomeHandler, WelcomePacket>();
            Server.OnClientConnected += id =>
            {
                Server.SendTCPDataTo(id, new WelcomePacket(id, $"Hello Client, your id is {id}"));
            };

            Server.Start(8800);

        }
    }
}