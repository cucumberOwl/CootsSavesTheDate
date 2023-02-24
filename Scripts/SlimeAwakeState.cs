using Godot;

public class SlimeAwakeState : SlimeBaseState
{
    private float awakeTimer = 1f;

    public SlimeAwakeState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        type = EntityStateType.Awake;
        GD.Print("Entered Awake");

        slime.GetNode<AnimatedSprite>("AnimatedSprite").Play("awake");
    }

    public override void UpdateState(float delta)
    {
        awakeTimer -= delta;
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (awakeTimer <= 0)
        {
            SwitchState(_factory.Idle());
        }
    }
}