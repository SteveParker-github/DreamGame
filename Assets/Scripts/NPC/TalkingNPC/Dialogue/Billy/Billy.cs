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
            {"Billy, I'm Detective Bernard Richards.", new SuspiciousConversation(
                "Billy, I'm Detective Bernard Richards.",
                "I'm not talking to a cop!",
                false,
                playerController
            )},
            {"Billy, we need to talk.", new FollowUpSuspiciousConversation(
                "Billy, we need to talk.",
                "I said I'm not talking to you!",
                false,
                playerController,
                "Billy",
                "Billy, I'm Detective Jim Harris.",
                dialogueManager)},
            {"Billy, please talk to me. I can help. I know what you have been going through.", new FollowUpConversation(
                "Billy, please talk to me. I can help. I know what you have been going through.",
                "Ohh yeah, and what have I been going through?",
                false,
                "Counsellor",
                "Do you know anything else about the family?",
                dialogueManager)},
            {"I know about the fire and how you have been treated since.", new FollowUpQuestStarter(
                "I know about the fire and how you have been treated since.",
                "The fire!!!! I'll show everyone fire...",
                false,
                ("Kill Billy's monsters", "Kill Billy's monsters (0/3)"),
                questManager,
                "Billy",
                "Billy, please talk to me. I can help. I know what you have been going through.",
                dialogueManager)},
            {"Billy, what's happening?", new QuestProgress(
                "Billy, what's happening?",
                "My rage... It grows. It burns.",
                false,
                ("Kill Billy's monsters", ""),
                questManager,
                new string[] {"Billy's Emotion"},
                inventoryManager)},
            {"Billy I understand now. Your anger is out of control. Think back about Lauren and how she made you happy.", new QuestEnd(
                "Billy I understand now. Your anger is out of control. Think back about Lauren and how she made you happy.",
                "What... Lauren! Those were happier times.",
                false,
                ("Kill Billy's monsters", ""),
                questManager,
                new string[] {"Billy's Emotion"},
                inventoryManager)},
            {"Thank god... you have calmed down.", new FollowUpConversation(
                "Thank god... you have calmed down.",
                "Sorry my anger has been controlling me and turning me into a monster.",
                false,
                "Billy",
                "Billy I understand now. Your anger is out of control. Think back about Lauren and how she made you happy.",
                dialogueManager)},
            {"It's ok Billy, you don't have to feel angry all the time, even though there's something going on at home. Your family still love you.", new FollowUpConversation(
                "It's ok Billy, you don't have to feel angry all the time, even though there's something going on at home. Your family still love you.",
                "You're right Detective, My family does love me.",
                false,
                "Billy",
                "Billy I understand now. Your anger is out of control. Think back about Lauren and how she made you happy.",
                dialogueManager)},
            {"Now, Billy, I don't suppose you can elaborate about what has been going on at home?", new ReceiveItemFollowUpConversation(
                "Now, Billy, I don't suppose you can elaborate about what has been going on at home?",
                "Sorry, all I know is dad is sleeping in the spare room and Lisa is sad all the time.",
                false,
                ("Billy's Statement", "Billy: \"All I know is dad is sleeping in the spare room and Lisa is sad all the time\""),
                inventoryManager,
                "Billy",
                "It's ok Billy, you don't have to feel angry all the time, even though there's something going on at home. Your family still love you.",
                dialogueManager)},
            {"Thanks Billy.", new Conversation(
                "Thanks kid.",
                "No, no, thank you for helping me.",
                true)},
        };
    }
}
