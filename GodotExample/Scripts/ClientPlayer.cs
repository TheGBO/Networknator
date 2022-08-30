using Godot;
using System;
using System.Collections.Generic;

public class ClientPlayer : Node2D
{
	public static Dictionary<int, ClientPlayer> list = new Dictionary<int, ClientPlayer>();

	public int ID { get; set; }
	
}
