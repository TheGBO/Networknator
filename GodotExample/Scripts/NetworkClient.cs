using Godot;
using Networknator.Networking;
using Networknator.Utils;
using System;

public class NetworkClient : Node
{
	public static NetworkClient instance;
	public Client client = new Client();

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
		client.Run("127.0.0.1:8090");
	}

}
