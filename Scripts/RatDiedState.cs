using Godot;

public class RatDiedState : RatBaseState
{
    public RatDiedState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        type = EntityStateType.Died;
        GD.Print("Entered Died");
        rat.GetNode<AnimatedSprite>("AnimatedSprite").Play("die");
        if (rat.DedDed)
        {
            rat.GetNode<AnimatedSprite>("AnimatedSprite").Frame = 9;
        }
        rat.ZIndex = 4;
        rat.Collision.Disabled = true;
        rat.HideBits();
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
    }
}