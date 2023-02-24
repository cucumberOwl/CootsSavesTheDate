public class SlimeStateFactory : EntityStateFactory
{
    public SlimeStateFactory(EntityStateMachine currentContext) : base(currentContext)
    {

    }

    public override EntityBaseState Awake()
    {
        return new SlimeAwakeState(_context, this);
    }

    public override EntityBaseState Idle()
    {
        return new SlimeIdleState(_context, this);
    }

    public override EntityBaseState Walk()
    {
        throw new System.NotImplementedException();
    }

    public override EntityBaseState Hunt()
    {
        throw new System.NotImplementedException();
    }

    public override EntityBaseState Attack()
    {
        return new SlimeAttackState(_context, this);
    }

    public override EntityBaseState Damaged()
    {
        throw new System.NotImplementedException();
    }

    public override EntityBaseState Died()
    {
        return new SlimeDiedState(_context, this);
    }

    public override EntityBaseState Pound()
    {
        return new SlimePoundState(_context, this);
    }

    public override EntityBaseState Fury()
    {
        return new SlimeFuryState(_context, this);
    }

    public override EntityBaseState Splodge()
    {
        return new SlimeSplodgeState(_context, this);
    }

    public override EntityBaseState Spawn()
    {
        return new SlimeSpawnState(_context, this);
    }
}