using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunState : EnemyBaseState
{
    public EnemyStunState(EnemyController enemyController, EnemyStateFactory enemyStateFactory)
    : base(enemyController, enemyStateFactory)
    { }

    public override void EnterState()
    {
        ctx.Agent.isStopped = true;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchState()
    {
    }

}
