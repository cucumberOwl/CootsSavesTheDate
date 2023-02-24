using System;
using Godot;

public class RatWalkState : RatBaseState
{
    int maxWalkTime;
    float speed;

    public RatWalkState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        speed = rat.speed;
    }

    public override void EnterState()
    {
        type = EntityStateType.Walking;
        GD.Print("Entered Walk");
        destination = _context.GlobalPosition;
        rng.Randomize();
        destination.x += rng.RandiRange(-30, 30);
        rng.Randomize();
        destination.y += rng.RandiRange(-20, 20);
        rng.Randomize();
        maxWalkTime = rng.RandiRange(200, 500);
        _context.GetNode<AnimatedSprite>("AnimatedSprite").Play("walk");
        SetVelocity();
    }

    public override void UpdateState(float delta)
    {
        _context.MoveAndSlide(velocity * speed, Vector2.Up);
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (rat.player.GlobalPosition.DistanceTo(rat.GlobalPosition) <= rat.vision)
        {
            SwitchState(_factory.Hunt());
        }
        if (maxWalkTime-- <= 0 || DestinationReached())
        {
            rng.Randomize();
            if (rng.RandiRange(0, 100) > 40)
            {
                SwitchState(_factory.Idle());
            }
            else
            {
                SwitchState(_factory.Walk());
            }

        }
    }

    private bool DestinationReached()
    {
        return Math.Abs(_context.GlobalPosition.x - destination.x) < 5 && Math.Abs(_context.GlobalPosition.y - destination.y) < 5;
    }
}