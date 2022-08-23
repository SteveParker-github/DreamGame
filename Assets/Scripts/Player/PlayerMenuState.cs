using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuState : PlayerBaseState
{
    public PlayerMenuState(PlayerController playerController, PlayerStateFactory playerStateFactory)
    : base(playerController, playerStateFactory)
    { }

    public override void EnterState()
    {
        ctx.IsPaused = true;
        ctx.InventoryManager.DisplayItems();
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        ctx.IsPaused = false;
    }
    public override void CheckSwitchState()
    {
        if (ctx.InventoryManager.IsFinished)
        {
            SwitchState(factory.PlayerWalkingState());
        }
    }
}