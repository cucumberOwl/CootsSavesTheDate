using Godot;

public class SlimeAttackState : SlimeBaseState
{
    private float attackTimer = 1f;

    public SlimeAttackState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        type = EntityStateType.Attack;
        GD.Print("Entered Attack");
        destination = slime.player.GlobalPosition;
        string animDIR = getAnimDir();
        slime.GetNode<AnimatedSprite>("AnimatedSprite").Play("attack_" + animDIR);
    }

    public override void UpdateState(float delta)
    {
        base.UpdateState(delta);
        slime.MoveAndSlide(dir * slime.attackSpeed, Vector2.Up);
        attackTimer -= delta;
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (attackTimer <= 0)
            SwitchState(_factory.Idle());
    }
}