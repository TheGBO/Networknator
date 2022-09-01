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
		player.Name = $"Player{id}";

		list.Add(id, player);
	}

	public void Destroy()
	{
		list.Remove(ID);
		QueueFree();
	}

	public override void _Process(float delta)
	{
		if(isLocal)
		{
			if(Input.IsKeyPressed((int)KeyList.W))			
				Move(new Vector2(0,-1));
			
			if(Input.IsKeyPressed((int)KeyList.A))		
				Move(new Vector2(-1,0));
			
			if(Input.IsKeyPressed((int)KeyList.S))			
				Move(new Vector2(0,1));

			if(Input.IsKeyPressed((int)KeyList.D))
				Move(new Vector2(1,0));
		}
	}

	private void Move(Vector2 vec)
	{
		GlobalPosition += vec;
		SendPosition();
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
