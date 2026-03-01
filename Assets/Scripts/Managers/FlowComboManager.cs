
using UnityEngine;
using System;
using System.Collections;

public class FlowComboManager : MonoBehaviour
{
    // Events
    public static event Action<int> OnComboChanged;
    public static event Action<int> OnMultiplierChanged;
    public static event Action<float> OnMomentumChanged;
    public static event Action OnComboBroken;

    // Configuration
    [Header("Combo Settings")]
    [SerializeField] private int comboPointsPerMultiplierTier = 5;
    [SerializeField] private int maxMultiplier = 10;
    [SerializeField] private float comboTimeout = 4f;

    [Header("Momentum Settings")]
    [SerializeField] private float speedBoostPerTier = 0.5f;
    [SerializeField] private float maxSpeedBoost = 3f;

    // State
    private int currentCombo;
    private int currentMultiplier;
    private float currentMomentumBonus;
    private Coroutine comboTimeoutCoroutine;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
        // Subscribe to events that build combo
        // e.g., PerfectDodgeDetector.OnPerfectDodge += () => AddCombo(1);
        // e.g., PlayerCollision.OnHitObstacle += BreakCombo;
        ResetCombo();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    public void AddCombo(int points)
    {
        if (points <= 0) return;

        currentCombo += points;
        OnComboChanged?.Invoke(currentCombo);

        UpdateMultiplier();
        UpdateMomentum();

        if (comboTimeoutCoroutine != null)
        {
            StopCoroutine(comboTimeoutCoroutine);
        }
        comboTimeoutCoroutine = StartCoroutine(ComboTimeoutRoutine());
    }

    public void BreakCombo()
    {
        if (currentCombo == 0) return;

        currentCombo = 0;
        currentMultiplier = 1;
        currentMomentumBonus = 0f;

        OnComboChanged?.Invoke(currentCombo);
        OnMultiplierChanged?.Invoke(currentMultiplier);
        OnMomentumChanged?.Invoke(currentMomentumBonus);
        OnComboBroken?.Invoke();

        if (comboTimeoutCoroutine != null)
        {
            StopCoroutine(comboTimeoutCoroutine);
            comboTimeoutCoroutine = null;
        }
    }

    private void UpdateMultiplier()
    {
        int newMultiplier = 1 + (currentCombo / comboPointsPerMultiplierTier);
        newMultiplier = Mathf.Min(newMultiplier, maxMultiplier);

        if (newMultiplier != currentMultiplier)
        {
            currentMultiplier = newMultiplier;
            OnMultiplierChanged?.Invoke(currentMultiplier);
        }
    }

    private void UpdateMomentum()
    {
        int comboTier = currentCombo / comboPointsPerMultiplierTier;
        float newMomentumBonus = Mathf.Min(comboTier * speedBoostPerTier, maxSpeedBoost);

        if (Math.Abs(newMomentumBonus - currentMomentumBonus) > 0.01f)
        {
            currentMomentumBonus = newMomentumBonus;
            OnMomentumChanged?.Invoke(currentMomentumBonus);
        }
    }

    private IEnumerator ComboTimeoutRoutine()
    {
        yield return new WaitForSeconds(comboTimeout);
        BreakCombo();
    }

    private void ResetCombo()
    {
        currentCombo = 0;
        currentMultiplier = 1;
        currentMomentumBonus = 0f;

        OnComboChanged?.Invoke(currentCombo);
        OnMultiplierChanged?.Invoke(currentMultiplier);
        OnMomentumChanged?.Invoke(currentMomentumBonus);

        if (comboTimeoutCoroutine != null)
        {
            StopCoroutine(comboTimeoutCoroutine);
            comboTimeoutCoroutine = null;
        }
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Playing)
        {
            ResetCombo();
        }
        else if (newState == GameState.EndOfRun || newState == GameState.Menu || newState == GameState.Paused)
        {
            BreakCombo();
        }
    }
}
