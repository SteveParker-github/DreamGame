using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveItemConversation : Conversation
{
    private (string, string) item;
    private InventoryManager inventoryManager;

    public ReceiveItemConversation(string option, string respond, bool isEnd, (string, string) item, InventoryManager inventoryManager)
    : base(option, respond, isEnd)
    {
        this.item = item;
        this.inventoryManager = inventoryManager;
    }

    public override string GetRespond()
    {
        Debug.Log(isRepeat);
        if (isRepeat) return respond;

        inventoryManager.AddNewItem(item);
        isRepeat = true;
        return respond;
    }
}
