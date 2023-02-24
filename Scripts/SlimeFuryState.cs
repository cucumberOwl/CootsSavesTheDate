using Godot;

public class SlimeFuryState : SlimeBaseState
{
    private float attackTimer = 1f;
    private int numOfAttacks = 5;

    public SlimeFuryState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        type = EntityStateType.Fury;
        GD.Print("Entered Fury");
        destination = slime.player.GlobalPosition;
        string animDIR = getAnimDir();
        slime.GetNode<AnimatedSprite>("AnimatedSprite").Play("attack_" + animDIR);
    }

    public override void UpdateState(float delta)
    {
        base.UpdateState(delta);
        slime.MoveAndSlide(dir * slime.attackSpeed, Vector2.Up);
        attackTimer -= delta;
        destination = slime.player.GlobalPosition;
        string animDIR = getAnimDir();
        slime.GetNode<AnimatedSprite>("AnimatedSprite").Play("attack_" + animDIR);
        slime.GetNode<AnimatedSprite>("AnimatedSprite").Frame = 2;
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if ((numOfAttacks <= 0 && attackTimer <= 0) || hit)
        {
            slime.specialAttackTimer = 5f;
            SwitchState(_factory.Idle());
        }
        if (attackTimer <= 0)
        {
            attackTimer = 1f;
            --numOfAttacks;
            destination = slime.player.GlobalPosition;
            string animDIR = getAnimDir();
            slime.GetNode<AnimatedSprite>("AnimatedSprite").Play("attack_" + animDIR);
            slime.GetNode<AnimatedSprite>("AnimatedSprite").Frame = 2;
        }
    }
}