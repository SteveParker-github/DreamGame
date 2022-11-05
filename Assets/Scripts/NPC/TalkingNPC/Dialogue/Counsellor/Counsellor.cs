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
                "That is a private matter. Now please leave.",
                false,
                playerController)}, //trigger eye to be sus
            {"Mr Adler, the school counsellor, right? Scott has asked me to speak with you regarding Billy.", new RequireItemsConversation(
                "Mr Adler, the school counsellor, right? Scott has asked me to speak with you regarding Billy.",
                "Ahh... so Scott has concerns for him too.",
                false,
                new string[] {"Scott's Note"},
                playerController)},
            {"Please, if you have some information about what is happening with Billy, it would be helpful", new ReceiveItemFollowUpConversation(
                "Please, if you have some information about what is happening with Billy, it would be helpful",
                "There is far too much going on just to tell you. I think you should read my notes",
                false,
                ("Counselling notes (1 of 4)", "These notes give me a rundown of everything Billy has been going through for the last six months. I wonder what other clues I can find."),
                inventoryManager,
                "Counsellor", 
                "Mr Adler, the school counsellor, right? Scott has asked me to speak with you regarding Billy.",
                dialogueManager)},
            {"There is only one page?", new FollowUpQuestStarter(
                "There is only one page?",
                "The rest of my notes are in my class. You may pick them up.",
                false,
                ("Collect the Counsellor's notes", "Collect the other notes (0/3)"),
                questManager,                    
                "Counsellor",
                "Please, if you have some information about what is happening with Billy, it would be helpful",
                dialogueManager)},
            {"Where are the notes again?", new QuestProgress(
                "Where are the notes again?",
                "They're in my class.",
                false,
                ("Collect the Counsellor's notes", ""),
                questManager,
                new string[] {
                    "Counselling notes (1 of 4)",
                    "Counselling notes (2 of 4)",
                    "Counselling notes (3 of 4)",
                    "Counselling notes (4 of 4)"},
                inventoryManager)},
            {"Do you know anything else about the family?", new QuestEnd(
                "Do you know anything else about the family?",
                "That is all I know, sorry",
                false,
                ("Collect the Counsellor's notes", ""),
                questManager,
                new string[] {
                    "Counselling notes (1 of 4)",
                    "Counselling notes (2 of 4)",
                    "Counselling notes (3 of 4)",
                    "Counselling notes (4 of 4)"},
                inventoryManager)},
            {"Much appreciated.", new Conversation(
                "Much appreciated.",
                "Your welcome.",
                true)}
        };
    }
}
