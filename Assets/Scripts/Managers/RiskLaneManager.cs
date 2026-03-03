
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Manages a high-risk, high-reward lane. Its parameters are now dynamically loaded
/// from the RemoteConfig system, adhering to project standards.
/// </summary>
public class RiskLaneManager : MonoBehaviour
{
    #region EVENTS
    public static event Action<int, bool, float, float> OnRiskLaneChanged;
    #endregion

    #region CONFIGURATION
    // --- EVOLUTION: Configuration is no longer serialized, it's loaded from RemoteConfig ---
    private float riskDuration;
    private float cooldownDuration;
    private float riskCoinMultiplier;
    private float riskObstacleMultiplier;
    private int totalLanes;
    #endregion

    #region STATE
    private Coroutine riskCycleCoroutine;
    private int currentRiskLaneIndex = -99;
    private bool isGloballyIncompatibleEventActive = false;
    #endregion

    #region UNITY_LIFECYCLE
    private void Awake()
    {
        ServiceLocator.Register(this);
        // --- EVOLUTION: Load configuration on awake ---
        LoadConfiguration();
    }

    private void Start()
    {
        EnvironmentEventManager.OnWorldEvent += HandleWorldEvent;
        GameStateManager.OnGameStateChanged += HandleGameStateChanged;
        // --- EVOLUTION: Subscribe to config updates ---
        RemoteConfig.OnConfigUpdated += HandleConfigUpdated;

        if (GameStateManager.Instance.CurrentState == GameState.Playing)
        {
            StartRiskCycle();
        }
    }

    private void OnDestroy()
    {
        EnvironmentEventManager.OnWorldEvent -= HandleWorldEvent;
        GameStateManager.OnGameStateChanged -= HandleGameStateChanged;
        // --- EVOLUTION: Unsubscribe from config updates ---
        RemoteConfig.OnConfigUpdated -= HandleConfigUpdated;
        ServiceLocator.Unregister<RiskLaneManager>();
    }
    #endregion

    #region CONFIGURATION_HANDLING
    /// <summary>
    /// NEW: Populates the configuration from the RemoteConfig manager.
    /// </summary>
    private void LoadConfiguration()
    {
        riskDuration = RemoteConfig.GetFloat("RiskLaneDuration", 15f);
        cooldownDuration = RemoteConfig.GetFloat("RiskLaneCooldown", 20f);
        riskCoinMultiplier = RemoteConfig.GetFloat("RiskLaneCoinMultiplier", 2.0f);
        riskObstacleMultiplier = RemoteConfig.GetFloat("RiskLaneObstacleMultiplier", 1.5f);
        totalLanes = RemoteConfig.GetInt("TotalLanes", 3);
        Debug.Log("RiskLaneManager configuration loaded from RemoteConfig.");
    }

    /// <summary>
    /// NEW: Handles live updates to the remote configuration.
    /// </summary>
    private void HandleConfigUpdated()
    {
        Debug.Log("Remote config updated. Reloading RiskLaneManager settings.");
        LoadConfiguration();
    }
    #endregion
    
    #region EVENT_HANDLERS
    // --- PRESERVED: All event handlers are unchanged ---
    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.Playing)
        {
            StartRiskCycle();
        }
        else
        {
            StopRiskCycle();
        }
    }

    private void HandleWorldEvent(WorldEventData eventData)
    {
        if (eventData.IsLaneBased == false)
        {
            isGloballyIncompatibleEventActive = eventData.IsActive;
            if (isGloballyIncompatibleEventActive && currentRiskLaneIndex != -99)
            {
                RevertCurrentRiskLane();
            }
        }
    }
    #endregion

    #region CORE_LOGIC
    // --- PRESERVED: All core logic is unchanged ---
    private void StartRiskCycle()
    {
        if (riskCycleCoroutine == null)
        {
            riskCycleCoroutine = StartCoroutine(RiskLaneCycle());
        }
    }

    private void StopRiskCycle()
    {
        if (riskCycleCoroutine != null)
        {
            StopCoroutine(riskCycleCoroutine);
            riskCycleCoroutine = null;
            RevertCurrentRiskLane();
        }
    }

    private IEnumerator RiskLaneCycle()
    {
        yield return new WaitForSeconds(cooldownDuration);
        while (true)
        {
            yield return new WaitUntil(() => !isGloballyIncompatibleEventActive);
            int newLaneIndex = SelectNewRiskLane();
            ActivateRiskLane(newLaneIndex);
            yield return new WaitForSeconds(riskDuration);
            RevertCurrentRiskLane();
            yield return new WaitForSeconds(cooldownDuration);
        }
    }

    private int SelectNewRiskLane()
    {
        int newIndex;
        int laneZeroBased = totalLanes - 1;
        int midLaneOffset = laneZeroBased / 2;
        do
        {
            newIndex = UnityEngine.Random.Range(0, totalLanes) - midLaneOffset;
        } 
        while (newIndex == currentRiskLaneIndex);
        return newIndex;
    }

    private void ActivateRiskLane(int laneIndex)
    {
        if (laneIndex == currentRiskLaneIndex) return;
        currentRiskLaneIndex = laneIndex;
        OnRiskLaneChanged?.Invoke(currentRiskLaneIndex, true, riskCoinMultiplier, riskObstacleMultiplier);
    }

    private void RevertCurrentRiskLane()
    {
        if (currentRiskLaneIndex != -99)
        {
            int laneToRevert = currentRiskLaneIndex;
            currentRiskLaneIndex = -99;
            OnRiskLaneChanged?.Invoke(laneToRevert, false, 1f, 1f);
        }
    }
    #endregion
}
