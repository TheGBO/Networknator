using System.Threading.Tasks;

namespace Networknator.Networking.Packets
{
    public abstract class PacketHandlerBase<T> : IPacketHandler where T : class
    {
        public void Handle(byte[] data, int senderID)
        {
            Process(PacketSerializer.Deserialize<T>(data), senderID);
        }

        public abstract void Process(T packet, int senderID);
    }
}