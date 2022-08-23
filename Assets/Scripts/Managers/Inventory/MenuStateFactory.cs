public class MenuStateFactory
{
    private InventoryManager context;
    public MenuStateFactory(InventoryManager currentContext)
    {
        context = currentContext;
    }

    public MenuBaseState MenuInventoryGridState()
    {
        return new MenuInventoryGridState(context, this);
    }

    public MenuBaseState MenuItemDetailState()
    {
        return new MenuItemDetailState(context, this);
    }

    public MenuBaseState MenuIdleState()
    {
        return new MenuIdleState(context, this);
    }

    public MenuBaseState MenuQuestState()
    {
        return new MenuQuestState(context, this);
    }
}
