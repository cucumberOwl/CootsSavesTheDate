using Godot;

public class SlimeDiedState : SlimeBaseState
{
    private float diedTimer = 3f;
    private bool win;
    public SlimeDiedState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        type = EntityStateType.Died;
        GD.Print("Entered Died");
        NoCollision = true;
        slime.GetNode<AnimatedSprite>("AnimatedSprite").Play("died");
        slime.killAllMinions();
    }

    public override void UpdateState(float delta)
    {
        //base.UpdateState(delta);
        diedTimer -= delta;
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        //base.CheckSwitchStates();
        if (diedTimer <= 0 && !win)
        {
            slime.playEnding();
            win = true;
        }
    }
}