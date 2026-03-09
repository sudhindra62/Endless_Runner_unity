
using UnityEngine;

/// <summary>
/// A specific implementation of ShardableItemProfile for unlocking cosmetic effects.
/// Authored by the Supreme Guardian Architect v12.
/// </summary>
[CreateAssetMenu(fileName = "EffectShardableItem", menuName = "Shards/Implementations/Effect Item")]
public class EffectShardableItem : ShardableItemProfile
{
    [Header("Effect Specific")]
    [Tooltip("The ID of the effect to unlock, as recognized by the CosmeticEffectManager.")]
    public string effectId;

    /// <summary>
    /// Implements the unlock logic for a cosmetic effect.
    /// </summary>
    public override void OnUnlock(object playerData)
    {
        // In a real implementation, you would get the CosmeticEffectManager instance and call it.
        Debug.Log($"<color=purple>Guardian Architect: Effect '{effectId}' unlocked via ShardableItemProfile. Forwarding to CosmeticEffectManager.</color>");
        // CosmeticEffectManager.Instance.UnlockEffect(effectId);
    }
}
