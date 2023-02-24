using Godot;
using System;
using System.Collections.Generic;

public class SlimeStateMachine : EntityStateMachine
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private ProgressBar HealthBar;
    public AnimatedSprite SlimeAnim;
    public AnimatedSprite Shadow;
    private AnimationPlayer animPlayer;
    public Position2D spritePos;


    public MusicController mc;

    public int attack;

    [Export]
    public int damageDistance = 22;

    [Export]
    public int walkSpeed = 40;

    [Export]
    public int attackSpeed = 100;

    [Export]
    public int hoverSpeed = 200;

    [Export]
    public float attackDistance = 60;

    [Export]
    public int health = 500;

    [Export]
    public float inviciblity = 1f;

    public float specialAttackTimer = 2f;
    private float damageEffect = 0;
    private bool blink;
    private RandomNumberGenerator rng;

    private bool presentChase;

    private List<RatStateMachine> ratMinions;
    public List<int> specials = new List<int>() { 0, 3 };

    private Sprite present;

    public Node drainPositions;

    private AnimatedSprite lockedDoor;
    private StaticBody2D doorCollision;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = GetNode<PlayerController>("/root/Node2D/Player");
        present = GetNode<Sprite>("Present");

        lockedDoor = GetNode<AnimatedSprite>("/root/Node2D/Door");
        doorCollision = GetNode<StaticBody2D>("/root/Node2D/Door/DoorCollision");

        HealthBar = GetNode<ProgressBar>("AnimatedSprite/HealthBar/ProgressBar");
        SlimeAnim = GetNode<AnimatedSprite>("AnimatedSprite");
        Shadow = GetNode<AnimatedSprite>("Shadow");
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        spritePos = GetNode<Position2D>("SpritePos");

        drainPositions = GetNode("/root/Node2D/DrainPositions");

        mc = GetNode<MusicController>("/root/MusicController");

        _states = new SlimeStateFactory(this);
        CurrentState = _states.Awake();
        CurrentState.EnterState();

        HealthBar.MaxValue = health;
        HealthBar.MinValue = 0;
        HealthBar.Value = health;
        HealthBar.Visible = false;

        ratMinions = new List<RatStateMachine>();

        rng = new RandomNumberGenerator();

        attack = 2;
    }

    public override void _Process(float delta)
    {
        Chase(delta);
        ZIndex = (int)GlobalPosition.y;
        CurrentState.UpdateState(delta);
        if (damageEffect > 0)
        {
            damageEffect -= delta;
            if (blink)
                Modulate = new Color(1, 0, 0, 0.8f);
            else
                Modulate = new Color(1, 1, 1, 1);
            blink = !blink;
        }
        else if (damageEffect <= 0)
        {
            Modulate = new Color(1, 1, 1, 1);
        }
    }

    public void TakeDamage(Vector2 point, int damage)
    {
        if (damageEffect > 0)
            return;
        health -= damage;
        HealthBar.Visible = true;
        HealthBar.Value = health;

        if (health <= 0)
        {
            //sludgePlayer.Stop();
        }
        else
        {
            mc.playHit();
        }

        damageEffect = inviciblity;
    }

    public void spawnRat()
    {
        var scene = (PackedScene)ResourceLoader.Load("res://RatEnemy.tscn");
        RatStateMachine rat = (RatStateMachine)scene.Instance();
        rng.Randomize();
        float x = rng.RandfRange(-200, 200);
        rng.Randomize();
        float y = rng.RandfRange(-60, 100);
        rat.GlobalPosition = new Vector2(x + 508, y + 203);
        rat.vision = 500;
        rng.Randomize();
        rat.startAttackDistance = rng.RandiRange(25, 80);
        rng.Randomize();
        rat.speed = rng.RandiRange(20, 35);

        ratMinions.Add(rat);

        GetNode("/root/Node2D").AddChild(rat);
    }

    public void killAllMinions()
    {
        foreach (RatStateMachine rat in ratMinions)
        {
            if (rat.CurrentState.type != EntityStateType.Died)
                rat.CurrentState.SwitchState(rat.CurrentState._factory.Died());
        }
    }

    public Vector2 ClosestDrain(Vector2 pos)
    {
        Vector2 drain = new Vector2(510, 182);
        float currentDist = pos.DistanceTo(drain);
        var drains = drainPositions.GetChildren();
        foreach (var item in drains)
        {
            Position2D point = (Position2D)item;
            float newDist = pos.DistanceTo(point.GlobalPosition);
            if (newDist < currentDist)
            {
                drain = point.GlobalPosition;
                currentDist = newDist;
            }
        }

        return drain;
    }

    public Vector2 BestDrain(Vector2 pos, int dist)
    {
        Vector2 drain;
        var drains = drainPositions.GetChildren();

        do
        {
            rng.Randomize();
            int index = rng.RandiRange(0, drains.Count - 1);

            Position2D point = (Position2D)drains[index];
            drain = point.GlobalPosition;
            drains.RemoveAt(index);

        } while (drain.DistanceTo(pos) < dist);

        return drain;
    }



    public void HideBits()
    {
        HealthBar.Visible = false;
        Shadow.Visible = false;
    }

    public void ShowBits()
    {
        HealthBar.Visible = health != HealthBar.MaxValue ? true : false;
        Shadow.Visible = true;
        Shadow.Frame = 0;
        Shadow.Play("default");

    }

    public void playEnding()
    {
        for (int i = 0; i < 20; ++i)
        {
            var scene = (PackedScene)ResourceLoader.Load("res://Coin.tscn");
            Coin coin = (Coin)scene.Instance();

            RandomNumberGenerator rng = new RandomNumberGenerator();
            rng.Randomize();
            Vector2 dir = new Vector2();

            rng.Randomize();
            dir.x = rng.RandfRange(-1, 1);
            rng.Randomize();
            dir.y = rng.RandfRange(-1, 1);

            coin.GlobalPosition = GlobalPosition;// + dir * 10;
            GetNode("/root/Node2D").AddChild(coin);
            coin.ApplyImpulse(GlobalTransform.origin, dir * 100);
        }


        animPlayer.Play("Present");
    }

    public void _on_animation_finished(string name)
    {
        if (name == "Present")
        {
            presentChase = true;
            doorCollision.CollisionLayer = 6;
            lockedDoor.Play("unlocked");
        }
    }

    private float chaseTimer = 0.03f;

    private void Chase(float delta)
    {
        if (!presentChase)
            return;

        Vector2 dest = getPresentPos();
        float dist = present.GlobalPosition.DistanceTo(dest);
        Vector2 dir = (dest - present.GlobalPosition).Normalized();
        if (chaseTimer <= 0)
        {
            float x = present.GlobalPosition.x + (dir.x * (dist / 4));
            float y = present.GlobalPosition.y + (dir.y * (dist / 4));
            present.GlobalPosition = new Vector2(x, y);
            chaseTimer = 0.03f;
        }
        chaseTimer -= delta;

    }

    private Vector2 getPresentPos()
    {
        Vector2 result = player.GlobalPosition;
        result.x += (player.direction.x * -1) * 10;
        result.y += (player.direction.y * -1) * 10;

        return result;
    }
}
