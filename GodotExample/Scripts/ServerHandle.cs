using Godot;
using System;

public class ServerHandle
{
    
    public void AddPlayer(int id)
    {
        ServerPlayer.list.Add(id, GD.Load<ServerPlayer>("res://Scenes/ServerPlayer"));
        
    }
}
