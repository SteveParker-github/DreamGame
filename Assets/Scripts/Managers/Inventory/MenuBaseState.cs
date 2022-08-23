using UnityEngine;
public abstract class MenuBaseState
{
    protected InventoryManager ctx;
    protected MenuStateFactory factory;

    public MenuBaseState(InventoryManager currentContext, MenuStateFactory playerStateFactory)
    {
        ctx = currentContext;
        factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    protected void SwitchState(MenuBaseState newState)
    {
        ExitState();

        newState.EnterState();

        ctx.CurrentState = newState;
    }
}