namespace Networknator.Networking.Packets
{
    public interface IPacketHandler
    {
        void HandleServer(byte[] data, int senderID);
        void HandleClient(byte[] data);
    }
}