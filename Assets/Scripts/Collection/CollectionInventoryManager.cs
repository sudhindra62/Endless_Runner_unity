
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
        string key = itemData.itemName;
        if (!fragmentCounts.ContainsKey(key))
        {
            fragmentCounts[key] = 0;
        }
        fragmentCounts[key] += amount;
        SaveFragmentCount(key, fragmentCounts[key]);
        OnInventoryChanged?.Invoke(itemData);
    }

    public int GetFragmentCount(CollectionItemData itemData)
    {
        string key = itemData.itemName;
        if (!fragmentCounts.ContainsKey(key))
        {
            fragmentCounts[key] = LoadFragmentCount(key);
        }
        return fragmentCounts[key];
    }

    private void SaveFragmentCount(string key, int count)
    {
        PlayerPrefs.SetInt(FRAGMENT_COUNT_KEY_PREFIX + key, count);
        PlayerPrefs.Save();
    }

    private int LoadFragmentCount(string key)
    {
        return PlayerPrefs.GetInt(FRAGMENT_COUNT_KEY_PREFIX + key, 0);
    }
}
