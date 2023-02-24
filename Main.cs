using Godot;
using System;

public class Main : Node2D
{
    private Global global;
    private int Timer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");
        if (global.mainPosition != Vector2.Zero)
        {
            GetNode<PlayerController>("/root/Node2D/Player").GlobalPosition = global.mainPosition;
        }

        global.reloadSceneItems(global.mainRoomRats, global.mainRoomBoxes);

        global.sceneName = SceneName.Main;
    }

}
