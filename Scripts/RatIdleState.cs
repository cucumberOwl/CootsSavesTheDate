using Godot;

public class RatIdleState : RatBaseState
{
    private int timeToIdle;

    public RatIdleState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        type = EntityStateType.Idle;
        GD.Print("Entered Idle");
        var rng = new RandomNumberGenerator();
        rng.Randomize();
        timeToIdle = rng.RandiRange(50, 200);
        GD.Print("Idle Time: " + timeToIdle);
        rng.Randomize();
        if (rng.RandiRange(0, 100) > 20)
            _context.GetNode<AnimatedSprite>("AnimatedSprite").Play("idle");
        else
            _context.GetNode<AnimatedSprite>("AnimatedSprite").Play("sniffa");
    }

    public override void UpdateState(float delta)
    {
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
        //GD.Print("Idle Time: " + timeToIdle);
        if (timeToIdle-- <= 0)
        {
            SwitchState(_factory.Walk());
        }
    }
}