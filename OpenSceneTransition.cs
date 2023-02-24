using Godot;
using System;

public class OpenSceneTransition : AnimationPlayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Play("fade_to_trans");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void _on_animation_finished(string name)
    {
        if (name == "fade_to_trans")
            Play("CutScene");
    }
}
