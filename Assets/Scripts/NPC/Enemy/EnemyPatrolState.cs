using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyBaseState
{
    public EnemyPatrolState(EnemyController enemyController, EnemyStateFactory enemyStateFactory)
    : base(enemyController, enemyStateFactory)
    { }

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
        if (ctx.HadPaused)
        {
            ctx.Agent.destination = ctx.TargetLocation;
            ctx.HadPaused = false;
            return;
        }

        if (ctx.Agent.remainingDistance <= ctx.Agent.stoppingDistance)
        {
            ctx.TargetLocation = ctx.PatrolPoints[Random.Range(0, ctx.PatrolPoints.Count)];
            ctx.Agent.destination = ctx.TargetLocation;
        }
    }
}
