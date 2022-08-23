public class EnemyStateFactory
{
    private EnemyController context;
    public EnemyStateFactory(EnemyController currentContext)
    {
        context = currentContext;
    }

    public EnemyBaseState EnemyPatrolState()
    {
        return new EnemyPatrolState(context, this);
    }

    public EnemyBaseState EnemyChaseState()
    {
        return new EnemyChaseState(context, this);
    }

    public EnemyBaseState EnemyStunState()
    {
        return new EnemyStunState(context, this);
    }

    public EnemyBaseState EnemyStaggerState()
    {
        return new EnemyStaggerState(context, this);
    }
}
