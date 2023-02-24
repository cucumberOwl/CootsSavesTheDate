using Godot;

public class SlimeSpawnState : SlimeBaseState
{
    private float Timer = 1.5f;
    private bool spawned;

    public SlimeSpawnState(EntityStateMachine currentContext, EntityStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        spawned = false;
    }

    public override void EnterState()
    {
        type = EntityStateType.Spawn;
        GD.Print("Entered Spawn");

        slime.GetNode<AnimatedSprite>("AnimatedSprite").Play("spawn");
    }

    public override void UpdateState(float delta)
    {
        base.UpdateState(delta);
        Timer -= delta;
        if (Timer <= 0.5f && !spawned)
        {
            spawned = true;
            slime.spawnRat();
            slime.spawnRat();
            slime.spawnRat();
        }
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (Timer <= 0)
        {
            slime.specialAttackTimer = 0.5f;
            SwitchState(_factory.Idle());
        }
    }
}