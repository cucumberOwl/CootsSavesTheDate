using Godot;
using System;

public class Heart : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public Vector2 dir;
    public float collectionTimer = 0.2f;
    private PlayerController player;
    private Position2D center;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = GetNode<PlayerController>("/root/Node2D/Player");
        center = GetNode<Position2D>("Center");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        ZIndex = (int)GlobalPosition.y;
        if (center.GlobalPosition.DistanceTo(player.GlobalPosition) <= 20 && collectionTimer <= 0)
        {
            player.collectHeart();
            QueueFree();
        }
        collectionTimer -= delta;
    }
}
