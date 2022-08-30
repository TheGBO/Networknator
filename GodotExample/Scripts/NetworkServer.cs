using Godot;
using System;
using Networknator;
using Networknator.Utils;
using Networknator.Networking;
using Networknator.Networking.Packets;

public class NetworkServer : Node
{
	public static NetworkServer instance;
	public Server server = new Server();


	public override void _Ready()
	{
		if(instance == null)
		{
			instance = this;
		}

		NetworknatorLogger.StartLogger(msg => GD.Print(msg));
	}

	public void StartServer()
	{

		server.OnConnection += id => 
		{
			server.SendDataTo(id, new PacketBuilder()
				.Write((int)ServerToClient.welcome)
				.Write(id)
				.Write($"Hello client, your id is {id}")
				.Done());
		};
		server.OnDataReceived += (id, data) => 
		{
			using(PacketReader reader = new PacketReader(data))
			{
				int packetID = reader.ReadInt();
				HandlePacket(packetID, reader, id);
			}
		};
		server.Run(8090);
	}

	public void HandlePacket(int id, PacketReader reader, int senderID)
	{
		switch((ClientToServer)id)
		{
			case ClientToServer.welcomeReceived:
				ServerPlayer.Spawn(reader.ReadInt());
				break;
			case ClientToServer.playerPosition:
				server.SendDataToAll(new PacketBuilder()
					.Write((int)ServerToClient.playerPosition)
					.Write(senderID)
					.Write(reader.ReadFloat())
					.Write(reader.ReadFloat())
					.Done());
				break;
		}
	}
}
