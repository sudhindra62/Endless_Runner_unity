
using UnityEngine;
using System;

public class MomentumManager : MonoBehaviour
{
    #region EVENTS
    public static event Action<int> OnMomentumTierChanged;
    public static event Action<float> OnSpeedModifierChanged;
    public static event Action<float> OnScoreMultiplierChanged;
    #endregion

    #region CONFIGURATION
    [Header("Momentum Tiers")]
    [SerializeField] private float[] timeThresholds = { 5f, 15f, 30f };
    [SerializeField] private float[] speedModifiers = { 1.1f, 1.25f, 1.5f };
    [SerializeField] private float[] scoreMultipliers = { 1.5f, 2.0f, 3.0f };

    [Header("Hard Caps")]
    [SerializeField] private float maxSpeedBonus = 8.0f;
    #endregion

    #region STATE
    private int currentTier = -1;
    private float uninterruptedFlowTime = 0f;
    private bool isMomentumActive = false;
    private float lastEmittedSpeedModifier = 1f;
    private float lastEmittedScoreMultiplier = 1f;
    #endregion

    #region UNITY_LIFECYCLE
    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        // Subscribe to relevant events
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        PlayerController.OnPlayerHit += HandlePlayerHit;
        FlowComboManager.OnComboBroken += HandleComboBroken;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        PlayerController.OnPlayerHit -= HandlePlayerHit;
        FlowComboManager.OnComboBroken -= HandleComboBroken;
        ServiceLocator.Unregister<MomentumManager>();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState == GameState.Playing && isMomentumActive)
        {
            uninterruptedFlowTime += Time.deltaTime;
            UpdateMomentumTier();
        }
    }
    #endregion

    #region EVENT_HANDLERS
    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.Playing)
        {
            isMomentumActive = true;
        }
        else
        {
            ResetMomentum();
        }
    }

    private void HandlePlayerHit()
    {
        ResetMomentum();
    }

    private void HandleComboBroken()
    {
        ResetMomentum();
    }
    #endregion

    #region INTERNAL_LOGIC
    private void UpdateMomentumTier()
    {
        int newTier = -1;
        for (int i = timeThresholds.Length - 1; i >= 0; i--)
        {
            if (uninterruptedFlowTime >= timeThresholds[i])
            {
                newTier = i;
                break;
            }
        }

        if (newTier != currentTier)
        {
            SetTier(newTier);
        }
    }

    private void SetTier(int tier)
    {
        currentTier = tier;
        OnMomentumTierChanged?.Invoke(currentTier);

        // Emit new modifiers, ensuring not to stack with Fever Mode incorrectly
        float feverSpeedMultiplier = FeverModeManager.Instance?.IsFeverActive() ?? false ? FeverModeManager.Instance.GetFeverSpeedMultiplier() : 1f;
        float finalSpeedModifier = currentTier >= 0 ? Mathf.Min(speedModifiers[currentTier] * feverSpeedMultiplier, maxSpeedBonus) : 1f;
        
        if (Math.Abs(finalSpeedModifier - lastEmittedSpeedModifier) > 0.01f)
        {
            OnSpeedModifierChanged?.Invoke(finalSpeedModifier);
            lastEmittedSpeedModifier = finalSpeedModifier;
        }
        
        float finalScoreMultiplier = currentTier >= 0 ? scoreMultipliers[currentTier] : 1f;
        if (Math.Abs(finalScoreMultiplier - lastEmittedScoreMultiplier) > 0.01f)
        {
            OnScoreMultiplierChanged?.Invoke(finalScoreMultiplier);
            lastEmittedScoreMultiplier = finalScoreMultiplier;
        }
    }

    public void ResetMomentum()
    {
        uninterruptedFlowTime = 0f;
        isMomentumActive = false; // Stop tracking time
        if (currentTier != -1)
        {
            SetTier(-1); // Reset to base values
        }
    }
    #endregion

    #region PUBLIC_API (FOR OTHER SYSTEMS)
    public float GetCurrentSpeedModifier() => lastEmittedSpeedModifier;
    public float GetCurrentScoreMultiplier() => lastEmittedScoreMultiplier;
    public int GetCurrentMomentumTier() => currentTier;
    #endregion
}
