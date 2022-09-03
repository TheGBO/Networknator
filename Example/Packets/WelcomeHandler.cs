using Networknator.Networking;
using Networknator.Networking.Packets;

namespace Example.Packets
{
    [RegisterHandler(typeof(WelcomePacket))]
    public class WelcomeHandler : PacketHandlerBase<WelcomePacket>
    {
        public static Dictionary<int, string> users = new Dictionary<int, string>();
        public override Task ProcessClient(WelcomePacket packet)
        {
            Console.WriteLine($"welcome received from server, your id is: ${packet.ID}");
            Console.Write("Enter an username:");
            string username = Console.ReadLine() ?? "guest";
            WelcomePacket toSend = new WelcomePacket(packet.ID, username);
            Client.SendTCPData(toSend);
            //start chat
            new Thread(() => 
            { 
                Console.Write("Start typing your Messages >> ");
                while(Client.IsRunning)
                {
                    string message = Console.ReadLine() ?? "";
                    if(message.Trim() != "")
                    {
                        Client.SendTCPData<ChatPacket>(new ChatPacket(packet.ID, message));
                    }
                }
            }).Start();
            
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