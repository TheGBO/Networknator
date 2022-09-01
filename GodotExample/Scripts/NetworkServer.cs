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
			server.SendTCPDataTo(id, new PacketBuilder()
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
		server.OnDisconnection += id =>
		{
			if(ServerPlayer.list.TryGetValue(id, out ServerPlayer serverPlayer))
			{
				serverPlayer.Destroy();
				GD.Print($"{id} has disconnected");
			}
			server.SendTCPDataToAll(new PacketBuilder()
				.Write((int)ServerToClient.playerLeft)
				.Write(id)
				.Done());
		};
		server.Run(8090);
	}

	public void HandlePacket(int id, PacketReader reader, int senderID)
	{
		switch((ClientToServer)id)
		{
			case ClientToServer.welcomeReceived:
				int toSpawn = reader.ReadInt();
				ServerPlayer.Spawn(toSpawn);
				GD.Print($"server will spawn {toSpawn}");
				break;
			case ClientToServer.playerPosition:
				server.SendTCPDataToAll(new PacketBuilder()
					.Write((int)ServerToClient.playerPosition)
					.Write(senderID)
					.Write(reader.ReadFloat())
					.Write(reader.ReadFloat())
					.Done());
				break;
		}
	}
}
