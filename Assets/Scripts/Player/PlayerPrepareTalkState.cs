using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrepareTalkState : PlayerBaseState
{
    public PlayerPrepareTalkState(PlayerController playerController, PlayerStateFactory playerStateFactory)
    : base(playerController, playerStateFactory)
    { }

    private const float MAXROTATIONSPEED = 1.0f;

    public override void EnterState()
    {
        ctx.OptionLocation = 0;
        ctx.ReadyToTalk = false;
    }

    public override void UpdateState()
    {
        if (!ctx.ReadyToTalk)
        {
            RotateTowardsNPC();
        }
        
        CheckSwitchState();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchState()
    {
        if (ctx.ReadyToTalk && ctx.SelectedNPC.ReadyToTalk)
        {
            SwitchState(factory.PlayerTalkingState());
        }
    }

    private void RotateTowardsNPC()
    {
        Vector3 targetDirection = ctx.TargetPosition - ctx.MainCamera.transform.position;

        Vector3 playerDirection = targetDirection;
        playerDirection.y = 0.0f;

        float rotationSpeed = MAXROTATIONSPEED * Time.deltaTime;

        Vector3 newCameraDirection = Vector3.RotateTowards(ctx.MainCamera.transform.forward, targetDirection, rotationSpeed, 0.0f);
        Vector3 newPlayerDirection = Vector3.RotateTowards(ctx.transform.forward, playerDirection, rotationSpeed, 0.0f);

        ctx.transform.rotation = Quaternion.LookRotation(newPlayerDirection);
        ctx.MainCamera.transform.rotation = Quaternion.LookRotation(newCameraDirection);

        if (Vector3.Angle(ctx.MainCamera.transform.forward, targetDirection) < 1.0f)
        {
            ctx.ReadyToTalk = true;
            Quaternion cameraRotation = ctx.MainCamera.transform.localRotation;
            ctx.MainCamera.transform.localRotation = Quaternion.Euler(cameraRotation.x, 0.0f, 0.0f);
        }
    }
}