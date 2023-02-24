using Godot;
using System;

public class PuzzleRoom : Node2D
{
    private AnimatedSprite lockedDoor;
    private StaticBody2D doorCollision;
    private RatStateMachine rat;
    private bool locked;

    private Global global;

    private Swipe swipeBidet;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        lockedDoor = GetNode<AnimatedSprite>("Door");
        doorCollision = GetNode<StaticBody2D>("Door/DoorCollision");

        rat = GetNode<RatStateMachine>("rats/KeyRat");
        global = GetNode<Global>("/root/Global");
        global.reloadSceneItems(global.puzzleRoomRats, global.puzzleRoomBoxes);

        locked = true;

        if (rat.CurrentState.type == EntityStateType.Died)
        {
            doorCollision.CollisionLayer = 6;
            lockedDoor.Play("unlocked");
            lockedDoor.Frame = 3;
        }

        swipeBidet = GetNode<Swipe>("ToiletClearer");
        if (!global.puzzleBlockage)
        {
            swipeBidet.clearBlockage();
        }

        global.sceneName = SceneName.Puzzle;
    }

    public override void _Process(float delta)
    {
        if (rat.CurrentState.type == EntityStateType.Died)
        {
            if (locked)
            {
                doorCollision.CollisionLayer = 6;
                lockedDoor.Play("unlocked");
                locked = false;
            }
        }
    }

    public void on_exit(Node player)
    {
        if (player.Name == "Player")
        {
            var ratChildren = GetNode("/root/Node2D/rats").GetChildren();
            global.puzzleRoomRats.Clear();
            foreach (var rat in ratChildren)
            {
                if (rat.GetType() == typeof(RatStateMachine))
                {
                    RatStateMachine r = (RatStateMachine)rat;
                    global.puzzleRoomRats.Add(new Global.RatState(r.GlobalPosition, r.health, r.Name));
                }
            }

            var boxesChildren = GetNode("/root/Node2D/boxes").GetChildren();
            global.puzzleRoomBoxes.Clear();
            foreach (var box in boxesChildren)
            {
                if (box.GetType() == typeof(Box))
                {
                    Box b = (Box)box;
                    global.puzzleRoomBoxes.Add(new Global.BoxState(b.GlobalPosition, b.broken, b.Name));
                }
            }
            GetTree().ChangeScene("res://Room1.tscn");
        }
    }
}
