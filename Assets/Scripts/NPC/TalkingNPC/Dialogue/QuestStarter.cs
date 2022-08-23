using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStarter : Conversation
{
    protected string questName;
    protected QuestManager questManager;

    public string QuestName { get => questName; }
    public QuestStarter(string option, string respond, bool isEnd, string questName, QuestManager questManager)
    : base(option, respond, isEnd)
    {
        this.questName = questName;
        this.questManager = questManager;

    }

    public override string GetRespond()
    {
        questManager.NewQuest(questName, new Quest(questName));
        isRepeat = true;
        return respond;
    }

    public override bool IsAvailable()
    {
        return !questManager.QuestExist(questName);
    }
}
