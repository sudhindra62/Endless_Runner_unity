
using UnityEngine;

/// <summary>
/// Engine for determining if a player receives a rare drop based on various factors.
/// Created by OMNI_LOGIC_COMPLETION_v2.
/// </summary>
public class RareDropEngine : Singleton<RareDropEngine>
{
    [Tooltip("Base chance for a rare drop, from 0.0 to 1.0")]
    [SerializeField] private float baseDropChance = 0.05f;

    // You could add complexity with pity timers, luck stats, etc.

    public void TryGrantRareDrop(Vector3 dropPosition)
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= baseDropChance)
        {
            GrantRareDrop(dropPosition);
        }
    }

    private void GrantRareDrop(Vector3 position)
    {
        Debug.Log("A rare drop was granted!");
        
        // This is where you'd get the specific item data from a drop table
        // For now, let's assume a generic "RareCollectible"
        string rareItemId = "RARE_ITEM_01";
        
        if (CollectionManager.Instance != null)
        {
            CollectionManager.Instance.AddItemToCollection(rareItemId);
        }

        // You could also instantiate a special effect or object at the drop position
        // VFXManager.Instance.PlayEffect("RareDropEffect", position);
    }
}
