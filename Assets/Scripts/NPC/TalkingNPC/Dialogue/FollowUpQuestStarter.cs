using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUpQuestStarter : QuestStarter
{
    protected string NPCName;
    protected string conversation;
    protected DialogueManager dialogueManager;

    public FollowUpQuestStarter(string option, string respond, bool isEnd, (string, string) quest, QuestManager questManager, string NPCName, string conversation, DialogueManager dialogueManager)
    : base(option, respond, isEnd, quest, questManager)
    {
        this.NPCName = NPCName;
        this.conversation = conversation;
        this.dialogueManager = dialogueManager;

    }

    public override string GetRespond()
    {
        questManager.NewQuest(quest.Item1, new Quest(quest.Item1, quest.Item2));
        isRepeat = true;
        return respond;
    }

    public override bool IsAvailable()
    {
        return !questManager.QuestExist(quest.Item1) && dialogueManager.ConversationExist(NPCName, conversation);
    }
}
