using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequireItemsReceiveItemConversation : RequireItemsConversation
{
    private (string, string) receiveItem;
    public RequireItemsReceiveItemConversation(string option, string respond, bool isEnd, string[] items, PlayerController playerController, (string, string) receiveItem)
    : base(option, respond, isEnd, items, playerController)
    {
        this.receiveItem = receiveItem;
    }

    public override string GetRespond()
    {
        if (isRepeat) return respond;

        playerController.InventoryManager.AddNewItem(receiveItem);
        isRepeat = true;
        return respond;
    }
}
