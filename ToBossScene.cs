using Godot;
using System;

public class ToBossScene : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private MusicController mc;
    private Global global;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mc = GetNode<MusicController>("/root/MusicController");

        global = GetNode<Global>("/root/Global");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void Go_to_Boss(Node player)
    {
        if (player.Name == "Player")
        {
            global.sceneName = SceneName.Boss;
            mc.stopSludge();
            mc.play_boss_music();
            GetTree().ChangeScene("res://BossScene.tscn");
        }
    }

    public void _on_enter_Room1(Node player)
    {
        if (player.Name == "Player")
        {
            Position2D playerPos = GetNode<Position2D>("/root/Node2D/playerExitPos");
            global.mainPosition = playerPos.GlobalPosition;
            var ratChildren = GetNode("/root/Node2D/rats").GetChildren();
            global.mainRoomRats.Clear();
            foreach (var rat in ratChildren)
            {
                if (rat.GetType() == typeof(RatStateMachine))
                {
                    RatStateMachine r = (RatStateMachine)rat;
                    global.mainRoomRats.Add(new Global.RatState(r.GlobalPosition, r.health, r.Name));
                }
            }

            var boxesChildren = GetNode("/root/Node2D/boxes").GetChildren();
            global.mainRoomBoxes.Clear();
            foreach (var box in boxesChildren)
            {
                if (box.GetType() == typeof(Box))
                {
                    Box b = (Box)box;
                    global.mainRoomBoxes.Add(new Global.BoxState(b.GlobalPosition, b.broken, b.Name));
                }
            }

            GetTree().ChangeScene("res://Room1.tscn");
        }
    }
}
