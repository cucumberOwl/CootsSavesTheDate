using Godot;

public class RatDamagedState : RatBaseState
{
    private float damagedTime;

    public RatDamagedState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        type = EntityStateType.Damaged;
        GD.Print("Entered Damaged");
        _context.GetNode<AnimatedSprite>("AnimatedSprite").Play("idle");
        damagedTime = rat.damagedTime;
    }

    public override void UpdateState(float delta)
    {
        rat.MoveAndSlide(rat.velocity, Vector2.Up);
        rat.blink();
        damagedTime -= delta;
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (damagedTime <= 0)
        {
            rat.resetBlink();
            if (rat.health <= 0)
            {
                SwitchState(_factory.Died());
                return;
            }
            rat.resetBlink();
            SwitchState(_factory.Idle());
        }
    }
}