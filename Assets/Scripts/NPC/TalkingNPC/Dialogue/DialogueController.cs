using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueController : MonoBehaviour
{
    private DialogueManager dialogueManager;
    protected Dictionary<string, Conversation> conversations;

    public void Setup(QuestManager questManager, InventoryManager inventoryManager, PlayerController playerController)
    {
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        CreateConversations(questManager, inventoryManager, dialogueManager, playerController);
        UpdateStatus();
    }

    public Dictionary<string, Conversation> GetConversations()
    {
        Dictionary<string, Conversation> newOptions = new Dictionary<string, Conversation>();
        foreach (KeyValuePair<string, Conversation> conversation in conversations)
        {
            if (!conversation.Value.IsAvailable()) continue;

            newOptions.Add(conversation.Key, conversation.Value);
        }
        return newOptions;
    }
    protected abstract void CreateConversations(QuestManager questManager, InventoryManager inventoryManager, DialogueManager dialogueManager, PlayerController playerController);

    protected void UpdateStatus()
    {
        string characterName = GetComponent<NPCController>().CharacterName;

        if (dialogueManager.CharacterNotExists(characterName)) return;

        List<string> previousConversations = dialogueManager.GetConversations(characterName);
        foreach (string conversation in previousConversations)
        {
            conversations[conversation].ToggleRepeat();
        }
    }

}
