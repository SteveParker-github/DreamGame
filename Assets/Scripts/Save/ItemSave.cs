[System.Serializable]
public class ItemSave
{
    public string itemName;
    public string itemDescription;

    public ItemSave(string itemName, string itemDescription)
    {
        this.itemName = itemName;
        this.itemDescription = itemDescription;
    }
}
