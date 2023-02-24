using Godot;
using System;

public class RatStateMachine : EntityStateMachine
{
    private AnimatedSprite sprite;
    public Vector2 velocity;
    private ProgressBar HealthBar;
    public CollisionShape2D Collision;
    private MusicController mc;

    [Export]
    public int speed = 15;

    [Export]
    public int vision = 100;

    [Export]
    public int damage = 15;

    [Export]
    public int damageDistance = 20;

    [Export]
    public int startAttackDistance = 20;

    [Export]
    public int knockBack = 80;

    [Export]
    public float damagedTime = 0.2f;

    [Export]
    public int health = 100;

    private bool isBlink;
    public bool DedDed;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        DedDed = false;
        sprite = GetNode<AnimatedSprite>("AnimatedSprite");
        sprite.Play("idle");

        player = GetNode<PlayerController>("/root/Node2D/Player");
        HealthBar = GetNode<ProgressBar>("HealthBar/ProgressBar");
        Collision = GetNode<CollisionShape2D>("CollisionShape2D");

        mc = GetNode<MusicController>("/root/MusicController");

        HealthBar.MaxValue = health;
        HealthBar.MinValue = 0;
        HealthBar.Value = health;
        HealthBar.Visible = false;

        _states = new RatStateFactory(this);
        CurrentState = _states.Idle();
        CurrentState.EnterState();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        ZIndex = (int)GlobalPosition.y;
        CurrentState.UpdateState(delta);
    }

    public void blink()
    {
        if (!isBlink)
        {
            sprite.Modulate = new Color(1, 0, 0, 0.8f);
        }
        else
        {
            sprite.Modulate = new Color(1, 1, 1, 1);
        }
        isBlink = !isBlink;
    }

    public void resetBlink()
    {
        sprite.Modulate = new Color(1, 1, 1, 1);
    }

    public void TakeDamage(Vector2 point, int damage)
    {
        if (CurrentState.type == EntityStateType.Damaged)
            return;
        health -= damage;

        velocity = (GlobalPosition - point).Normalized() * knockBack;

        if (health <= 0)
        {
            mc.playEnemyDeath();
        }
        else
        {
            mc.playHit();
        }
        updateHbar();
    }

    public void HideBits()
    {
        HealthBar.Visible = false;
    }

    private void updateHbar()
    {
        HealthBar.Visible = true;
        HealthBar.Value = health;
    }

    public void reload()
    {
        if (health <= 0)
        {
            DedDed = true;
            CurrentState.SwitchState(_states.Died());
        }
        if (health < 100)
        {
            updateHbar();
        }
    }
}
