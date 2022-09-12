using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUpRequireItemsConversation : FollowUpConversation
{
    string[] items;
    InventoryManager inventoryManager;

    public FollowUpRequireItemsConversation(string option, string respond, bool isEnd, string NPCName, string conversation, DialogueManager dialogueManager, string[] items, InventoryManager inventoryManager)
    : base(option, respond, isEnd, NPCName, conversation, dialogueManager)
    {
        this.items = items;
        this.inventoryManager = inventoryManager;
    }

    public override bool IsAvailable()
    {
        foreach (string item in items)
        {
            if (!inventoryManager.Exist(item))
            {
                return false;
            }
        }

        return dialogueManager.ConversationExist(NPCName, conversation);
    }
}
