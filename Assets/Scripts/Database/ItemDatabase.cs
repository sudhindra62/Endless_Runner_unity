using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "RPG/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> items;

    public Item GetItem(string itemName)
    {
        return items.Find(item => item.name == itemName);
    }
}
