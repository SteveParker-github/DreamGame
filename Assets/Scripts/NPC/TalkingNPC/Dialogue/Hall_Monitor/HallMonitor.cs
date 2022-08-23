using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallMonitor : DialogueController
{
    protected override void CreateConversations(
        QuestManager questManager, 
        InventoryManager inventoryManager, 
        DialogueManager dialogueManager,
        PlayerController playerController)
    {
        conversations = new Dictionary<string, Conversation>
        {
            {"How are you?", new Conversation("How are you?", "I'm busy, get back to class.", false)},
            {"Where is everyone?", new Conversation("Where is everyone?", "They're at the assembily.", false)},
            {"I need to get outside.", new QuestStarter("I need to get outside.", "Get a hall pass or I can't let you go.", false, "Fetch Hall Pass", questManager)},
            {"How can I get outside again?", new QuestProgress("How can I get outside again?", "I need to see your hall pass", false, "Fetch Hall Pass", questManager, "Hall Pass", inventoryManager)},
            {"Here is my hall pass.", new QuestEnd("Here is my hall pass.", "Alright you can go out", false, "Fetch Hall Pass", questManager, "Hall Pass", inventoryManager)},
            {"Thanks.", new Conversation("Thanks.", "Your welcome.", true)},
        };
    }
}
