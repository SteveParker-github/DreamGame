[System.Serializable]
public class QuestSave
{
    public string questName;
    public string questProgress;
    public string description;
    public QuestSave(string questName, string questProgress, string description)
    {
        this.questName = questName;
        this.questProgress = questProgress;
        this.description = description;
    }

    public (string, string, string) ToTuple()
    {
        return (questName, questProgress, description);
    }
}