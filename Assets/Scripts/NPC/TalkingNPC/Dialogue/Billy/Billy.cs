using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billy : DialogueController
{
    protected override void CreateConversations(
        QuestManager questManager,
        InventoryManager inventoryManager,
        DialogueManager dialogueManager,
        PlayerController playerController)
    {
        conversations = new Dictionary<string, Conversation>
        {
            {"Billy, I'm Detective Jim Harris.", new SuspiciousConversation(
                "Billy, I'm Detective Jim Harris.",
                "I'm not talking to a cop!",
                false,
                playerController
            )},
            {"Billy we need to talk.", new FollowUpSuspiciousConversation(
                "Billy we need to talk.",
                "I said I'm not talking to you!",
                false,
                playerController,
                "Billy",
                "Billy, I'm Detective Jim Harris.",
                dialogueManager)},
            {"Billy I know about the fire.", new RequireItemsQuestStarter(
                "Billy I know about the fire.",
                "My rage keeps growing!",
                false,
                "Kill Billy's monsters",
                questManager,
                new string[] {"Counsellor's Notes"},
                playerController)}, //Triggers once the lighter and notes have been placed in inventory. Triggers shadow monsters to attack
            {"Billy what's wrong?", new QuestProgress(
                "Billy what's wrong?",
                "My emotions are out of control!",
                false,
                "Kill Billy's monsters",
                questManager,
                "Billy's Emotion",
                inventoryManager)}, //Only triggered once monsters are active
            {"Billy I understand now. Your anger is out of control. Think back about Lauren and how she made you happy.", new QuestEnd(
                "Billy I understand now. Your anger is out of control. Think back about Lauren and how she made you happy.",
                "What... Lauren! Those were happier times.",
                false,
                "Kill Billy's monsters",
                questManager,
                "Billy's Emotion",
                inventoryManager)}, //This option appaears once emotion has been discovered
            {"Thank god you have calmed down.", new FollowUpConversation(
                "Thank god you have calmed down.",
                "Sorry my emotions are all over the place.",
                false,
                "Billy",
                "Billy I understand now. Your anger is out of control. Think back about Lauren and how she made you happy.",
                dialogueManager)}, //Appears once Billy calms down
            {"Your counselling notes suggest something has been going on with your family can you elaborate?", new ReceiveItemFollowUpConversation(
                "Your counselling notes suggest something has been going on with your family can you elaborate?",
                "All I know is dad is sleeping in the spare room and Lisa is sad all the time",
                false,
                ("Billy's Statement", "Billy: \"All I know is dad is sleeping in the spare room and Lisa is sad all the time\""),
                inventoryManager,
                "Billy",
                "Billy I understand now. Your anger is out of control. Think back about Lauren and how she made you happy.",
                dialogueManager)}, //Must have couselling notes and billy calm. this statement clue goes into inventory
            {"Thanks kid.", new Conversation(
                "Thanks kid.",
                "That's alright Mister.",
                true)},
        };
    }
}
