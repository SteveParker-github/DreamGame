using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    private Dictionary<string, List<string>> previousConversations;
    // Start is called before the first frame update
    void Start()
    {
        previousConversations = new Dictionary<string, List<string>>();
    }

    public void AddMessage(string characterName, string message)
    {
        string sceneName = GetSceneName();

        if (previousConversations.ContainsKey(characterName))
        {
            if (previousConversations[characterName].Contains(message)) return;
            previousConversations[characterName].Add(message);
            return;
        }

        previousConversations.Add(characterName, new List<string> { message });
    }

    private string GetSceneName()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            string sceneName = SceneManager.GetSceneAt(i).name;
            if (sceneName != "GlobalScene" && sceneName != "MainMenuScene")
            {
                return sceneName;
            }
        }
        return "";
    }

    public bool CharacterNotExists(string CharacterName)
    {
        return !previousConversations.ContainsKey(CharacterName);
    }

    public List<string> GetConversations(string characterName)
    {
        return previousConversations[characterName];
    }

    public DialogueSave GetDialogue()
    {
        DialogueSave dialogueSave = new DialogueSave();

        List<PlayerConversation> playerConversations = new List<PlayerConversation>();

        foreach (KeyValuePair<string, List<string>> item in previousConversations)
        {
            playerConversations.Add(new PlayerConversation(item.Key, item.Value.ToArray()));
        }

        dialogueSave.playerConversations = playerConversations.ToArray();

        return dialogueSave;
    }

    public void LoadDialogues(DialogueSave newDialogue)
    {
        previousConversations = new Dictionary<string, List<string>>();

        foreach (PlayerConversation item in newDialogue.playerConversations)
        {
            previousConversations.Add(item.NPCName, new List<string>(item.conversations));
        }
    }

    public bool ConversationExist(string NPCName, string conversation)
    {
        if (CharacterNotExists(NPCName))
        {
            return false;
        }

        return previousConversations[NPCName].Contains(conversation);
    }
}
