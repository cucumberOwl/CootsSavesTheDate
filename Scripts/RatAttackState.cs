using Godot;

public class RatAttackState : RatBaseState
{
    private int attackTimer;
    private int recoveryTime = 20;
    float speed;
    private bool hit;

    public RatAttackState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        speed = rat.speed;
    }

    public override void EnterState()
    {
        type = EntityStateType.Attack;
        GD.Print("Entered Attack");
        rat.GetNode<AnimatedSprite>("AnimatedSprite").Play("attack");
        attackTimer = 50;
        hit = false;
    }

    public override void UpdateState(float delta)
    {
        if (attackTimer < 20 && attackTimer > 5 && !hit)
        {
            destination = rat.player.GlobalPosition;
            SetVelocity();
            Position2D attackPoint = rat.GetNode<Position2D>(attackPointName);
            if (attackPoint.GlobalPosition.DistanceTo(rat.player.GlobalPosition) < rat.damageDistance)
            {
                rat.player.TakeDamage(attackPoint.GlobalPosition, 100, rat.damage);
                hit = true;
            }
        }
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (attackTimer-- <= 0)
        {
            rat.GetNode<AnimatedSprite>("AnimatedSprite").Play("idle");
            if (recoveryTime-- <= 0)
            {
                SwitchState(_factory.Idle());
            }
            else
            {
                destination = rat.player.GlobalPosition;
                SetVelocity();
                _context.MoveAndSlide(velocity * speed, Vector2.Up);
            }
        }

    }
}