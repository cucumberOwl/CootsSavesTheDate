using Godot;
using System;

public class Box : RigidBody2D
{
    [Export]
    public int health = 25;

    [Export]
    public int numOfHits = 1;

    [Export]
    public int boxType = 0;

    [Export]
    public int coins = 1;

    [Export]
    public int heart = 0;

    private PlayerController player;
    private AnimatedSprite box;
    private Vector2 pos;
    private Position2D centerPos;
    private StaticBody2D staticBody;

    private AnimatedSprite shadow;

    private float speed = 150;
    private float hitTimer;

    public bool broken = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = GetNode<PlayerController>("/root/Node2D/Player");
        box = GetNode<AnimatedSprite>("AnimatedSprite");
        shadow = GetNode<AnimatedSprite>("Shadow");

        staticBody = GetNode<StaticBody2D>("StaticBody2D");
        box.Stop();
        box.Frame = boxType;

        centerPos = GetNode<Position2D>("Position2D");
        GravityScale = 0;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        ZIndex = (int)GlobalPosition.y;
        if (player.currentAttack != null)
        {
            if (player.currentAttack.GlobalPosition.DistanceTo(centerPos.GlobalPosition) < 30 && hitTimer <= 0 && !broken)
            {
                hitTimer = 0.3f;
                if (--numOfHits <= 0)
                {
                    broken = true;
                    box.Play("broken");
                    CollisionLayer = 16;
                    staticBody.CollisionMask = 16;
                    pos = Vector2.Zero;
                    speed = 0;
                    Mode = ModeEnum.Static;
                    Sleeping = true;
                    shadow.Visible = false;
                    dropItems();
                    player.breakBox();
                    return;
                }
            }
        }
        if (player.ZindexPos.GlobalPosition.DistanceTo(GlobalPosition) < 25 && hitTimer <= 0 && !broken)
        {

            pos = (GlobalPosition - player.ZindexPos.GlobalPosition).Normalized();
            ApplyImpulse(GlobalTransform.origin, pos * (speed / 2));
        }

        if (hitTimer > 0)
            hitTimer -= delta;

    }

    public void dropItems()
    {
        for (int i = 0; i < coins; ++i)
        {
            var scene = (PackedScene)ResourceLoader.Load("res://Coin.tscn");
            Coin coin = (Coin)scene.Instance();

            RandomNumberGenerator rng = new RandomNumberGenerator();
            rng.Randomize();
            Vector2 dir = new Vector2();
            if (rng.RandiRange(0, 1) == 1)
            {
                rng.Randomize();
                dir.x = rng.RandfRange(0.5f, 1);
            }
            else
            {
                rng.Randomize();
                dir.x = rng.RandfRange(-1, -0.5f);
            }
            rng.Randomize();
            if (rng.RandiRange(0, 1) == 1)
            {
                rng.Randomize();
                dir.y = rng.RandfRange(0.5f, 1);
            }
            else
            {
                rng.Randomize();
                dir.y = rng.RandfRange(-1, -0.5f);
            }
            GD.Print(dir);
            coin.GlobalPosition = GlobalPosition;// + dir * 10;
            GetNode("/root/Node2D").AddChild(coin);
            coin.ApplyImpulse(GlobalTransform.origin, dir * 100);
        }

        for (int h = 0; h < heart; ++h)
        {
            var scene = (PackedScene)ResourceLoader.Load("res://Heart.tscn");
            Heart heart = (Heart)scene.Instance();

            RandomNumberGenerator rng = new RandomNumberGenerator();
            rng.Randomize();
            Vector2 dir = new Vector2();
            if (rng.RandiRange(0, 1) == 1)
            {
                rng.Randomize();
                dir.x = rng.RandfRange(0.5f, 1);
            }
            else
            {
                rng.Randomize();
                dir.x = rng.RandfRange(-1, -0.5f);
            }
            rng.Randomize();
            if (rng.RandiRange(0, 1) == 1)
            {
                rng.Randomize();
                dir.y = rng.RandfRange(0.5f, 1);
            }
            else
            {
                rng.Randomize();
                dir.y = rng.RandfRange(-1, -0.5f);
            }
            GD.Print(dir);
            heart.GlobalPosition = GlobalPosition; //+ dir * 10;
            GetNode("/root/Node2D").AddChild(heart);
            heart.ApplyImpulse(GlobalTransform.origin, dir * 100);
        }
    }

    public void reload()
    {
        if (broken)
        {
            box.Play("broken");
            box.Frame = 3;
            CollisionLayer = 16;
            staticBody.CollisionMask = 16;
            pos = Vector2.Zero;
            speed = 0;
            Mode = ModeEnum.Static;
            Sleeping = true;
            shadow.Visible = false;
        }
    }

}
