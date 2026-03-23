
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Manages a high-risk, high-reward lane. Its parameters are now dynamically loaded
/// from the RemoteConfig system, adhering to project standards.
/// --- EVOLUTION: Now compatible with DecisionPathManager ---
/// </summary>
public class RiskLaneManager : MonoBehaviour
{
    #region EVENTS
    public static event Action<int, bool, float, float> OnRiskLaneChanged;
    #endregion

    #region CONFIGURATION
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
    private bool isDecisionPathActive = false; // --- NEW: Flag to pause logic during decision paths ---
    #endregion

    #region UNITY_LIFECYCLE
    private void Awake()
    {
        ServiceLocator.Register(this);
        LoadConfiguration();
    }

    private void Start()
    {
        // --- PRESERVED: Original event subscriptions ---
        EnvironmentEventManager.OnWorldEvent += HandleWorldEvent;
        if (GameStateManager.Instance != null) GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        RemoteConfig.OnConfigUpdated += HandleConfigUpdated;

        // --- INTEGRATION: Subscribe to DecisionPathManager events for compatibility ---
        DecisionPathManager.OnPathSplit += HandlePathSplit;
        DecisionPathManager.OnPathMerge += HandlePathMerge;

        if (GameStateManager.Instance.CurrentState == GameStateManager.GameState.Playing)
        {
            StartRiskCycle();
        }
    }

    private void OnDestroy()
    {
        // --- PRESERVED: Original event unsubscriptions ---
        EnvironmentEventManager.OnWorldEvent -= HandleWorldEvent;
        if (GameStateManager.Instance != null) GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        RemoteConfig.OnConfigUpdated -= HandleConfigUpdated;
        
        // --- INTEGRATION: Unsubscribe from DecisionPathManager events ---
        DecisionPathManager.OnPathSplit -= HandlePathSplit;
        DecisionPathManager.OnPathMerge -= HandlePathMerge;

        ServiceLocator.Unregister<RiskLaneManager>();
    }
    #endregion

    #region CONFIGURATION_HANDLING
    // --- PRESERVED: All original configuration logic is unchanged ---
    private void LoadConfiguration()
    {
        riskDuration = RemoteConfig.GetFloat("RiskLaneDuration", 15f);
        cooldownDuration = RemoteConfig.GetFloat("RiskLaneCooldown", 20f);
        riskCoinMultiplier = RemoteConfig.GetFloat("RiskLaneCoinMultiplier", 2.0f);
        riskObstacleMultiplier = RemoteConfig.GetFloat("RiskLaneObstacleMultiplier", 1.5f);
        totalLanes = RemoteConfig.GetInt("TotalLanes", 3);
    }

    private void HandleConfigUpdated()
    {
        LoadConfiguration();
    }
    #endregion
    
    #region EVENT_HANDLERS
    // --- INTEGRATION: Event handlers to pause/resume logic for DecisionPathManager compatibility ---
    private void HandlePathSplit(int minLane, int maxLane)
    {
        isDecisionPathActive = true;
        StopRiskCycle(); // Pause risk lane logic immediately
    }

    private void HandlePathMerge()
    {
        isDecisionPathActive = false;
        StartRiskCycle(); // Resume risk lane logic
    }

    // --- PRESERVED & ADAPTED: Now checks for the new compatibility flag ---
    private void HandleGameStateChanged(GameStateManager.GameState newState)
    {
        if (newState == GameStateManager.GameState.Playing && !isDecisionPathActive)
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
        if (!eventData.IsLaneBased)
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
    // --- PRESERVED & ADAPTED: Core logic now respects the isDecisionPathActive flag ---
    private void StartRiskCycle()
    {
        // Only start if not already running AND Decision Path is not active to prevent conflicts
        if (riskCycleCoroutine == null && !isDecisionPathActive)
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
            // --- ADAPTED: Wait until no incompatible events are active (including Decision Path) ---
            yield return new WaitUntil(() => !isGloballyIncompatibleEventActive && !isDecisionPathActive);
            
            int newLaneIndex = SelectNewRiskLane();
            ActivateRiskLane(newLaneIndex);
            
            yield return new WaitForSeconds(riskDuration);
            
            RevertCurrentRiskLane();
            
            yield return new WaitForSeconds(cooldownDuration);
        }
    }

    private int SelectNewRiskLane()
    {
        // --- PRESERVED: Original lane selection logic is unchanged ---
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
        // --- PRESERVED: Original activation logic is unchanged ---
        if (laneIndex == currentRiskLaneIndex) return;
        currentRiskLaneIndex = laneIndex;
        OnRiskLaneChanged?.Invoke(currentRiskLaneIndex, true, riskCoinMultiplier, riskObstacleMultiplier);
    }

    private void RevertCurrentRiskLane()
    {
        // --- PRESERVED: Original revert logic is unchanged ---
        if (currentRiskLaneIndex != -99)
        {
            int laneToRevert = currentRiskLaneIndex;
            currentRiskLaneIndex = -99;
            OnRiskLaneChanged?.Invoke(laneToRevert, false, 1f, 1f);
        }
    }
    #endregion
}
