[System.Serializable]
public class QuestSave
{
    public string questName;
    public string questProgress;
    public QuestSave(string questName, string questProgress)
    {
        this.questName = questName;
        this.questProgress = questProgress;
    }

    public (string, string) ToTuple()
    {
        return (questName, questProgress);
    }
}