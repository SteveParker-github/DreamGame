using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    private Dictionary<string, Quest> quests;
    private void Awake()
    {
        quests = new Dictionary<string, Quest>();
    }

    public void NewQuest(string questName, Quest newQuest)
    {
        quests.Add(questName, newQuest);
    }

    public bool QuestExist(string questName)
    {
        return quests.ContainsKey(questName);
    }

    public bool QuestMatchStatus(string questName, string status)
    {
        bool result = false;

        if (QuestExist(questName))
        {
            result = quests[questName].Status == status;
        }

        return result;
    }

    public void UpdateQuestStatus(string questName, string newStatus)
    {
        if(!QuestExist(questName)) return;

        quests[questName].Status = newStatus;
    }

    public List<(string, string)> GetBasicQuestList()
    {
        List<(string, string)> questList = new List<(string, string)>();

        foreach (KeyValuePair<string, Quest> item in quests)
        {
            questList.Add((item.Value.QuestName, item.Value.Status));
        }
        
        return questList;
    }

    public bool QuestInProgress(string questName)
    {
        if (!QuestExist(questName)) return false;

        return quests[questName].Status == "inProgress";
    }

    public void FinishQuest(string questName)
    {
       if (!QuestExist(questName)) return;

       quests[questName].Status = "Complete"; 
    }

    // provides all the information for the save game
    public QuestSave[] GetFullQuestList()
    {
        List<QuestSave> questList = new List<QuestSave>();

        foreach (KeyValuePair<string, Quest> item in quests)
        {
            questList.Add(new QuestSave(item.Value.QuestName, item.Value.Status));
        }

        return questList.ToArray();
    }

    public void LoadQuests(QuestSave[] newQuests)
    {
        quests = new Dictionary<string, Quest>();

        foreach (QuestSave quest in newQuests)
        {
            quests.Add(quest.questName, new Quest(quest.questName));
            quests[quest.questName].Status = quest.questProgress;
        }
    }
}
