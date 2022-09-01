using System;
using MessagePack;
using Networknator.Networking;
using Networknator.Networking.Packets;



[MessagePackObject]
public class CustomPacket
{
    [Key(0)]
    public string Message { get; set; }

    [Key(1)]
    public int Sender { get; set; }

    public CustomPacket(string message, int sender)
    {
        Message = message;
        Sender = sender;
    }

    public CustomPacket()
    {

    }
}

public class CustomPacketHandler : PacketHandlerBase<CustomPacket>
{
    public override void Process(CustomPacket packet, int senderID)
    {
        Console.WriteLine($"{packet.Message}");
        if(Server.IsRunning)
        {
            Server.SendTCPDataTo<CustomPacket>(senderID, new CustomPacket($"Hello client of id {senderID}", senderID));
        }
    }
}
