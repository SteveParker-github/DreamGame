using UnityEngine;
public abstract class NPCBaseState
{
    protected NPCController ctx;
    protected NPCStateFactory factory;

    public NPCBaseState(NPCController currentContext, NPCStateFactory playerStateFactory)
    {
        ctx = currentContext;
        factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    protected void SwitchState(NPCBaseState newState)
    {
        ExitState();

        newState.EnterState();

        ctx.CurrentState = newState;
    }
}