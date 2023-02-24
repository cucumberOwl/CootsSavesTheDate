using Godot;
using System;

public class BossScene : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public void on_exit(Node player)
    {
        if (player.Name == "Player")
            GetTree().ChangeScene("res://EndingScene.tscn");
    }
}
