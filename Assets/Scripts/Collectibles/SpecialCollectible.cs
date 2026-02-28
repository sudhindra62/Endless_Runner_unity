
using UnityEngine;

/// <summary>
/// Represents a special collectible item that triggers an immediate in-game effect.
/// In this case, it converts all active obstacles into coins.
/// </summary>
public class SpecialCollectible : Collectible
{
    private EffectsManager effectsManager;

    protected override void Start()
    {
        base.Start();
        // Resolve dependencies using the ServiceLocator
        effectsManager = ServiceLocator.Get<EffectsManager>();
        poolTag = "SpecialCollectible";
    }

    protected override void OnCollect()
    {
        if (effectsManager != null)
        {
            effectsManager.ConvertAllObstaclesToCoins();
        }
        else
        {
            Debug.LogWarning("EffectsManager not found. Cannot convert obstacles.");
        }
    }
}
