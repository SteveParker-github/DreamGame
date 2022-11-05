using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    private Dictionary<string, Quest> quests;
    private string currentQuest;
    private void Awake()
    {
        quests = new Dictionary<string, Quest>();
    }

    public void NewQuest(string questName, Quest newQuest)
    {
        quests.Add(questName, newQuest);
        if (questName == "Kill Billy's monsters")
        {
            GameObject.Find("ClockTower").GetComponent<AudioSource>().Play();
        }
        currentQuest = questName;
    }

    public bool QuestExist(string questName)
    {
        return quests.ContainsKey(questName);
    }

    public bool QuestMatchStatus(string questName, string status)
    {
        if (!QuestExist(questName)) return false;

        return quests[questName].Status == status;
    }

    public void UpdateQuestStatus(string questName, string newStatus)
    {
        if (!QuestExist(questName)) return;

        quests[questName].Status = newStatus;
    }

    public void UpdateQuestDescription(string questName, string description)
    {
        if (!QuestExist(questName)) return;

        quests[questName].Description = description;
    }

    public List<Quest> GetBasicQuestList()
    {
        return new List<Quest>(quests.Values);
    }

    public bool QuestInProgress(string questName)
    {
        if (!QuestExist(questName)) return false;

        return quests[questName].Status == "inProgress";
    }

    public void FinishQuest(string questName)
    {
        if (!QuestExist(questName)) return;

        quests[questName].Description = "";
        quests[questName].Status = "Complete";

        if (currentQuest == questName)
        {
            GetNewCurrentQuest();
        }
    }

    // provides all the information for the save game
    public QuestSave[] GetFullQuestList()
    {
        List<QuestSave> questList = new List<QuestSave>();

        foreach (KeyValuePair<string, Quest> item in quests)
        {
            questList.Add(new QuestSave(item.Value.QuestName, item.Value.Status, item.Value.Description));
        }

        return questList.ToArray();
    }

    public string GetCurrentQuest()
    {
        return currentQuest;
    }

    public (string, string) GetCurrentQuestInfo()
    {
        if (currentQuest == null || !QuestExist(currentQuest)) return ("", "");

        return (quests[currentQuest].QuestName, quests[currentQuest].Description);
    }

    public void newCurrentQuest(string questName)
    {
        currentQuest = questName;
    }

    private void GetNewCurrentQuest()
    {
        foreach (KeyValuePair<string, Quest> item in quests)
        {
            if (item.Value.Status == "InProgress")
            {
                currentQuest = item.Key;
                return;
            }
        }

        currentQuest = "";
    }

    public void LoadQuests(QuestSave[] newQuests, string currentQuest)
    {
        quests = new Dictionary<string, Quest>();

        foreach (QuestSave quest in newQuests)
        {
            quests.Add(quest.questName, new Quest(quest.questName, quest.description));
            quests[quest.questName].Status = quest.questProgress;
        }

        this.currentQuest = currentQuest;
    }
}
