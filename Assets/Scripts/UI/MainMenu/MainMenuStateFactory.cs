public class MainMenuStateFactory
{
    private MainMenuController context;
    public MainMenuStateFactory(MainMenuController currentContext)
    {
        context = currentContext;
    }

    public MainMenuBaseState MainPanelState()
    {
        return new MainPanelState(context, this);
    }

    public MainMenuBaseState LoadGamePanelState()
    {
        return new LoadGamePanelState(context, this);
    }

    public MainMenuBaseState SaveGamePanelState()
    {
        return new SaveGamePanelState(context, this);
    }
}
