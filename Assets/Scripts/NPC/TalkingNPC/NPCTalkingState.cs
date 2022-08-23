using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalkingState : NPCBaseState
{
    public NPCTalkingState(NPCController nPCController, NPCStateFactory nPCStateFactory)
    : base(nPCController, nPCStateFactory)
    { }

    private Dictionary<string, Conversation> conversations;
    private Dictionary<string, bool> options;

    public override void EnterState()
    {
        conversations = new Dictionary<string, Conversation>();

        NewOptions();
        ctx.PlayerController.UIManager.ChangeCaption("Hello Player", false);
    }

    public override void UpdateState()
    {
        if (ctx.PlayerController.Message != null)
        {
            Respond();
        }

        CheckSwitchState();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchState()
    {
        if (!ctx.PlayerController.IsTalkState)
        {
            ctx.EnterTalkState = false;
            SwitchState(factory.NPCRoamingState());
        }
    }

    private void Respond()
    {
        string message = ctx.PlayerController.Message;
        ctx.DialogueManager.AddMessage(ctx.CharacterName, message);
        ctx.PlayerController.UIManager.ChangeCaption(conversations[message].GetRespond(), conversations[message].GetIsEnd());
        ctx.PlayerController.Message = null;
        NewOptions();
    }

    private void NewOptions()
    {
        conversations = ctx.DialogueController.GetConversations();
        options = new Dictionary<string, bool>();

        foreach (KeyValuePair<string, Conversation> conversation in conversations)
        {
            options.Add(conversation.Key, conversation.Value.GetIsRepeat());
        }

        ctx.PlayerController.UIManager.DialogOptions(options);
    }
}
