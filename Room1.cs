using Godot;
using System;
using System.Collections.Generic;

public class Room1 : Node
{
    private List<RatStateMachine> rats;
    private AnimatedSprite lockedDoor;
    private StaticBody2D doorCollision;
    private Global global;

    private bool locked;

    public override void _Ready()
    {
        lockedDoor = GetNode<AnimatedSprite>("rats/Door");
        doorCollision = GetNode<StaticBody2D>("rats/Door/DoorCollision");
        global = GetNode<Global>("/root/Global");

        global.reloadSceneItems(global.room1Rats, global.room1Boxes);
        if (global.room1Position != Vector2.Zero)
        {
            GetNode<PlayerController>("/root/Node2D/Player").GlobalPosition = global.room1Position;
        }

        rats = new List<RatStateMachine>();
        var children = GetNode("rats").GetChildren();
        foreach (var rat in children)
        {
            if (rat.GetType() == typeof(RatStateMachine))
            {
                RatStateMachine r = (RatStateMachine)rat;
                if (r.CurrentState.type != EntityStateType.Died)
                    rats.Add(r);
            }
        }
        locked = false;
        if (rats.Count <= 0)
        {
            doorCollision.CollisionLayer = 6;
            lockedDoor.Play("unlocked");
            lockedDoor.Frame = 3;
        }

        global.sceneName = SceneName.Room1;
    }

    public override void _Process(float delta)
    {
        foreach (var rat in rats)
        {
            if (rat.CurrentState.type != EntityStateType.Died)
                return;
        }
        if (locked)
        {
            doorCollision.CollisionLayer = 6;
            lockedDoor.Play("unlocked");
            locked = false;
        }
        locked = true;
    }

    public void on_enterance(Node player)
    {
        if (player.Name == "Player")
        {
            Position2D playerPos = GetNode<Position2D>("/root/Node2D/playerEnterPos");
            global.room1Position = playerPos.GlobalPosition;
            var ratChildren = GetNode("/root/Node2D/rats").GetChildren();
            global.room1Rats.Clear();
            foreach (var rat in ratChildren)
            {
                if (rat.GetType() == typeof(RatStateMachine))
                {
                    RatStateMachine r = (RatStateMachine)rat;
                    global.room1Rats.Add(new Global.RatState(r.GlobalPosition, r.health, r.Name));
                }
            }

            var boxesChildren = GetNode("/root/Node2D/boxes").GetChildren();
            global.room1Boxes.Clear();
            foreach (var box in boxesChildren)
            {
                if (box.GetType() == typeof(Box))
                {
                    Box b = (Box)box;
                    global.room1Boxes.Add(new Global.BoxState(b.GlobalPosition, b.broken, b.Name));
                }
            }

            GetTree().ChangeScene("res://Main.tscn");
        }
    }

    public void on_exit(Node player)
    {
        if (player.Name == "Player")
        {
            Position2D playerPos = GetNode<Position2D>("/root/Node2D/playerExitPos");
            global.room1Position = playerPos.GlobalPosition;
            var ratChildren = GetNode("/root/Node2D/rats").GetChildren();
            global.room1Rats.Clear();
            foreach (var rat in ratChildren)
            {
                if (rat.GetType() == typeof(RatStateMachine))
                {
                    RatStateMachine r = (RatStateMachine)rat;
                    global.room1Rats.Add(new Global.RatState(r.GlobalPosition, r.health, r.Name));
                }
            }

            var boxesChildren = GetNode("/root/Node2D/boxes").GetChildren();
            global.room1Boxes.Clear();
            foreach (var box in boxesChildren)
            {
                if (box.GetType() == typeof(Box))
                {
                    Box b = (Box)box;
                    global.room1Boxes.Add(new Global.BoxState(b.GlobalPosition, b.broken, b.Name));
                }
            }

            GetTree().ChangeScene("res://PuzzleRoom.tscn");
        }
    }

}
