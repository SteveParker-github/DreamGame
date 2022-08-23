using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspiciousConversation : Conversation
{
    private PlayerController playerController;
    public SuspiciousConversation(string option, string respond, bool isEnd, PlayerController playerController)
    : base(option, respond, isEnd)
    {
        this.playerController = playerController;
    }

    public override string GetRespond()
    {
        if (isRepeat) return respond;

        isRepeat = true;
        playerController.DialogueFails++;
        return respond;
    }
}
