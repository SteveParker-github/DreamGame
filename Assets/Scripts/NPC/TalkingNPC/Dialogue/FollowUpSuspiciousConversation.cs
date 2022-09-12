using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUpSuspiciousConversation : SuspiciousConversation
{
    private string NPCName;
    private string conversation;
    private DialogueManager dialogueManager;
    public FollowUpSuspiciousConversation(string option, string respond, bool isEnd, PlayerController playerController, string NPCName, string conversation, DialogueManager dialogueManager)
    : base(option, respond, isEnd, playerController)
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
