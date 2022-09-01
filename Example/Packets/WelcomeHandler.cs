using Example.Singletons;
using Networknator.Networking.Packets;

namespace Example.Packets
{
    public class WelcomeHandler : PacketHandlerBase<WelcomePacket>
    {
        public override void ProcessClient(WelcomePacket packet)
        {
            System.Console.WriteLine($"welcome received, username: {packet.UserName}, my id: ${packet.ID}");
        }

        public override void ProcessServer(WelcomePacket packet, int senderID)
        {
            System.Console.WriteLine($"{packet.UserName} Logged in");
            ServerManager.users.Add(packet.ID, packet.UserName);
        }
    }
}