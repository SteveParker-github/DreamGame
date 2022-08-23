public class PlayerStateFactory
{
    private PlayerController context;
    public PlayerStateFactory(PlayerController currentContext)
    {
        context = currentContext;
    }

    public PlayerBaseState PlayerWalkingState()
    {
        return new PlayerWalkingState(context, this);
    }

    public PlayerBaseState PlayerPrepareTalkState()
    {
        return new PlayerPrepareTalkState(context, this);
    }

    public PlayerBaseState PlayerTalkingState()
    {
        return new PlayerTalkingState(context, this);
    }

    public PlayerBaseState PlayerListeningState()
    {
        return new PlayerListeningState(context, this);
    }

    public PlayerBaseState PlayerMenuState()
    {
        return new PlayerMenuState(context, this);
    }

    public PlayerBaseState PlayerMainMenuState()
    {
        return new PlayerMainMenuState(context, this);
    }
}
