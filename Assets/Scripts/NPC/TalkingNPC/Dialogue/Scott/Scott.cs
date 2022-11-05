using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scott : DialogueController
{
    protected override void CreateConversations(
        QuestManager questManager,
        InventoryManager inventoryManager,
        DialogueManager dialogueManager,
        PlayerController playerController)
    {
        conversations = new Dictionary<string, Conversation>
        {
            {"Hey kid, tell me everything you know about Billy?", new SuspiciousConversation(
                "Hey kid, tell me everything you know about Billy?",
                "I don't know you, please go away",
                false,
                playerController)}, //trigger eye to be sus
            {"You wouldn't happen to be friends with Billy, would you?", new Conversation(
                "You wouldn't happen to be friends with Billy, would you?",
                "Yeah I'm his friend, Scott",
                false)}, //trigger positive interaction?
            {"Well, Scott, can you tell me what has been going on with Billy?", new FollowUpConversation(
                "Well, Scott, can you tell me what has been going on with Billy?",
                "Billy doesn't really say too much about what's been going on, but he mentioned he has been seeing Mr Adler, the school counsellor.",
                false,
                "Scott",
                "You wouldn't happen to be friends with Billy, would you?",
                dialogueManager)},
            {"How will I get the counsellor to speak with me?", new ReceiveItemFollowUpConversation(
                "How will I get the counsellor to speak with me?",
                "Here, take this note. The counsellor trusts me, so hopefully he'll trust you now",
                false,
                ("Scott's Note", "These notes give me a rundown of everything Billy has been going through for the last six months. I wonder what other clues I can find"),
                inventoryManager,
                "Scott",
                "Well, Scott, can you tell me what has been going on with Billy?",
                dialogueManager)},
            {"Thanks kid.", new Conversation(
                "Thanks kid.",
                "That's alright Mister.",
                true)},
        };
    }
}
