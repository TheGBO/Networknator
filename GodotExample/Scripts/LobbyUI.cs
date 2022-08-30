using Godot;
using System;

public class LobbyUI : Control
{
	public static LobbyUI instance;
	public override void _Ready()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

	private void _on_HostBtn_pressed()
	{
		NetworkServer.instance.StartServer();
	}
	
	private void _on_ClientBtn_pressed()
	{
		NetworkClient.instance.StartClient();
	}
}


