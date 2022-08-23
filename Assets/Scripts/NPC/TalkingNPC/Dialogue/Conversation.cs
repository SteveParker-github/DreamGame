using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation
{
    protected string option;
    protected string respond;
    protected bool isEnd;
    protected bool isRepeat;
    public Conversation(string option, string respond, bool isEnd)
    {
        this.option = option;
        this.respond = respond;
        this.isEnd = isEnd;
        isRepeat = false;
    }

    public string GetOption()
    {
        return option;
    }

    public virtual string GetRespond()
    {
        isRepeat = true;
        return respond;
    }

    public bool GetIsEnd()
    {
        return isEnd;
    }

    public bool GetIsRepeat()
    {
        return isRepeat;
    }

    public void ToggleRepeat()
    {
        isRepeat = true;
    }

    public virtual bool IsAvailable()
    {
        return true;
    }
}
