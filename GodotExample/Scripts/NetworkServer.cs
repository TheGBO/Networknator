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
	public static ServerSend serverSend;
	private ServerHandle serverHandle;

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
		serverSend = new ServerSend(server);
		server.OnConnection += id => 
		{
			serverSend.Welcome(id, $"Hello client, your id is {id}");
		};
		server.Run(8090);
	}
}
