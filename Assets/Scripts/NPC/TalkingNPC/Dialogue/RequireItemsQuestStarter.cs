using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequireItemsQuestStarter : QuestStarter
{
    private string[] items;
    private PlayerController playerController;
    public RequireItemsQuestStarter(string option, string respond, bool isEnd, (string, string) quest, QuestManager questManager, string[] items, PlayerController playerController)
    : base(option, respond, isEnd, quest, questManager)
    {
        this.items = items;
        this.playerController = playerController;
    }

    public override bool IsAvailable()
    {
        if (questManager.QuestExist(quest.Item1))
        {
            return false;
        }

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
