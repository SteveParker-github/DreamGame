using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    private string questName;
    private string status;

    public string Status { get => status; set => status = value; }
    public string QuestName { get => questName; }

    public Quest(string questName)
    {
        this.questName = questName;
        status = "inProgress";
    }
}
