using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestProgress : QuestStarter
{
    protected string[] items;
    protected InventoryManager inventoryManager;

    public QuestProgress(string option, string respond, bool isEnd, (string, string) quest, QuestManager questManager, string[] items, InventoryManager inventoryManager)
    : base(option, respond, isEnd, quest, questManager)
    {
        this.items = items;
        this.inventoryManager = inventoryManager;
    }

    public override string GetRespond()
    {
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
                return true;
            }
        }

        return false;
    }
}
