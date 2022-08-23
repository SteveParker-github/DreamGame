using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryMenuState : PlayerBaseState
{
    public PlayerInventoryMenuState(PlayerController playerController, PlayerStateFactory playerStateFactory)
    : base(playerController, playerStateFactory)
    { }

    public override void EnterState()
    {
        ctx.IsPaused = true;
        ctx.InventoryManager.ToggleInventoryMenu();
        ctx.InventoryManager.DisplayItems();
        Debug.Log("Player is in the inventory menu");
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        ctx.IsPaused = false;
        ctx.InventoryManager.ToggleInventoryMenu();
    }
    public override void CheckSwitchState()
    {
        if (ctx.IsInventoryMenuInput)
        {
            ctx.IsInventoryMenuInput = false;
            SwitchState(factory.PlayerWalkingState());
        }
    }
}