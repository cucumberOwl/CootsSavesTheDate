using Godot;

public class SlimeSplodgeState : SlimeBaseState
{
    private bool foundDrain;
    private float betweenAttack = 0.5f;
    private bool attacking;
    private int numOfAttacks = 3;

    public SlimeSplodgeState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        type = EntityStateType.Splodge;
        GD.Print("Entered Splodge");
        destination = slime.ClosestDrain(slime.GlobalPosition);

    }

    public override void UpdateState(float delta)
    {
        base.UpdateState(delta);
        if (!foundDrain)
        {
            string animDIR = getAnimDir();
            slime.GetNode<AnimatedSprite>("AnimatedSprite").Play("idle_" + animDIR);
            slime.MoveAndSlide(dir * slime.walkSpeed, Vector2.Up);

            if (slime.GlobalPosition.DistanceTo(destination) <= 2)
            {
                foundDrain = true;
                slime.GetNode<AnimatedSprite>("AnimatedSprite").Play("sink");
                slime.HideBits();
            }
        }
        else
        {

            if (betweenAttack <= 0 && !attacking)
            {
                slime.GlobalPosition = slime.BestDrain(slime.player.GlobalPosition, 50);
                destination = slime.ClosestDrain(slime.player.GlobalPosition);
                attacking = true;
                slime.ShowBits();
            }
            if (attacking)
            {
                string animDIR = getAnimDir();
                slime.GetNode<AnimatedSprite>("AnimatedSprite").Play("attack_" + animDIR);
                slime.GetNode<AnimatedSprite>("AnimatedSprite").Frame = 2;
                slime.MoveAndSlide(dir * slime.attackSpeed, Vector2.Up);

                if (slime.GlobalPosition.DistanceTo(destination) <= 2)
                {
                    attacking = false;
                    slime.GetNode<AnimatedSprite>("AnimatedSprite").Play("sink");
                    slime.HideBits();
                    betweenAttack = 0.5f;
                    --numOfAttacks;
                }
            }

            betweenAttack -= delta;
        }

        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (numOfAttacks <= 0)
        {
            slime.specialAttackTimer = 5f;
            slime.ShowBits();
            SwitchState(_factory.Idle());
        }
    }
}