[System.Serializable]
public class ItemData
{
    public string itemName;

    public ItemData(Item item)
    {
        itemName = item.name;
    }
}
