using UnityEngine;
public abstract class EnemyBaseState
{
    protected EnemyController ctx;
    protected EnemyStateFactory factory;

    public EnemyBaseState(EnemyController currentContext, EnemyStateFactory enemyStateFactory)
    {
        ctx = currentContext;
        factory = enemyStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    protected void SwitchState(EnemyBaseState newState)
    {
        ExitState();

        newState.EnterState();

        ctx.CurrentState = newState;
    }
}