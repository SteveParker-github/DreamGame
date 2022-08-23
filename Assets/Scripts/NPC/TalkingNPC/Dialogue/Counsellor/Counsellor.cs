using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counsellor : DialogueController
{
    protected override void CreateConversations(
        QuestManager questManager, 
        InventoryManager inventoryManager, 
        DialogueManager dialogueManager,
        PlayerController playerController)
    {
        conversations = new Dictionary<string, Conversation>
        {
            {"Tell me what you know about Billy.", new SuspiciousConversation(
                "Tell me what you know about Billy",
                "That's a private matter",
                false,
                playerController)}, //trigger eye to be sus
            {"Hi I'm a friend of Billy's I need to know what has been going on with him?", new FollowUpConversation(
                "Hi I'm a friend of Billy's I need to know what has been going on with him?",
                "There has been a lot going on with the poor kid.",
                false,
                "Billy's Friend",
                "Can you tell me about Billy's personal life?",
                dialogueManager)}, //Only available after +ve convo with billy's friend.
            {"Can you tell me anything specific?", new ReceiveItemConversation(
                "Can you tell me anything specific?",
                "I think you should look through this.",
                false,
                ("Counsellor's Notes", "The notes the counsellor has written about Billy"),
                inventoryManager)}, //trigger counseling notes to be in player's inventory
            {"Much appreciated.", new Conversation(
                "Much appreciated.",
                "Your welcome.",
                true)},
        };
    }
}
