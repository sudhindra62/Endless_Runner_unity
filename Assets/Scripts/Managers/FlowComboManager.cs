using UnityEngine;
using System;

public class FlowComboManager : MonoBehaviour
{
    public static FlowComboManager Instance { get; private set; }

    public static event Action<int> OnComboChanged;
    public static event Action<float> OnComboMultiplierChanged;
    public static event Action<int> OnComboBroken;
    public static event Action<int, int> OnTierIncreased;

    private int currentCombo;
    private float comboDecayTimer;
    private float comboMultiplier = 1f;
    private int currentTier;
    
    public float timeout => comboDecayTimer;
    public int ComboCount => currentCombo;
    public float ComboMultiplier => comboMultiplier;

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
    
    public void AddCombo() => IncreaseCombo();
    public void AddCombo(int amount) { for (int i = 0; i < amount; i++) IncreaseCombo(); }

    public void ResetCombo()
    {
        if (currentCombo > 0) OnComboBroken?.Invoke(currentCombo);
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
            int previousTier = currentTier;
            comboMultiplier = newMultiplier;
            currentTier = GetTierForMultiplier(comboMultiplier);
            OnComboMultiplierChanged?.Invoke(comboMultiplier);
            if (currentTier > previousTier)
            {
                OnTierIncreased?.Invoke(previousTier, currentTier);
            }
        }
    }

    public void SetTimeout(float duration) { comboDecayTimer = duration; }
    public float GetTimeout() => comboDecayTimer;

    private static int GetTierForMultiplier(float multiplier)
    {
        if (multiplier >= 4f) return 4;
        if (multiplier >= 3f) return 3;
        if (multiplier >= 2f) return 2;
        if (multiplier >= 1.5f) return 1;
        return 0;
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
    }
}
