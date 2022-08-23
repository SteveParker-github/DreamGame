using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUpConversation : Conversation
{
    private string NPCName;
    private string conversation;
    private DialogueManager dialogueManager;

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
