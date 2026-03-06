
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CollectionSet
{
    public string setName;
    public List<CollectionItemData> itemsInSet;
    public int bonusGems;
    public string bonusTitle;
    public bool isSetClaimed;
}

public class CollectionManager : Singleton<CollectionManager>
{
    public List<CollectionItemData> allCollectionItems;
    public List<CollectionSet> collectionSets;

    private const string SET_CLAIMED_KEY_PREFIX = "SetClaimed_";

    private void Start()
    {
        LoadSetData();
        CollectionInventoryManager.OnInventoryChanged += CheckForCompletion;
    }

    private void OnDestroy()
    {
        CollectionInventoryManager.OnInventoryChanged -= CheckForCompletion;
    }

    private void CheckForCompletion(CollectionItemData itemData)
    {
        if (CollectionInventoryManager.Instance.GetFragmentCount(itemData) >= itemData.requiredFragments)
        {
            Debug.Log(itemData.itemName + " collection complete!");
            // Automatically unlock the item
            UnlockItem(itemData);
        }
    }

    private void UnlockItem(CollectionItemData itemData)
    {
        // Here you would integrate with your SkinManager, CosmeticEffectManager, etc.
        // For example:
        // if (itemData.itemType == CollectionItemType.SkinFragment)
        // {
        //     SkinManager.Instance.UnlockSkin(itemData.rewardPrefab);
        // }
        Debug.Log("Unlocked: " + itemData.itemName);
    }

    public void CheckForSetCompletion()
    {
        foreach (var set in collectionSets)
        {
            if (set.isSetClaimed) continue;

            bool allItemsCollected = true;
            foreach (var item in set.itemsInSet)
            {
                // This assumes an unlock status is stored somewhere, e.g., in the respective manager
                // For this example, we'll just check if the fragments are sufficient
                if (CollectionInventoryManager.Instance.GetFragmentCount(item) < item.requiredFragments)
                {
                    allItemsCollected = false;
                    break;
                }
            }

            if (allItemsCollected)
            {
                ClaimSetBonus(set);
            }
        }
    }

    private void ClaimSetBonus(CollectionSet set)
    {
        // Grant bonus rewards
        // CurrencyManager.Instance.AddGems(set.bonusGems);
        // PlayerProgression.Instance.UnlockTitle(set.bonusTitle);
        Debug.Log("Set bonus claimed for: " + set.setName);

        set.isSetClaimed = true;
        SaveSetData(set);
    }

    private void SaveSetData(CollectionSet set)
    {
        PlayerPrefs.SetInt(SET_CLAIMED_KEY_PREFIX + set.setName, set.isSetClaimed ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSetData()
    {
        foreach (var set in collectionSets)
        {
            set.isSetClaimed = PlayerPrefs.GetInt(SET_CLAIMED_KEY_PREFIX + set.setName, 0) == 1;
        }
    }
}
