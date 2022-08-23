using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPrepareTalkState : NPCBaseState
{
    public NPCPrepareTalkState(NPCController nPCController, NPCStateFactory nPCStateFactory)
    : base(nPCController, nPCStateFactory)
    { }

    private const float MAXROTATIONSPEED = 2.0f;
    private bool isStateComplete;

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        RotateTowardsNPC();
        CheckSwitchState();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchState()
    {
        if (isStateComplete)
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
            isStateComplete = true;
        }
    }
}
