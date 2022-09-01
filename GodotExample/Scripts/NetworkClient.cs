using Godot;
using Networknator.Networking;
using Networknator.Networking.Packets;
using Networknator.Utils;
using System;

public class NetworkClient : Node
{
	public static NetworkClient instance;
	public Client client = new Client();
	public static int localID;

	public override void _Ready()
	{
		if(instance == null)
		{
			instance = this;
		}

		NetworknatorLogger.StartLogger(msg => GD.Print(msg));
	}

	public void StartClient()
	{
		client.OnConnected += () => 
		{
			GD.Print("Connection success!");
			LobbyUI.instance.Hide();
		};

		client.OnDisconnected += reason =>
		{
			LobbyUI.instance.Show();
		};

		client.OnDataReceived += data => 
		{
			using(PacketReader reader = new PacketReader(data))
			{
				int packetID = reader.ReadInt();
				HandlePacket(packetID, reader);
			}
		};

		client.Run("127.0.0.1:8090");
	}

	public void HandlePacket(int id, PacketReader reader)
	{
		switch ((ServerToClient)id)
		{
			case ServerToClient.welcome:
				int myID = reader.ReadInt();
				localID = myID;
				string message = reader.ReadString();
				GD.Print(message);
				client.SendTCP(new PacketBuilder()
					.Write((int)ClientToServer.welcomeReceived)
					.Write(myID)
					.Done());
				GD.Print("Sending welcomeReceived packet to server");
				break;
		
			case ServerToClient.playerSpawned:
				int toSpawn = reader.ReadInt();
				ClientPlayer.Spawn(toSpawn);
				GD.Print($"received spawn packet, will spawn {toSpawn}");
				break;

			case ServerToClient.playerPosition:
				if(ClientPlayer.list.TryGetValue(reader.ReadInt(), out ClientPlayer player))
				{
					player.GlobalPosition = new Vector2(reader.ReadFloat(), reader.ReadFloat());
				}
				break;

			case ServerToClient.playerLeft:
				int toDelete = reader.ReadInt();
				ClientPlayer.list[toDelete].Destroy();
				GD.Print("disconnection packet received");
				break;
		}
	}

}
