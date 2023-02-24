using Godot;

public class SlimeIdleState : SlimeBaseState
{
    private float attackCooldown = 3f;

    public SlimeIdleState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        type = EntityStateType.Idle;
        GD.Print("Entered Idle");
    }

    public override void UpdateState(float delta)
    {
        base.UpdateState(delta);
        destination = slime.player.GlobalPosition;
        string animDIR = getAnimDir();
        slime.GetNode<AnimatedSprite>("AnimatedSprite").Play("idle_" + animDIR);
        if (slime.GlobalPosition.DistanceTo(destination) > 5)
            slime.MoveAndSlide(dir * slime.walkSpeed, Vector2.Up);
        attackCooldown -= delta;
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        GD.Print(slime.specialAttackTimer);
        if (slime.specialAttackTimer <= 0)
        {
            SwitchSpecialAttackState();
            return;
        }
        if (attackCooldown <= 0 && slime.GlobalPosition.DistanceTo(slime.player.GlobalPosition) < slime.attackDistance)
        {
            SwitchState(_factory.Attack());
        }
    }
}