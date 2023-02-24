using System;
using Godot;

public class SlimeBaseState : EntityBaseState
{
    protected SlimeStateMachine slime;
    protected Vector2 destination;
    protected Vector2 dir;
    protected Vector2 velocity;
    protected string attackPointName;
    protected bool hit;
    protected bool NoCollision;

    protected float prev_attack;

    public SlimeBaseState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        slime = (SlimeStateMachine)_context;
        hit = false;
        NoCollision = false;
        prev_attack = -1;
    }

    public override void UpdateState(float delta)
    {
        slime.specialAttackTimer -= delta;
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (slime.health <= 0 && type != EntityStateType.Died)
            SwitchState(_factory.Died());
        // if (slime.GlobalPosition.y > slime.player.GlobalPosition.y)
        // {
        //     slime.ZIndex = 6;
        // }
        // else
        // {
        //     slime.ZIndex = 4;
        // }
        if (slime.spritePos.GlobalPosition.DistanceTo(slime.player.GlobalPosition) < slime.damageDistance && !NoCollision)
        {
            hit = slime.player.TakeDamage(slime.spritePos.GlobalPosition, 200, getDamage(slime.CurrentState));
        }

        if (slime.player.currentAttack != null && !NoCollision)
        {
            if (slime.player.currentAttack.GlobalPosition.DistanceTo(slime.GlobalPosition) < slime.damageDistance)
            {
                slime.TakeDamage(slime.player.currentAttack.GlobalPosition, slime.player.attackDamage);
            }
        }
    }

    public override void EnterState()
    {
    }

    protected int getDamage(EntityBaseState state)
    {
        switch (state.type)
        {
            case EntityStateType.Pound: return 20;
            default: return 15;
        }
    }

    protected string getAnimDir()
    {
        dir = (destination - slime.GlobalPosition).Normalized();
        if (Math.Abs(dir.x) >= 0.25 && Math.Abs(dir.y) >= .25)
        {
            if (dir.x > 0.25 && dir.y > 0.25)
                return "SE";
            if (dir.x > 0.25 && dir.y < -0.25)
                return "NE";
            if (dir.x < -0.25 && dir.y > 0.25)
                return "SW";
            if (dir.x < -0.25 && dir.y < -0.25)
                return "NW";
        }

        if (Math.Abs(dir.x) >= Math.Abs(dir.y))
        {
            if (dir.x > 0)
                return "E";
            else
                return "W";
        }
        else
        {
            if (dir.y > 0)
                return "S";
            else
                return "N";
        }
    }

    protected void SwitchSpecialAttackState()
    {
        if (slime.attack == 0)
            SwitchState(_factory.Fury());
        if (slime.attack == 1)
            SwitchState(_factory.Pound());
        if (slime.attack == 2)
            SwitchState(_factory.Spawn());
        if (slime.attack == 3)
            SwitchState(_factory.Splodge());

        if (slime.attack == 2)
        {
            slime.attack = 1;
            slime.specialAttackTimer = 4f;
            return;
        }
        slime.specialAttackTimer = 4f;
        rng.Randomize();
        slime.attack = rng.RandiRange(0, 3);

        if (slime.specials.Count > 0 && slime.attack != 2)
        {
            int index = rng.RandiRange(0, slime.specials.Count - 1);
            slime.attack = slime.specials[index];
            slime.specials.RemoveAt(index);
        }

    }

    protected void SetVelocity()
    {
        // velocity = (destination - rat.GlobalPosition).Normalized();
        // if (velocity.x > 0)
        // {
        //     rat.GetNode<AnimatedSprite>("AnimatedSprite").FlipH = true;
        //     attackPointName = "AttackPointRight";
        // }
        // else
        // {
        //     rat.GetNode<AnimatedSprite>("AnimatedSprite").FlipH = false;
        //     attackPointName = "AttackPointLeft";
        // }
    }
}