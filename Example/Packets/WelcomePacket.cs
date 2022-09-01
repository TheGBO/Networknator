using MessagePack;

namespace Example.Packets
{
    [MessagePackObject]
    public class WelcomePacket
    {
        [Key(0)] public int ID { get; set; }
        [Key(1)] public string UserName { get; set; }

        public WelcomePacket(int id, string username)
        {
            ID = id;
            UserName = username;
        }

        public WelcomePacket()
        {
            ID = -1;
            UserName = "";
        }
    }
}