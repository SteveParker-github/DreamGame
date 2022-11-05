using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEnd : QuestStarter
{
    protected string[] items;
    protected InventoryManager inventoryManager;

    public QuestEnd(string option, string respond, bool isEnd, (string, string) quest, QuestManager questManager, string[] items, InventoryManager inventoryManager)
    : base(option, respond, isEnd, quest, questManager)
    {
        this.items = items;
        this.inventoryManager = inventoryManager;
    }
    public override string GetRespond()
    {
        questManager.FinishQuest(quest.Item1);

        foreach (string item in items)
        {
            inventoryManager.DestroyItem(item);
        }

        isRepeat = true;
        return respond;
    }

    public override bool IsAvailable()
    {
        if (!questManager.QuestInProgress(quest.Item1)) return false;

        foreach (string item in items)
        {
            if (!inventoryManager.Exist(item))
            {
                return false;
            }
        }

        return true;
    }
}
