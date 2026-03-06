using UnityEngine;
using System;

public class FlowComboManager : MonoBehaviour
{
    public static FlowComboManager Instance { get; private set; }

    public event Action<int> OnComboChanged;
    public event Action<float> OnComboMultiplierChanged;

    private int currentCombo;
    private float comboMultiplier = 1f;
    private float comboDecayTimer;

    private const float COMBO_DECAY_TIME = 3f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Subscribe to relevant game events here
        // Example: PerfectDodgeDetector.OnPerfectDodge += IncreaseCombo;
        // Example: PlayerCollisionHandler.OnPlayerHit += ResetCombo;
    }

    private void Update()
    {
        if (currentCombo > 0)
        {
            comboDecayTimer -= Time.unscaledDeltaTime;
            if (comboDecayTimer <= 0)
            {
                ResetCombo();
            }
        }
    }

    public void IncreaseCombo()
    {
        currentCombo++;
        comboDecayTimer = COMBO_DECAY_TIME;
        UpdateMultiplier();
        OnComboChanged?.Invoke(currentCombo);
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        comboDecayTimer = 0;
        UpdateMultiplier();
        OnComboChanged?.Invoke(currentCombo);
    }

    private void UpdateMultiplier()
    {
        float newMultiplier = 1f;
        if (currentCombo >= 50) newMultiplier = 4f;
        else if (currentCombo >= 31) newMultiplier = 3f;
        else if (currentCombo >= 16) newMultiplier = 2f;
        else if (currentCombo >= 6) newMultiplier = 1.5f;

        if (Math.Abs(newMultiplier - comboMultiplier) > 0.01f)
        {
            comboMultiplier = newMultiplier;
            OnComboMultiplierChanged?.Invoke(comboMultiplier);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
    }
}
