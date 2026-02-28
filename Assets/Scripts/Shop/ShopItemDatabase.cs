
using UnityEngine;

public class ShopItemDatabase : MonoBehaviour
{
    public ShopItemData[] shopItems;

    public ShopItemData GetItem(string itemId)
    {
        foreach (var item in shopItems)
        {
            if (item.itemId == itemId)
            {
                return item;
            }
        }
        return null;
    }
}
