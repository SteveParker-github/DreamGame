using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveItemFollowUpConversation : ReceiveItemConversation
{
    protected string NPCName;
    protected string conversation;
    protected DialogueManager dialogueManager;

    public ReceiveItemFollowUpConversation(string option, string respond, bool isEnd, (string, string) item, InventoryManager inventoryManager, string NPCName, string conversation, DialogueManager dialogueManager)
    : base(option, respond, isEnd, item, inventoryManager)
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
