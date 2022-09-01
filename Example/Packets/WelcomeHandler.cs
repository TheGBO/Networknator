using Networknator.Networking;
using Networknator.Networking.Packets;

namespace Example.Packets
{
    public class WelcomeHandler : PacketHandlerBase<WelcomePacket>
    {
        public static Dictionary<int, string> users = new Dictionary<int, string>();
        public override Task ProcessClient(WelcomePacket packet)
        {
            Console.WriteLine($"welcome received from server, your id is: ${packet.ID}");
            Console.Write("Enter an username:");
            string username = Console.ReadLine() ?? "guest";
            Console.WriteLine(username);
            WelcomePacket toSend = new WelcomePacket(packet.ID, username);
            Client.SendTCPData(toSend);
            return Task.CompletedTask;
        }

        public override Task ProcessServer(WelcomePacket packet, int senderID)
        {
            Console.WriteLine($"{packet.UserName} Logged in");
            users.Add(packet.ID, packet.UserName);
            return Task.CompletedTask;
        }
    }
}