using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Character : DialogueController
{
    protected override void CreateConversations(
        QuestManager questManager,
        InventoryManager inventoryManager,
        DialogueManager dialogueManager,
        PlayerController playerController)
    {
        conversations = new Dictionary<string, Conversation>
        {
            {"How are you?", new Conversation("How are you?", "I'm fine thanks", false)},
            {"How's the weather?", new Conversation("How's the weather?", "It's looking good to me.", false)},
            {"How can I help?", new QuestStarter("How can I help?", "Bring me a DreamCube", false, ("fetch Dream Cube", "Get the dreamcube"), questManager)},
            {"What am I suppose to do again?", new QuestProgress("What am I suppose to do again?", "sigh... get the Dream Cube for me", false, ("fetch Dream Cube", ""), questManager, new string[] {"Dream Cube"}, inventoryManager)},
            {"Here's the cube.", new QuestEnd("Here's the cube.", "Thanks for that.", false, ("fetch Dream Cube", ""), questManager, new string[] {"Dream Cube"}, inventoryManager)},
            {"Take care.", new Conversation("Take care.", "You too.", true)},
        };
    }
}
