using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PowerUpFusionManager : MonoBehaviour
{
    public static event Action<FusionType, float> OnFusionStart;
    public static event Action<FusionType> OnFusionEnd;

    [Header("Fusion Configuration")]
    [SerializeField] private float coinStormDuration = 15f;
    [SerializeField] private float invincibleDashDuration = 10f;
    [SerializeField] private float ultraComboDuration = 12f;

    private readonly HashSet<PowerUpType> activePowerUps = new HashSet<PowerUpType>();
    private FusionType activeFusion = FusionType.None;
    private Coroutine activeFusionCoroutine;

    // Dependencies
    private PowerUpManager powerUpManager;
    private UIManager uiManager;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        powerUpManager = ServiceLocator.Get<PowerUpManager>();
        if (powerUpManager != null)
        {
            powerUpManager.OnPowerUpActivated += OnPowerUpActivated;
            powerUpManager.OnPowerUpDeactivated += OnPowerUpDeactivated;
        }

        uiManager = ServiceLocator.Get<UIManager>();

        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        if (powerUpManager != null)
        {
            powerUpManager.OnPowerUpActivated -= OnPowerUpActivated;
            powerUpManager.OnPowerUpDeactivated -= OnPowerUpDeactivated;
        }

        GameManager.OnGameStateChanged -= OnGameStateChanged;
        ServiceLocator.Unregister<PowerUpFusionManager>();
    }

    private void OnPowerUpActivated(PowerUpType newPowerUp)
    {
        activePowerUps.Add(newPowerUp);
        CheckForFusions();
    }

    private void OnPowerUpDeactivated(PowerUpType expiredPowerUp)
    {
        activePowerUps.Remove(expiredPowerUp);
        // No need to check for fusions here, as deactivation shouldn't trigger a new fusion.
    }

    private void CheckForFusions()
    {
        if (activeFusion != FusionType.None) return; // Only one fusion at a time

        // Check for Coin Storm (Magnet + Coin Doubler)
        if (activePowerUps.Contains(PowerUpType.Magnet) && activePowerUps.Contains(PowerUpType.CoinDoubler))
        {
            ActivateFusion(FusionType.CoinStorm, coinStormDuration);
        }
        // Check for Invincible Dash (Shield + Speed Boost)
        else if (activePowerUps.Contains(PowerUpType.Shield) && activePowerUps.Contains(PowerUpType.SpeedBoost))
        {
            ActivateFusion(FusionType.InvincibleDash, invincibleDashDuration);
        }
        // Check for Ultra Combo (Score Multiplier + Fever Mode)
        else if (activePowerUps.Contains(PowerUpType.ScoreMultiplier) && activePowerUps.Contains(PowerUpType.FeverMode))
        {
            ActivateFusion(FusionType.UltraCombo, ultraComboDuration);
        }
    }

    private void ActivateFusion(FusionType fusionType, float duration)
    {
        if (activeFusion != FusionType.None) return;

        activeFusion = fusionType;

        // Pause the timers of the base power-ups that created the fusion
        PauseTimersForFusion(fusionType, true);

        // Start the fusion coroutine
        activeFusionCoroutine = StartCoroutine(FusionRoutine(fusionType, duration));
    }

    private IEnumerator FusionRoutine(FusionType fusionType, float duration)
    {
        OnFusionStart?.Invoke(fusionType, duration);
        if (uiManager != null) uiManager.ShowFusionUI(fusionType, duration);
        
        yield return new WaitForSeconds(duration);

        EndFusion(fusionType);
    }

    private void EndFusion(FusionType fusionType)
    {
        if (activeFusion != fusionType) return;
        
        OnFusionEnd?.Invoke(fusionType);
        if (uiManager != null) uiManager.HideFusionUI();
        activeFusion = FusionType.None;
        activeFusionCoroutine = null;

        // Resume the timers of the base power-ups
        PauseTimersForFusion(fusionType, false);

        // Re-check for any other possible fusions, though unlikely
        CheckForFusions();
    }

    private void PauseTimersForFusion(FusionType fusionType, bool pause)
    {
        if (powerUpManager == null) return;

        switch (fusionType)
        {
            case FusionType.CoinStorm:
                powerUpManager.PausePowerUpTimer(PowerUpType.Magnet, pause);
                powerUpManager.PausePowerUpTimer(PowerUpType.CoinDoubler, pause);
                break;
            case FusionType.InvincibleDash:
                powerUpManager.PausePowerUpTimer(PowerUpType.Shield, pause);
                powerUpManager.PausePowerUpTimer(PowerUpType.SpeedBoost, pause);
                break;
            case FusionType.UltraCombo:
                powerUpManager.PausePowerUpTimer(PowerUpType.ScoreMultiplier, pause);
                powerUpManager.PausePowerUpTimer(PowerUpType.FeverMode, pause);
                break;
        }
    }
    
    private void OnGameStateChanged(GameState newState)
    {
        // Reset on new run or returning to menu
        if (newState == GameState.Menu || newState == GameState.EndOfRun)
        {
            if (activeFusionCoroutine != null)
            {
                StopCoroutine(activeFusionCoroutine);
                activeFusionCoroutine = null;
            }
            if (activeFusion != FusionType.None)
            {
                OnFusionEnd?.Invoke(activeFusion);
                 if (uiManager != null) uiManager.HideFusionUI();
            }
            activeFusion = FusionType.None;
            activePowerUps.Clear();
        }
    }

    public FusionType GetActiveFusion() => activeFusion;
}
