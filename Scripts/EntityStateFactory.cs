public abstract class EntityStateFactory
{
    public EntityStateMachine _context;

    public EntityStateFactory(EntityStateMachine currentContext)
    {
        _context = currentContext;
    }

    public abstract EntityBaseState Idle();
    public abstract EntityBaseState Walk();
    public abstract EntityBaseState Hunt();
    public abstract EntityBaseState Attack();
    public abstract EntityBaseState Damaged();
    public abstract EntityBaseState Died();
    public abstract EntityBaseState Awake();
    public abstract EntityBaseState Pound();
    public abstract EntityBaseState Fury();
    public abstract EntityBaseState Splodge();
    public abstract EntityBaseState Spawn();


}