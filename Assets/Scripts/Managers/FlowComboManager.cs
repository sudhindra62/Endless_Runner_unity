
using UnityEngine;
using System;

public class FlowComboManager : Singleton<FlowComboManager>
{
    // Events
    public static event Action<int> OnComboChanged;
    public static event Action<float> OnComboMultiplierChanged;
    public static event Action OnComboBroken; // EVOLUTION: Added for MomentumManager

    // Properties
    public int Combo { get; private set; }
    public float ComboMultiplier { get; private set; } = 1f;

    // Configuration
    [Header("Multiplier Settings")]
    [SerializeField] private float multiplierIncrement = 0.1f;

    private void Start()
    {
        OnComboChanged += HandleComboChanged;
        OnComboChanged?.Invoke(Combo);
        OnComboMultiplierChanged?.Invoke(ComboMultiplier);
    }

    private void OnDestroy()
    {
        OnComboChanged -= HandleComboChanged;
    }

    // EVOLUTION: Overloaded method to allow for specific combo additions (e.g., from near-misses).
    /// <summary>
    /// Adds a specified amount to the current combo.
    /// </summary>
    public void AddToCombo(int amount)
    {
        if (amount <= 0) return;
        Combo += amount;
        OnComboChanged?.Invoke(Combo);
    }
    
    /// <summary>
    /// Increments the combo by one. Original logic preserved.
    /// </summary>
    public void AddToCombo()
    {
        AddToCombo(1);
    }

    /// <summary>
    /// Resets the combo to zero and invokes the combo broken event.
    /// </summary>
    public void BreakCombo()
    {
        if (Combo == 0) return;
        Combo = 0;
        OnComboChanged?.Invoke(Combo);
        OnComboBroken?.Invoke(); // EVOLUTION: Notify listeners that the combo has broken.
    }

    /// <summary>
    /// Recalculates and updates the combo multiplier.
    /// </summary>
    private void HandleComboChanged(int newCombo)
    {
        ComboMultiplier = 1f + (newCombo * multiplierIncrement);
        OnComboMultiplierChanged?.Invoke(ComboMultiplier);
    }
}
