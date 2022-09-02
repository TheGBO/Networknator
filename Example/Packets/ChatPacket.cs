using MessagePack;

namespace Example.Packets
{
    [MessagePackObject]
    public class ChatPacket
    {
        [Key(0)] public int SenderID { get; set; }
        [Key(1)] public string MessageContent { get; set; }

        public ChatPacket(int senderID, string messageContent)
        {
            SenderID = senderID;
            MessageContent = messageContent;
        }

        public ChatPacket()
        {
            SenderID = -1;
            MessageContent = "";
        }
    }
}
