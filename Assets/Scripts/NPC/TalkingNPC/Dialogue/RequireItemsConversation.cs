using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequireItemsConversation : Conversation
{
    protected string[] items;
    protected PlayerController playerController;
    public RequireItemsConversation(string option, string respond, bool isEnd, string[] items, PlayerController playerController)
    : base(option, respond, isEnd)
    {
        this.items = items;
        this.playerController = playerController;
    }

    public override bool IsAvailable()
    {
        foreach (string item in items)
        {
            if (!playerController.InventoryManager.Exist(item))
            {
                return false;
            }
        }
        
        return true;
    }
}
