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
        ctx.IsFinished = true;
    }
    public override void UpdateState()
    {
    }
    public override void ExitState()
    {
    }
    public override void CheckSwitchState()
    {
    }
}
