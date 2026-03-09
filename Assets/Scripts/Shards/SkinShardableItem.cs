
using UnityEngine;

/// <summary>
/// A specific implementation of ShardableItemProfile for unlocking character skins.
/// Authored by the Supreme Guardian Architect v12.
/// </summary>
[CreateAssetMenu(fileName = "SkinShardableItem", menuName = "Shards/Implementations/Skin Item")]
public class SkinShardableItem : ShardableItemProfile
{
    [Header("Skin Specific")]
    [Tooltip("The ID of the skin to unlock, as recognized by the SkinManager.")]
    public string skinId;

    /// <summary>
    /// Implements the unlock logic for a skin.
    /// This calls the appropriate manager to unlock the skin.
    /// </summary>
    public override void OnUnlock(object playerData)
    {
        // In a real implementation, you would get the SkinManager instance and call it.
        // For this example, we'll just log a message.
        Debug.Log($"<color=purple>Guardian Architect: Skin '{skinId}' unlocked via ShardableItemProfile. Forwarding to SkinManager.</color>");
        // SkinManager.Instance.UnlockSkin(skinId);
    }
}
