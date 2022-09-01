using Networknator.Networking;
using Networknator.Networking.Packets;

namespace Example.Packets
{
    public class ChatHandler : PacketHandlerBase<ChatPacket>
    {
        public override async Task ProcessClient(ChatPacket packet)
        {
            System.Console.WriteLine($"{packet.MessageContent}");
            await Task.CompletedTask;
        }

        public override async Task ProcessServer(ChatPacket packet, int senderID)
        {
            string toSend = $"[{WelcomeHandler.users[packet.SenderID]}]::{packet.MessageContent}";
            Server.SendTCPDataToAll<ChatPacket>(new ChatPacket(packet.SenderID, toSend));
            await Task.CompletedTask;
        }
    }
}