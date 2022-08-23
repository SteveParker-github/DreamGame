using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListeningState : PlayerBaseState
{
    public PlayerListeningState(PlayerController playerController, PlayerStateFactory playerStateFactory)
    : base(playerController, playerStateFactory)
    { }

    private const float MAXROTATIONSPEED = 1.0f;
    private bool isStateComplete;

    public override void EnterState()
    {
        ctx.UIManager.ToggleCaption();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void ExitState()
    {
        ctx.UIManager.ToggleCaption();
    }

    public override void CheckSwitchState()
    {
        if (ctx.IsInteractInput)
        {
            ctx.IsInteractInput = false;
            if (ctx.UIManager.IsEnd())
            {
                ctx.IsTalkState = false;
                SwitchState(factory.PlayerWalkingState());
                return;
            }
            SwitchState(factory.PlayerTalkingState());
            return;
        }
    }
}