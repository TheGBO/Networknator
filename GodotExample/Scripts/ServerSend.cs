using Godot;
using Networknator.Networking;
using Networknator.Networking.Packets;
using System;

public class ServerSend
{
    private Server Server {get;set;}

    public ServerSend(Server server)
    {
        Server = server;
    }

    public void Welcome(int toID, string message)
    {
        Server.SendDataTo(toID, new PacketBuilder()
            .Write((int)ServerToClient.welcome)
            .Write(message)
            .Done());
    }
}
