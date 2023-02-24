using Godot;
using System;

public class HUD : CanvasLayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private ProgressBar healthBar;
    private MusicController mc;
    private HSlider volumeSlider;
    private Label coinsLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        healthBar = GetNode<ProgressBar>("HealthBar/TextureRect/ProgressBar");
        mc = GetNode<MusicController>("/root/MusicController");

        volumeSlider = GetNode<HSlider>("HealthBar/TextureRect/Volume/Label/HSlider");
        volumeSlider.Value = mc.mainVolume;

        coinsLabel = GetNode<Label>("HealthBar/TextureRect/Coins/Count/Label");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void setHealth(int health)
    {
        healthBar.Value = health;
    }

    public void _on_volume_changed(float value)
    {
        mc.updateVolume(value);
    }

    public void updateCoins(int coins, int maxcoins)
    {
        coinsLabel.Text = coins + " / " + maxcoins;
    }

}
