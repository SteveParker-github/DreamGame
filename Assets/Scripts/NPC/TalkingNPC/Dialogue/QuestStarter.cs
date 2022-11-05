using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStarter : Conversation
{
    protected (string, string) quest;
    protected QuestManager questManager;

    public (string, string) Quest { get => quest; }
    public QuestStarter(string option, string respond, bool isEnd, (string, string) quest, QuestManager questManager)
    : base(option, respond, isEnd)
    {
        this.quest = quest;
        this.questManager = questManager;

    }

    public override string GetRespond()
    {
        questManager.NewQuest(quest.Item1, new Quest(quest.Item1, quest.Item2));
        isRepeat = true;
        return respond;
    }

    public override bool IsAvailable()
    {
        return !questManager.QuestExist(quest.Item1);
    }
}
