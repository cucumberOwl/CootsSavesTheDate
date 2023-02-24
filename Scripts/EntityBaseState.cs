using Godot;

public abstract class EntityBaseState
{
    public EntityStateFactory _factory;
    public EntityStateMachine _context;
    public RandomNumberGenerator rng;

    public EntityStateType type;

    public EntityBaseState(EntityStateMachine context, EntityStateFactory playerStateFactory)
    {
        _context = context;
        _factory = playerStateFactory;
        rng = new RandomNumberGenerator();
    }

    public abstract void EnterState();

    public abstract void UpdateState(float delta);

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public void UpdateStates() { }

    public void SwitchState(EntityBaseState newState)
    {
        ExitState();

        newState.EnterState();

        _context.CurrentState = newState;
    }

}