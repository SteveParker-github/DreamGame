[System.Serializable]
public class SaveFile
{
    public QuestSave[] quests;
    public string currentQuest;
    public InventorySave inventory;
    public DialogueSave dialogue;
    public string location;
    public PlayerSave player;
    public Enemies enemies;
}
