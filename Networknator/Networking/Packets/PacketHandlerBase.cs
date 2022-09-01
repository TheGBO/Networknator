using System.Threading.Tasks;

namespace Networknator.Networking.Packets
{
    public abstract class PacketHandlerBase<T> : IPacketHandler where T : class
    {
        public async Task HandleClient(byte[] data)
        {
            await ProcessClient(PacketSerializer.Deserialize<T>(data));
        }

        public async Task HandleServer(byte[] data, int senderID)
        {
            await ProcessServer(PacketSerializer.Deserialize<T>(data), senderID);
        }


        public abstract Task ProcessClient(T packet);
        public abstract Task ProcessServer(T packet, int senderID);
    }
}