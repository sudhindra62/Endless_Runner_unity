using UnityEngine;
using System;

public class FlowComboManager : Singleton<FlowComboManager>
{
    // Events to notify other systems of changes
    public static event Action<int> OnComboChanged;
    public static event Action<float> OnComboMultiplierChanged;

    // Public properties for read-only access
    public int Combo { get; private set; }
    public float ComboMultiplier { get; private set; } = 1f;

    // Configuration
    [Header("Multiplier Settings")]
    [Tooltip("The amount the multiplier increases for each combo point.")]
    [SerializeField] private float multiplierIncrement = 0.1f;

    private void Start()
    {
        // Subscribe to its own event to manage the multiplier internally
        OnComboChanged += HandleComboChanged;
        
        // Ensure initial state is broadcast
        OnComboChanged?.Invoke(Combo);
        OnComboMultiplierChanged?.Invoke(ComboMultiplier);
    }

    private void OnDestroy()
    {
        OnComboChanged -= HandleComboChanged;
    }

    /// <summary>
    /// Increments the combo by one.
    /// </summary>
    public void AddToCombo()
    {
        Combo++;
        OnComboChanged?.Invoke(Combo);
    }

    /// <summary>
    /// Resets the combo to zero.
    /// </summary>
    public void BreakCombo()
    {
        if (Combo == 0) return; // No need to break if already at 0
        Combo = 0;
        OnComboChanged?.Invoke(Combo);
    }

    /// <summary>
    /// Recalculates and updates the combo multiplier whenever the combo count changes.
    /// </summary>
    private void HandleComboChanged(int newCombo)
    {
        ComboMultiplier = 1f + (newCombo * multiplierIncrement);
        OnComboMultiplierChanged?.Invoke(ComboMultiplier);
    }
}
