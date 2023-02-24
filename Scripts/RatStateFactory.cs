public class RatStateFactory : EntityStateFactory
{
    public RatStateFactory(EntityStateMachine currentContext) : base(currentContext)
    {

    }

    public override EntityBaseState Idle()
    {
        return new RatIdleState(_context, this);
    }

    public override EntityBaseState Walk()
    {
        return new RatWalkState(_context, this);
    }

    public override EntityBaseState Hunt()
    {
        return new RatHuntState(_context, this);
    }

    public override EntityBaseState Attack()
    {
        return new RatAttackState(_context, this);
    }

    public override EntityBaseState Damaged()
    {
        return new RatDamagedState(_context, this);
    }

    public override EntityBaseState Died()
    {
        return new RatDiedState(_context, this);
    }

    public override EntityBaseState Awake()
    {
        throw new System.NotImplementedException();
    }

    public override EntityBaseState Pound()
    {
        throw new System.NotImplementedException();
    }

    public override EntityBaseState Fury()
    {
        throw new System.NotImplementedException();
    }

    public override EntityBaseState Splodge()
    {
        throw new System.NotImplementedException();
    }

    public override EntityBaseState Spawn()
    {
        throw new System.NotImplementedException();
    }
}