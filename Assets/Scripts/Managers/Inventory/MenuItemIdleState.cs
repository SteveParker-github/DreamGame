using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuIdleState : MenuBaseState
{
    public MenuIdleState(InventoryManager inventoryManager, MenuStateFactory menuStateFactory)
    : base(inventoryManager, menuStateFactory)
    { }

    public override void EnterState()
    {
        ctx.InventoryMenu.SetActive(false);
        Debug.Log("Enter Idle system");
        ctx.IsFinished = true;
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        Debug.Log("Exit Idle system");
    }
    public override void CheckSwitchState()
    {
    }
}
