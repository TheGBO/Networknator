namespace Networknator.Networking.Packets
{
    public interface IPacketHandler
    {
        void Handle(byte[] data, int senderID);
    }
}