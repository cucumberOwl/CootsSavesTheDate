using Godot;
using System;

public class Swipe : Sprite
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private AnimatedSprite shit;
    private AudioStreamPlayer2D flush;
    private Sprite Blockage;
    private Global global;
    private MusicController mc;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");
        mc = GetNode<MusicController>("/root/MusicController");

        shit = GetNode<AnimatedSprite>("Shit");
        //shit.Play("none");

        flush = GetNode<AudioStreamPlayer2D>("Flush");

        Blockage = GetNode<Sprite>("Blockage");
    }

    public void Coots_Entered(Node coots)
    {
        //shit.Play("none");
    }

    public void Coots_Exited(Node coots)
    {
        if (coots.Name != "Player")
            return;

        if (!shit.Playing || shit.Frame >= 23)
        {
            shit.Frame = 0;
            shit.Play("shit");
            flush.VolumeDb = mc.mainVolume;
            flush.Play();
            if (Blockage != null)
            {
                global.puzzleBlockage = false;
                clearBlockage();
            }

        }
    }

    public void clearBlockage()
    {
        Blockage.Visible = false;
        Blockage.QueueFree();
        Blockage = null;
    }
}
