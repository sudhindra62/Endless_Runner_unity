using UnityEngine;
using System;
using System.Collections.Generic;

// Enum definitions for clarity. Assumed to be globally accessible or defined in a separate file.
public enum PowerUpType { Magnet, Shield, CoinDoubler, ScoreMultiplier, SpeedBoost, FlowCombo }
public enum FusionType { None, CoinStorm, InvincibleDash, FeverMode }

/// <summary>
/// Data structure for passing fusion information through events.
/// </summary>
public class FusionModifierData
{
    public FusionType Type { get; }
    public float Duration { get; }

    public FusionModifierData(FusionType type, float duration)
    {
        Type = type;
        Duration = duration;
    }
}

/// <summary>
/// Manages the logic for fusing active power-ups into more powerful, temporary modes.
/// Acts as an event-driven extension layer to PowerUpManager.
/// </summary>
public class PowerUpFusionManager : MonoBehaviour
{
    public static PowerUpFusionManager Instance { get; private set; }

    public event Action<FusionModifierData> OnFusionActivated;
    public event Action OnFusionDeactivated;

    private readonly HashSet<PowerUpType> activePowerUps = new HashSet<PowerUpType>();
    private FusionType currentFusion = FusionType.None;
    private float fusionTimer;
    private const float FUSION_BASE_DURATION = 10f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (currentFusion != FusionType.None)
        {
            fusionTimer -= Time.deltaTime;
            if (fusionTimer <= 0)
            {
                DeactivateFusion();
            }
        }
    }

    public void RegisterPowerUp(PowerUpType type)
    {
        if (activePowerUps.Contains(type) || currentFusion != FusionType.None) return;

        activePowerUps.Add(type);
        CheckForFusion();
    }

    public void UnregisterPowerUp(PowerUpType type)
    {
        activePowerUps.Remove(type);
    }

    private void CheckForFusion()
    {
        // Check for Coin Storm (Magnet + Coin Doubler)
        if (activePowerUps.Contains(PowerUpType.Magnet) && activePowerUps.Contains(PowerUpType.CoinDoubler))
        {
            ActivateFusion(FusionType.CoinStorm, new List<PowerUpType> { PowerUpType.Magnet, PowerUpType.CoinDoubler });
            return;
        }

        // Check for Invincible Dash (Shield + Speed Boost)
        if (activePowerUps.Contains(PowerUpType.Shield) && activePowerUps.Contains(PowerUpType.SpeedBoost))
        {
            ActivateFusion(FusionType.InvincibleDash, new List<PowerUpType> { PowerUpType.Shield, PowerUpType.SpeedBoost });
            return;
        }

        // Check for Fever Mode (Multiplier + High Flow Combo)
        // NOTE: Requires external state from FlowComboManager.
        if (activePowerUps.Contains(PowerUpType.ScoreMultiplier) && FlowComboManager.Instance.GetCurrentMultiplier() >= 3f)
        {
            ActivateFusion(FusionType.FeverMode, new List<PowerUpType> { PowerUpType.ScoreMultiplier });
            return;
        }
    }

    private void ActivateFusion(FusionType fusionType, List<PowerUpType> consumedPowerUps)
    {
        currentFusion = fusionType;
        fusionTimer = FUSION_BASE_DURATION;

        Debug.Log($"FUSION ACTIVATED: {fusionType}");

        // Notify other systems of the new fusion state.
        var fusionData = new FusionModifierData(fusionType, fusionTimer);
        OnFusionActivated?.Invoke(fusionData);

        // Consume the base power-ups used in the fusion.
        foreach (var powerUp in consumedPowerUps)
        {
            UnregisterPowerUp(powerUp);
            // PowerUpManager should listen to the OnFusionActivated event to end the base power-ups.
        }
    }

    private void DeactivateFusion()
    {
        Debug.Log($"FUSION DEACTIVATED: {currentFusion}");
        currentFusion = FusionType.None;
        fusionTimer = 0;
        OnFusionDeactivated?.Invoke();
    }
}
