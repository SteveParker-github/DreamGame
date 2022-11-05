using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPrepareTalkState : NPCBaseState
{
    public NPCPrepareTalkState(NPCController nPCController, NPCStateFactory nPCStateFactory)
    : base(nPCController, nPCStateFactory)
    { }

    private const float MAXROTATIONSPEED = 2.0f;

    public override void EnterState()
    {
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
        if (ctx.ReadyToTalk)
        {
            SwitchState(factory.NPCTalkingState());
        }
    }

    private void RotateTowardsNPC()
    {
        Vector3 targetDirection = ctx.MainPlayer.transform.position - ctx.transform.position;
        targetDirection.y = 0.0f;

        float rotationSpeed = MAXROTATIONSPEED * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(ctx.transform.forward, targetDirection, rotationSpeed, 0.0f);

        ctx.transform.rotation = Quaternion.LookRotation(newDirection);

        if (Vector3.Angle(ctx.transform.forward, targetDirection) < 1.0f)
        {
            ctx.ReadyToTalk = true;
        }
    }
}
