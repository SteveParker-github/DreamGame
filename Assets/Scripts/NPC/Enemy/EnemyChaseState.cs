using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyController enemyController, EnemyStateFactory enemyStateFactory)
    : base(enemyController, enemyStateFactory)
    { }

    private Vector3 lastLocation;
    private bool playerLost;
    private float hitCooldown;
    private float playerMissingCooldown;

    public override void EnterState()
    {
        ctx.Agent.isStopped = false;
        ctx.WasPatrol = false;
        lastLocation = ctx.PlayerLocation.position;
        playerLost = false;
        hitCooldown = 0.0f;
    }

    public override void UpdateState()
    {
        if (ctx.PlayerSeen)
        {
            Chase();
            playerMissingCooldown = 10.0f;
        }
        else
        {
            GoLastLocation();
        }
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

        if (playerLost)
        {
            SwitchState(factory.EnemyPatrolState());
            return;
        }
    }

    private void Chase()
    {
        lastLocation = ctx.PlayerLocation.position;

        if (Vector3.Distance(ctx.transform.position, ctx.PlayerLocation.position) < 2.0f)
        {
            ctx.Agent.destination = ctx.transform.position;
            if (HitPlayer()) return;
        }

        ctx.Agent.destination = lastLocation;
    }

    private void GoLastLocation()
    {
        if (ctx.Agent.remainingDistance < 0.5f || playerMissingCooldown <= 0.0f)
        {
            playerLost = true;
            return;
        }

        playerMissingCooldown -= Time.deltaTime;
        ctx.Agent.destination = lastLocation;
    }

    private bool HitPlayer()
    {
        if (hitCooldown > 0.0f)
        {
            hitCooldown -= Time.deltaTime;
            return false;
        }

        ctx.PlayerController.Hit();
        hitCooldown = 1.0f;
        return true;
    }
}