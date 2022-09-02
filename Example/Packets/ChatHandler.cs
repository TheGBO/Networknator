using Networknator.Networking;
using Networknator.Networking.Packets;

namespace Example.Packets
{
    public class ChatHandler : PacketHandlerBase<ChatPacket>
    {
        public override Task ProcessClient(ChatPacket packet)
        {
            System.Console.WriteLine($"\n{packet.MessageContent}");
            return Task.CompletedTask;
        }

        public override Task ProcessServer(ChatPacket packet, int senderID)
        {
            string toSend = $"[{WelcomeHandler.users[packet.SenderID]}]::{packet.MessageContent}";
            Console.WriteLine(toSend);
            Server.SendTCPDataToAll<ChatPacket>(new ChatPacket(packet.SenderID, toSend));
            return Task.CompletedTask;
        }
    }
}