using Godot;
using System;

public class OpeningAudio : Node2D
{
    private MusicController mc;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mc = GetNode<MusicController>("/root/MusicController");
        AudioStreamPlayer2D vespa = GetNode<AudioStreamPlayer2D>("/root/Node2D/Sprite2/AudioStreamPlayer2D");
        vespa.VolumeDb = mc.mainVolume;

        AudioStreamPlayer audioTrack = GetNode<AudioStreamPlayer>("/root/Node2D/AudioTrack");
        audioTrack.VolumeDb = mc.mainVolume;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
