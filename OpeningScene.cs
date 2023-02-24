using Godot;
using System;

public class OpeningScene : AnimationPlayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private MusicController mc;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mc = GetNode<MusicController>("/root/MusicController");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    public void _on_finished(string name)
    {
        if (name == "CutScene")
        {
            mc.play_main_music();
            GetTree().ChangeScene("res://Main.tscn");
        }
    }

    public void _on_skip()
    {
        mc.play_main_music();
        GetTree().ChangeScene("res://Main.tscn");
    }

}
