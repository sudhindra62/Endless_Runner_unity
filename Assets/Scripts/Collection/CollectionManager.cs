
using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Manages the player's inventory of collected items (e.g., for a collection album).
/// Created by OMNI_LOGIC_COMPLETION_v2.
/// </summary>
public class CollectionManager : Singleton<CollectionManager>
{
    public static event Action<string> OnItemAddedToCollection; // string: itemID

    // This would be loaded from player save data
    private HashSet<string> collectedItemIDs = new HashSet<string>();

    public void AddItemToCollection(string itemID)
    {
        if (collectedItemIDs.Contains(itemID))
        {
            Debug.Log($"Player already has item {itemID} in their collection.");
            return;
        }

        collectedItemIDs.Add(itemID);
        OnItemAddedToCollection?.Invoke(itemID);
        Debug.Log($"Item {itemID} added to collection!");

        // Save the updated collection data
        // SaveManager.Instance.SavePlayerData();
    }

    public bool HasItemInCollection(string itemID)
    {
        return collectedItemIDs.Contains(itemID);
    }

    public HashSet<string> GetCollection()
    {
        return new HashSet<string>(collectedItemIDs);
    }

    public List<CollectionItemData> allCollectionItems
    {
        get
        {
            CollectionItemData[] database = Resources.LoadAll<CollectionItemData>(string.Empty);
            List<CollectionItemData> resolvedItems = new List<CollectionItemData>();

            foreach (CollectionItemData item in database)
            {
                if (item != null && collectedItemIDs.Contains(item.itemName))
                {
                    resolvedItems.Add(item);
                }
            }

            return resolvedItems;
        }
    }
}
