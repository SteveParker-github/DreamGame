public class NPCStateFactory
{
    private NPCController context;
    public NPCStateFactory(NPCController currentContext)
    {
        context = currentContext;
    }

    public NPCBaseState NPCRoamingState()
    {
        return new NPCRoamingState(context, this);
    }

    public NPCBaseState NPCPrepareTalkState()
    {
        return new NPCPrepareTalkState(context, this);
    }

    public NPCBaseState NPCTalkingState()
    {
        return new NPCTalkingState(context, this);
    }
}
