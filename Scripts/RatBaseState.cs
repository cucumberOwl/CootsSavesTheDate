using Godot;

public class RatBaseState : EntityBaseState
{
    protected RatStateMachine rat;
    protected Vector2 destination;
    protected Vector2 velocity;
    protected string attackPointName;

    public RatBaseState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        rat = (RatStateMachine)_context;
    }

    public override void UpdateState(float delta)
    {
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        // if (rat.GlobalPosition.y > rat.player.GlobalPosition.y)
        // {
        //     rat.ZIndex = 6;
        // }
        // else
        // {
        //     rat.ZIndex = 4;
        // }
        if (rat.player.currentAttack != null)
        {
            if (rat.player.currentAttack.GlobalPosition.DistanceTo(rat.GlobalPosition) < rat.damageDistance && rat.CurrentState.type != EntityStateType.Damaged)
            {
                rat.TakeDamage(rat.player.currentAttack.GlobalPosition, rat.player.attackDamage);
                SwitchState(_factory.Damaged());
            }
        }
    }

    public override void EnterState()
    {
    }

    protected void SetVelocity()
    {
        velocity = (destination - rat.GlobalPosition).Normalized();
        if (velocity.x > 0)
        {
            rat.GetNode<AnimatedSprite>("AnimatedSprite").FlipH = true;
            attackPointName = "AttackPointRight";
        }
        else
        {
            rat.GetNode<AnimatedSprite>("AnimatedSprite").FlipH = false;
            attackPointName = "AttackPointLeft";
        }
    }
}