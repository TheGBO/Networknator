using System.Threading.Tasks;

namespace Networknator.Networking.Packets
{
    public interface IPacketHandler
    {
        Task HandleServer(byte[] data, int senderID);
        Task HandleClient(byte[] data);
    }
}