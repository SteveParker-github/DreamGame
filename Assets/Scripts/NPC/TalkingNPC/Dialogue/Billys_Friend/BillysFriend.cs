using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillysFriend : DialogueController
{
    protected override void CreateConversations(
        QuestManager questManager, 
        InventoryManager inventoryManager, 
        DialogueManager dialogueManager,
        PlayerController playerController)
    {
        conversations = new Dictionary<string, Conversation>
        {
            {"Who are you kid?", new Conversation(
                "Who are you kid?", 
                "I'm Billy's friend Keith.", 
                false)}, //trigger positive interaction?
            {"Hey kid what do you know about Billy?", new SuspiciousConversation(
                "Hey kid what do you know about Billy?", 
                "I don't know you please go away", 
                false,
                playerController)}, //trigger eye to be sus
            {"Can you tell me about Billy's personal life?", new Conversation(
                "Can you tell me about Billy's personal life?", 
                "All I know is he has been seeing the counsellor", 
                false)}, //trigger +ve interaction with counsellor
            {"Thanks kid.", new Conversation(
                "Thanks kid.", 
                "That's alright Mister.", 
                true)},
        };
    }
}
