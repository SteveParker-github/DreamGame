using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRoamingState : NPCBaseState
{
    public NPCRoamingState(NPCController nPCController, NPCStateFactory nPCStateFactory)
    : base(nPCController, nPCStateFactory)
    { }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchState()
    {
        if (ctx.EnterTalkState)
        {
            SwitchState(factory.NPCPrepareTalkState());
        }
    }
}
