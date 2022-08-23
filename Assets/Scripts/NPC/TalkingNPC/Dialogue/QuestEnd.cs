using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEnd : QuestStarter
{
    protected string itemName;
    protected InventoryManager inventoryManager;

    public QuestEnd(string option, string respond, bool isEnd, string questName, QuestManager questManager, string itemName, InventoryManager inventoryManager)
    : base(option, respond, isEnd, questName, questManager)
    {
        this.itemName = itemName;
        this.inventoryManager = inventoryManager;
    }
    public override string GetRespond()
    {
        questManager.FinishQuest(questName);
        inventoryManager.DestroyItem(itemName);
        isRepeat = true;
        return respond;
    }

    public override bool IsAvailable()
    {
        bool result = questManager.QuestInProgress(questName) && inventoryManager.InPossesion(itemName);
        return result;
    }
}
