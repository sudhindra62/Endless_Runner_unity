
using UnityEngine;
using System;

/// <summary>
/// Manages the player's "flow" combo, which builds with skillful play
/// and provides a score multiplier bonus.
/// This system is event-driven and does not directly modify other systems.
/// </summary>
public class FlowComboManager : Singleton<FlowComboManager>
{
    // --- Events ---
    public static event Action<int> OnComboChanged; // Sends the current combo count
    public static event Action<float> OnComboMultiplierChanged; // Sends the new multiplier
    public static event Action OnComboBroken;
    public static event Action<float> OnTierIncreased; // Sends the new multiplier when a tier is crossed

    // --- State ---
    public int ComboCount { get; private set; }
    public float CurrentMultiplier { get; private set; }

    // --- Configuration ---
    [Header("Settings")]
    [SerializeField] private float comboDecayTime = 3.0f;

    // --- Private Fields ---
    private float lastComboEventTime;
    private float previousMultiplier;

    private void Start()
    {
        Initialize();
        // Subscribe to game events that should break the combo.
        // These subscriptions are examples and should be implemented in the target classes.
        // PlayerCollisionHandler.OnPlayerHit += BreakCombo;
        // GameFlowController.OnRunEnd += BreakCombo;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks.
        // PlayerCollisionHandler.OnPlayerHit -= BreakCombo;
        // GameFlowController.OnRunEnd -= BreakCombo;
    }

    private void Update()
    {
        // If the combo is active, check if the decay timer has expired.
        if (ComboCount > 0 && Time.unscaledTime > lastComboEventTime + comboDecayTime)
        {
            Debug.Log("Combo timed out.");
            BreakCombo();
        }
    }

    private void Initialize()
    {
        ComboCount = 0;
        CurrentMultiplier = 1f;
        previousMultiplier = 1f;
        lastComboEventTime = 0f; // Use 0 to indicate no active combo
    }

    /// <summary>
    /// Adds a specified amount to the current combo count.
    /// Called by external systems (e.g., PerfectDodgeDetector).
    /// </summary>
    public void AddToCombo(int amount = 1)
    {
        if (amount <= 0) return;

        ComboCount += amount;
        lastComboEventTime = Time.unscaledTime; // Reset decay timer

        OnComboChanged?.Invoke(ComboCount);
        UpdateMultiplier(true);
    }

    /// <summary>
    /// Resets the combo count and multiplier to their initial states.
    /// Called by external systems (e.g., when the player gets hit).
    /// </summary>
    public void BreakCombo()
    {
        if (ComboCount == 0) return; // Already broken

        Initialize();

        // Notify listeners that the combo has been broken
        OnComboBroken?.Invoke();
        OnComboChanged?.Invoke(ComboCount);
        UpdateMultiplier(false);
    }

    /// <summary>
    /// Calculates the multiplier based on the current combo count and notifies listeners if it changes.
    /// </summary>
    private void UpdateMultiplier(bool justIncreased)
    {
        if (ComboCount >= 50) CurrentMultiplier = 4f;
        else if (ComboCount >= 31) CurrentMultiplier = 3f;
        else if (ComboCount >= 16) CurrentMultiplier = 2f;
        else if (ComboCount >= 6) CurrentMultiplier = 1.5f;
        else CurrentMultiplier = 1f;

        // Check if the multiplier value has changed since the last update
        if (Math.Abs(CurrentMultiplier - previousMultiplier) > 0.01f)
        {
            OnComboMultiplierChanged?.Invoke(CurrentMultiplier);
            
            // If the combo was just increased and crossed a tier, fire the tier-up event
            if (justIncreased && CurrentMultiplier > previousMultiplier)
            {
                OnTierIncreased?.Invoke(CurrentMultiplier);
            }

            previousMultiplier = CurrentMultiplier;
        }
    }
}
