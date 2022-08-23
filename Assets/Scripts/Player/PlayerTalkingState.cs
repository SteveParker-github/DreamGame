using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTalkingState : PlayerBaseState
{
    public PlayerTalkingState(PlayerController playerController, PlayerStateFactory playerStateFactory)
    : base(playerController, playerStateFactory)
    { }
    private const float MAXMOVECOOLDOWN = 0.2f;
    private int maxOptions;
    private float moveCooldown;
    public override void EnterState()
    {
        ctx.OptionLocation = 0;
        ctx.UIManager.ToggleDialogueBox();
        maxOptions = ctx.UIManager.MaxOptions;
        moveCooldown = 0;
    }

    public override void UpdateState()
    {
        if (ctx.MoveInput != Vector2.zero && moveCooldown <= 0)
        {
            MoveOptions();
        }

        moveCooldown -= Time.deltaTime;

        CheckSwitchState();
    }

    public override void ExitState()
    {
        ctx.UIManager.ToggleDialogueBox();
    }

    public override void CheckSwitchState()
    {
        if (ctx.IsInteractInput)
        {
            SelectOption();

            if (ctx.IsTalkState)
            {
                SwitchState(factory.PlayerListeningState());
                return;
            }

            SwitchState(factory.PlayerWalkingState());
        }
    }

    private void MoveOptions()
    {
        int prevOptionLocation = ctx.OptionLocation;
        ctx.OptionLocation += ctx.MoveInput.y > 0.1f ? -1 : 1;
        ctx.OptionLocation = Mathf.Clamp(ctx.OptionLocation, 0, maxOptions - 1);

        moveCooldown = MAXMOVECOOLDOWN;

        if (prevOptionLocation == ctx.OptionLocation) return;

        ctx.UIManager.MoveOptionBox(prevOptionLocation, ctx.OptionLocation);
    }

    private void SelectOption()
    {
        string message = ctx.UIManager.GetMessage(ctx.OptionLocation);

        ctx.IsInteractInput = false;

        if (message == "Leave")
        {
            ctx.IsTalkState = false;
            return;
        }

        ctx.Message = message;
    }
}
