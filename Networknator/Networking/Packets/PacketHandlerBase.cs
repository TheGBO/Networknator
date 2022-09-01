using System.Threading.Tasks;

namespace Networknator.Networking.Packets
{
    public abstract class PacketHandlerBase<T> : IPacketHandler where T : class
    {
        public void HandleClient(byte[] data)
        {
            ProcessClient(PacketSerializer.Deserialize<T>(data));
        }

        public void HandleServer(byte[] data, int senderID)
        {
            ProcessServer(PacketSerializer.Deserialize<T>(data), senderID);
        }


        public abstract void ProcessClient(T packet);
        public abstract void ProcessServer(T packet, int senderID);
    }
}