using Godot;
using Networknator.Networking.Packets;
using System;
using System.Collections.Generic;

public class ServerPlayer : Node2D
{
	public static Dictionary<int, ServerPlayer> list = new Dictionary<int, ServerPlayer>();
	private static PackedScene playerScene = ResourceLoader.Load<PackedScene>("res://Scenes/ServerPlayer.tscn");
	public int ID {get;set;}
	
	public static void Spawn(int id)
	{
		foreach (ServerPlayer otherPlayer in list.Values)
		{
			otherPlayer.SendSpawn(id);
		}
		ServerPlayer player = playerScene.Instance<ServerPlayer>();
		player.ID = id;
		player.Name = $"Player::{id}";
		player.SendSpawn();
		NetworkServer.instance.AddChild(player);
		list.Add(id, player);
	}

	public void SendSpawn(int toClientid)
	{
		NetworkServer.instance.server.SendDataTo(toClientid, AddSpawnData());
	}

	public void SendSpawn()
	{
		NetworkServer.instance.server.SendDataToAll(AddSpawnData());
	}

	private byte[] AddSpawnData()
	{
		return new PacketBuilder()
			.Write((int)ServerToClient.playerSpawned)
			.Write(ID)
			.Done();
	}
}
