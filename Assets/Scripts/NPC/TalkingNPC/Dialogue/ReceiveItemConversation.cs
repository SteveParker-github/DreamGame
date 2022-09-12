using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveItemConversation : Conversation
{
    protected (string, string) item;
    protected InventoryManager inventoryManager;

    public ReceiveItemConversation(string option, string respond, bool isEnd, (string, string) item, InventoryManager inventoryManager)
    : base(option, respond, isEnd)
    {
        this.item = item;
        this.inventoryManager = inventoryManager;
    }

    public override string GetRespond()
    {
        if (isRepeat) return respond;

        inventoryManager.AddNewItem(item);
        isRepeat = true;
        return respond;
    }
}
