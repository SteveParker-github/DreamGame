using UnityEngine;
public abstract class PlayerBaseState
{
    protected PlayerController ctx;
    protected PlayerStateFactory factory;

    public PlayerBaseState(PlayerController currentContext, PlayerStateFactory playerStateFactory)
    {
        ctx = currentContext;
        factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    protected void SwitchState(PlayerBaseState newState)
    {
        ExitState();

        newState.EnterState();

        ctx.CurrentState = newState;
    }
}