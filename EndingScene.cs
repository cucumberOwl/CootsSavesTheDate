using Godot;
using System;

public class EndingScene : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        MusicController mc = GetNode<MusicController>("/root/MusicController");
        mc.musicPlayer.Stop();

        AudioStreamPlayer audioTrack = GetNode<AudioStreamPlayer>("AudioTrack");
        audioTrack.VolumeDb = mc.mainVolume;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    public void _on_MainMenu_pressed()
    {
        GetTree().ChangeScene("res://MainMenu.tscn");
    }
}
