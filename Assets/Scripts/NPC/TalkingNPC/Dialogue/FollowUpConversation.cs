using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUpConversation : Conversation
{
    protected string NPCName;
    protected string conversation;
    protected DialogueManager dialogueManager;

    public FollowUpConversation(string option, string respond, bool isEnd, string NPCName, string conversation, DialogueManager dialogueManager)
    : base(option, respond, isEnd)
    {
        this.NPCName = NPCName;
        this.conversation = conversation;
        this.dialogueManager = dialogueManager;

    }

    public override bool IsAvailable()
    {
        return dialogueManager.ConversationExist(NPCName, conversation);
    }
}
