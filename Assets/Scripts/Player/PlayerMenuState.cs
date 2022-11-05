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
        ctx.GameManager.IsPaused = true;
        ctx.UIManager.ToggleQuestTracker();
        if (ctx.UIManager.IsItemViewActive())
        {
            ctx.InventoryManager.ShowItemDetail();
        }
        else
        {
            ctx.InventoryManager.DisplayItems();
        }
        ctx.AmbientSound.Pause();
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        ctx.IsPaused = false;
        ctx.UIManager.ToggleQuestTracker();
        ctx.GameManager.IsPaused = false;
        ctx.AmbientSound.UnPause();
    }
    public override void CheckSwitchState()
    {
        if (ctx.InventoryManager.IsFinished)
        {
            SwitchState(factory.PlayerWalkingState());
        }
    }
}