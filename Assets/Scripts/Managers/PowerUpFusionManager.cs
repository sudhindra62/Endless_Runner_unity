
using UnityEngine;
using System;
using System.Collections.Generic;

public enum FusionType
{
    None,
    CoinStorm,
    InvincibleDash,
    FeverFrenzy
}

public struct FusionModifierData
{
    public FusionType Type;
    public float Duration;
}

public class PowerUpFusionManager : Singleton<PowerUpFusionManager>
{
    public static event Action<FusionModifierData> OnFusionActivated;
    public static event Action<FusionType> OnFusionDeactivated;

    private readonly Dictionary<FusionType, float> activeFusions = new Dictionary<FusionType, float>();
    private readonly List<FusionType> recentlyTriggeredFusions = new List<FusionType>();

    private PowerUpManager powerUpManager;
    private FlowComboManager flowComboManager;

    private void Start()
    {
        powerUpManager = PowerUpManager.Instance;
        flowComboManager = FlowComboManager.Instance;
        
        PowerUpManager.OnPowerUpActivated += HandlePowerUpActivation;
        PowerUpManager.OnPowerUpDeactivated += HandlePowerUpDeactivation;
    }

    private void OnDestroy()
    {
        if (PowerUpManager.Instance != null) PowerUpManager.OnPowerUpActivated -= HandlePowerUpActivation;
        if (PowerUpManager.Instance != null) PowerUpManager.OnPowerUpDeactivated -= HandlePowerUpDeactivation;
    }

    private void Update()
    {
        if (activeFusions.Count == 0) return;

        var expiredFusions = new List<FusionType>();
        var fusionKeys = new List<FusionType>(activeFusions.Keys);

        foreach (var fusionType in fusionKeys)
        {
            activeFusions[fusionType] -= Time.deltaTime;
            if (activeFusions[fusionType] <= 0)
            {
                expiredFusions.Add(fusionType);
            }
        }

        foreach (var expiredFusion in expiredFusions)
        {
            DeactivateFusion(expiredFusion);
        }
    }

    private void HandlePowerUpActivation(PowerUpType type, float duration)
    {
        CheckForFusion(type, duration);
    }
    
    private void HandlePowerUpDeactivation(PowerUpType type)
    {
        ClearRecentFusionOnComponentDeactivation(type);
    }

    private void CheckForFusion(PowerUpType newPowerUp, float duration)
    {
        if (powerUpManager == null) return;

        // Coin Storm: Magnet + CoinDoubler
        if ((newPowerUp == PowerUpType.Magnet && powerUpManager.IsPowerUpActive(PowerUpType.CoinDoubler)) ||
            (newPowerUp == PowerUpType.CoinDoubler && powerUpManager.IsPowerUpActive(PowerUpType.Magnet)))
        {
            ActivateFusion(FusionType.CoinStorm, duration);
        }

        // Invincible Dash: Shield + Speed Boost
        if ((newPowerUp == PowerUpType.Shield && powerUpManager.IsPowerUpActive(PowerUpType.SpeedBoost)) ||
            (newPowerUp == PowerUpType.SpeedBoost && powerUpManager.IsPowerUpActive(PowerUpType.Shield)))
        {
            ActivateFusion(FusionType.InvincibleDash, duration);
        }

        // Fever Frenzy: Multiplier + FlowCombo Tier 3+
        if (newPowerUp == PowerUpType.ScoreMultiplier && flowComboManager != null && flowComboManager.GetCurrentTier() >= 3)
        {
            ActivateFusion(FusionType.FeverFrenzy, duration);
        }
    }

    private void ActivateFusion(FusionType fusionType, float duration)
    {
        if (recentlyTriggeredFusions.Contains(fusionType)) return;

        activeFusions[fusionType] = duration;
        recentlyTriggeredFusions.Add(fusionType);
        
        OnFusionActivated?.Invoke(new FusionModifierData { Type = fusionType, Duration = duration });
    }

    private void DeactivateFusion(FusionType fusionType)
    {
        if (!activeFusions.ContainsKey(fusionType)) return;

        activeFusions.Remove(fusionType);
        OnFusionDeactivated?.Invoke(fusionType);
    }
    
    private void ClearRecentFusionOnComponentDeactivation(PowerUpType componentType)
    {
        switch (componentType)
        {
            case PowerUpType.Magnet:
            case PowerUpType.CoinDoubler:
                recentlyTriggeredFusions.Remove(FusionType.CoinStorm);
                break;
            case PowerUpType.Shield:
            case PowerUpType.SpeedBoost:
                recentlyTriggeredFusions.Remove(FusionType.InvincibleDash);
                break;
            case PowerUpType.ScoreMultiplier:
                recentlyTriggeredFusions.Remove(FusionType.FeverFrenzy);
                break;
        }
    }
}
