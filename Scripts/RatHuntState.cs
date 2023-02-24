using System;
using Godot;

public class RatHuntState : RatBaseState
{
    float speed;

    public RatHuntState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        rat = (RatStateMachine)_context;
        speed = rat.speed;
    }

    public override void EnterState()
    {
        type = EntityStateType.Hunt;
        GD.Print("Entered Hunt");
        destination = rat.player.GlobalPosition;
        rat.GetNode<AnimatedSprite>("AnimatedSprite").Play("walk");
        SetVelocity();
    }

    public override void UpdateState(float delta)
    {
        destination = rat.player.GlobalPosition;
        SetVelocity();
        _context.MoveAndSlide(velocity * speed, Vector2.Up);
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (DestinationReached())
        {
            SwitchState(_factory.Attack());
        }
    }

    private bool DestinationReached()
    {
        return rat.GlobalPosition.DistanceTo(destination) < rat.startAttackDistance;
    }
}