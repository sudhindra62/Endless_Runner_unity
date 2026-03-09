
using UnityEngine;

/// <summary>
/// A ScriptableObject that defines an item that can be unlocked or crafted using shards.
/// This asset centralizes unlock requirements and fulfillment logic.
/// Authored by the Supreme Guardian Architect v12.
/// </summary>
[CreateAssetMenu(fileName = "ShardableItem", menuName = "Shards/Shardable Item Profile")]
public abstract class ShardableItemProfile : ScriptableObject
{
    [Header("Shard Information")]
    [Tooltip("The unique identifier for the shards used to craft this item.")]
    public string shardId;

    [Tooltip("The number of shards required to unlock this item.")]
    public int shardsRequired = 100;

    /// <summary>
    /// An abstract method that defines the action to be taken when this item is unlocked.
    /// Each subclass will implement its own specific unlock logic (e.g., unlocking a skin, granting a character).
    /// </summary>
    /// <param name="playerData">The player's data, allowing the unlock logic to modify it.</param>
    public abstract void OnUnlock(object playerData); // Using 'object' for flexibility
}
