[System.Serializable]
public class PlayerConversation
{
    public string NPCName;
    public string[] conversations;

    public PlayerConversation(string NPCName, string[] conversations)
    {
        this.NPCName = NPCName;
        this.conversations = conversations;
    }
}
