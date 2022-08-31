using Godot;
using Networknator.Networking.Packets;
using System;
using System.Collections.Generic;

public class ClientPlayer : Node2D
{
	public static Dictionary<int, ClientPlayer> list = new Dictionary<int, ClientPlayer>();
	private static PackedScene playerScene = ResourceLoader.Load<PackedScene>("res://Scenes/ClientPlayer.tscn");
	public int ID { get; set; }
	public bool isLocal;

	public static void Spawn(int id)
	{
		ClientPlayer player;

		if(id == NetworkClient.localID)
		{
			player = playerScene.Instance<ClientPlayer>();
			player.isLocal = true;
			NetworkClient.instance.AddChild(player);
		}
		else
		{
			player = playerScene.Instance<ClientPlayer>();
			player.isLocal = false;
			NetworkClient.instance.AddChild(player);
		}

		list.Add(id, player);
	}

	public override void _Process(float delta)
	{
		if(isLocal)
		{
			if(Input.IsKeyPressed((int)KeyList.W))			
				GlobalPosition -= new Vector2(0,1);
			
			if(Input.IsKeyPressed((int)KeyList.A))		
				GlobalPosition -= new Vector2(1,0);
			
			if(Input.IsKeyPressed((int)KeyList.S))			
				GlobalPosition += new Vector2(0,1);

			if(Input.IsKeyPressed((int)KeyList.D))
				GlobalPosition += new Vector2(1,0);
			SendPosition();
		}
	}

	private void SendPosition()
	{
		NetworkClient.instance.client.SendTCP(new PacketBuilder()
			.Write((int)ClientToServer.playerPosition)
			.Write(GlobalPosition.x)
			.Write(GlobalPosition.y)
			.Done());
	}
}
