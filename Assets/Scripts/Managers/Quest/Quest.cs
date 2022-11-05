using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    private string questName;
    private string status;
    private string description;

    public string Status { get => status; set => status = value; }
    public string QuestName { get => questName; }
    public string Description { get => description; set => description = value; }

    public Quest(string questName, string description)
    {
        this.questName = questName;
        status = "inProgress";
        this.description = description;
    }
}
