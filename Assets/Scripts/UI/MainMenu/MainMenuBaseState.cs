using UnityEngine;
public abstract class MainMenuBaseState
{
    protected MainMenuController ctx;
    protected MainMenuStateFactory factory;

    public MainMenuBaseState(MainMenuController currentContext, MainMenuStateFactory mainMenuStateFactory)
    {
        ctx = currentContext;
        factory = mainMenuStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    protected void SwitchState(MainMenuBaseState newState)
    {
        ExitState();

        newState.EnterState();

        ctx.CurrentState = newState;
    }
}