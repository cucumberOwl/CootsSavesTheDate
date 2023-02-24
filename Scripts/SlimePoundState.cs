using Godot;

public class SlimePoundState : SlimeBaseState
{
    private float rise = 1f;
    private float floatingTime;
    private int numOfAttacks = 5;

    public SlimePoundState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        type = EntityStateType.Pound;
        GD.Print("Entered Pound");
        slime.GetNode<AnimatedSprite>("AnimatedSprite").Play("pound");
        rng.Randomize();
        floatingTime = rng.RandfRange(0.5f, 4);
    }

    public override void UpdateState(float delta)
    {
        base.UpdateState(delta);
        if (rise > 0)
        {
            slime.SlimeAnim.Position = new Vector2(slime.SlimeAnim.Position.x, slime.SlimeAnim.Position.y - (100 * delta));
        }
        if (rise <= 0.8f)
        {
            NoCollision = true;
        }
        if (rise < 0)
        {
            floatingTime -= delta;
        }
        if (floatingTime <= 0)
        {
            slime.SlimeAnim.Position = new Vector2(slime.SlimeAnim.Position.x, slime.SlimeAnim.Position.y + (280 * delta));
            if (slime.SlimeAnim.Position.y >= slime.spritePos.Position.y)
            {
                NoCollision = false;
                slime.SlimeAnim.Position = slime.spritePos.Position;
            }
        }
        rise -= delta;
        if (slime.GlobalPosition.DistanceTo(slime.player.GlobalPosition) > 5)
            slime.MoveAndSlide(dir * slime.hoverSpeed, Vector2.Up);
        destination = slime.player.GlobalPosition;
        getAnimDir();
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (rise < 0 && floatingTime < 0 && slime.SlimeAnim.Position.y >= slime.spritePos.Position.y)
        {
            slime.specialAttackTimer = 5f;
            slime.SlimeAnim.Position = slime.spritePos.Position;
            SwitchState(_factory.Idle());
        }
    }
}