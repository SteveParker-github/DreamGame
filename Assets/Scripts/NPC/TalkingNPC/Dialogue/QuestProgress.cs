using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestProgress : QuestStarter
{
    protected string itemName;
    protected InventoryManager inventoryManager;

    public QuestProgress(string option, string respond, bool isEnd, string questName, QuestManager questManager, string itemName, InventoryManager inventoryManager)
    : base(option, respond, isEnd, questName, questManager)
    {
        this.itemName = itemName;
        this.inventoryManager = inventoryManager;
    }

    public override string GetRespond()
    {
        isRepeat = true;
        return respond;
    }

    public override bool IsAvailable()
    {
        bool result = questManager.QuestInProgress(questName) && !inventoryManager.InPossesion(itemName);
        return result;
    }
}
