using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStaggerState : EnemyBaseState
{
    public EnemyStaggerState(EnemyController enemyController, EnemyStateFactory enemyStateFactory)
    : base(enemyController, enemyStateFactory)
    { }
    private float staggerCooldown;

    public override void EnterState()
    {
        ctx.Agent.isStopped = true;
        staggerCooldown = 1.0f;
    }

    public override void UpdateState()
    {
        staggerCooldown -= Time.deltaTime;
        CheckSwitchState();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchState()
    {
        if (ctx.IsStunned)
        {
            SwitchState(factory.EnemyStunState());
            return;
        }

        if (staggerCooldown <= 0.0f)
        {
            ctx.IsStagger = false;
            if (ctx.WasPatrol)
            {
                SwitchState(factory.EnemyPatrolState());
                return;
            }

            SwitchState(factory.EnemyChaseState());
            return;
        }
    }

}
