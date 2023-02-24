using Godot;
using System;
using System.Collections.Generic;

public class MainMenu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private ColorRect fade;
    private AnimationPlayer animPlayer;
    private Sprite credits;
    private MusicController mc;
    private Global global;
    private HSlider volumeBar;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        fade = GetNode<ColorRect>("ColorRect");
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        credits = GetNode<Sprite>("CreditCont/Credits");

        mc = GetNode<MusicController>("/root/MusicController");
        global = GetNode<Global>("/root/Global");

        volumeBar = GetNode<HSlider>("VBoxContainer/Label/HSlider");

        global.cootsCoins = 0;
        global.playerHealth = 100;

        global.mainRoomRats = new List<Global.RatState>();
        global.mainRoomBoxes = new List<Global.BoxState>();

        global.room1Rats = new List<Global.RatState>();
        global.room1Boxes = new List<Global.BoxState>();

        global.puzzleRoomRats = new List<Global.RatState>();
        global.puzzleRoomBoxes = new List<Global.BoxState>();

        global.puzzleBlockage = true;
        global.mainPosition = Vector2.Zero;
        global.room1Position = Vector2.Zero;
        global.puzzlePosition = Vector2.Zero;

        volumeBar.Value = mc.mainVolume;
        mc.play_menu_music();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void _on_Start_pressed()
    {
        fade.Visible = true;
        animPlayer.Play("fade_to_black");
        mc.musicPlayer.Stop();
    }

    public void _on_animation_finished(string name)
    {
        if (name == "fade_to_black")
            GetTree().ChangeScene("res://OpeningScene.tscn");
    }

    public void _on_close_credits()
    {
        credits.Visible = false;
    }

    public void _on_open_credits()
    {
        credits.Visible = true;
    }

    public void _on_volume_changed(float value)
    {
        mc.updateVolume(value);
    }
}
