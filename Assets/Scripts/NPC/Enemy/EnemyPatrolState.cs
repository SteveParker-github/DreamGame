using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public EnemyPatrolState(EnemyController enemyController, EnemyStateFactory enemyStateFactory)
    : base(enemyController, enemyStateFactory)
    { }

    private Vector3 targetLocation;

    public override void EnterState()
    {
        ctx.Agent.isStopped = false;
        ctx.WasPatrol = true;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Patrol();
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

        if (ctx.IsStagger)
        {
            SwitchState(factory.EnemyStaggerState());
            return;
        }

        if (ctx.PlayerSeen)
        {
            SwitchState(factory.EnemyChaseState());
            return;
        }
    }

    private void Patrol()
    {
        if (ctx.Agent.remainingDistance < 0.5f) targetLocation = Vector3.zero;

        if (targetLocation == Vector3.zero)
        {
            targetLocation = ctx.PatrolPoints[Random.Range(0, ctx.PatrolPoints.Count)];
            ctx.Agent.destination = targetLocation;
        }
    }
}
