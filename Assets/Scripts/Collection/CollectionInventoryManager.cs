
using UnityEngine;
using System;
using System.Collections.Generic;

public class CollectionInventoryManager : Singleton<CollectionInventoryManager>
{
    public static event Action<CollectionItemData> OnInventoryChanged;

    private Dictionary<string, int> fragmentCounts = new Dictionary<string, int>();

    private const string FRAGMENT_COUNT_KEY_PREFIX = "FragmentCount_";

    public void AddFragments(CollectionItemData itemData, int amount)
    {
        if (SaveManager.Instance == null) return;
        
        string key = itemData.itemName;
        if (!SaveManager.Instance.Data.fragmentInventory.ContainsKey(key))
        {
            SaveManager.Instance.Data.fragmentInventory[key] = 0;
        }
        SaveManager.Instance.Data.fragmentInventory[key] += amount;
        SaveManager.Instance.SaveGame();
        
        OnInventoryChanged?.Invoke(itemData);
    }

    public int GetFragmentCount(CollectionItemData itemData)
    {
        if (SaveManager.Instance == null) return 0;
        
        string key = itemData.itemName;
        if (SaveManager.Instance.Data.fragmentInventory.TryGetValue(key, out int count))
        {
            return count;
        }
        return 0;
    }

    // Legacy methods removed in favor of direct GameData access
}
