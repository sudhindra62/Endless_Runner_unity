
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FeverModeManager : MonoBehaviour
{
    // Events
    public static event Action<float> OnFeverGaugeChanged;
    public static event Action<float> OnFeverStart;
    public static event Action OnFeverEnd;

    [Header("Fever Configuration")]
    [SerializeField] private float feverDuration = 10f;
    [SerializeField] private float maxFeverGauge = 100f;
    [SerializeField] private float feverSpeedBoost = 5f;
    [SerializeField] private int feverMultiplier = 20;

    // State
    private float currentFeverGauge;
    private bool isFeverActive;
    private Coroutine feverCoroutine;

    // Modifiers
    private readonly Dictionary<string, float> chargeMultipliers = new Dictionary<string, float>();

    private void Awake()
    {
        // Assumes a ServiceLocator is in the scene
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        GameStateManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateManager.OnGameStateChanged -= OnGameStateChanged;
    }

    public void AddFeverPoints(float amount)
    {
        if (isFeverActive || amount <= 0) return;

        float finalAmount = amount * CalculateChargeMultiplier();
        currentFeverGauge = Mathf.Min(currentFeverGauge + finalAmount, maxFeverGauge);
        OnFeverGaugeChanged?.Invoke(currentFeverGauge / maxFeverGauge);

        if (currentFeverGauge >= maxFeverGauge)
        {
            ActivateFever();
        }
    }

    private void ActivateFever()
    {
        if (isFeverActive) return;

        isFeverActive = true;
        currentFeverGauge = 0;
        OnFeverGaugeChanged?.Invoke(0f);
        feverCoroutine = StartCoroutine(FeverRoutine());
    }

    private IEnumerator FeverRoutine()
    {
        OnFeverStart?.Invoke(feverDuration);
        yield return new WaitForSeconds(feverDuration);
        OnFeverEnd?.Invoke();
        isFeverActive = false;
        feverCoroutine = null;
    }

    private void OnGameStateChanged(GameState newState)
    {
        // Reset on new run start or returning to menu
        if (newState == GameState.Playing || newState == GameState.Menu || newState == GameState.EndOfRun)
        {
            ResetFeverState();
        }
    }

    private void ResetFeverState()
    {
        if (feverCoroutine != null)
        {
            StopCoroutine(feverCoroutine);
            feverCoroutine = null;
        }
        
        if (isFeverActive)
        {
            OnFeverEnd?.Invoke();
            isFeverActive = false;
        }
        
        currentFeverGauge = 0;
        OnFeverGaugeChanged?.Invoke(0f);
    }

    public bool IsFeverActive() => isFeverActive;
    public float GetFeverSpeedBoost() => isFeverActive ? feverSpeedBoost : 0f;
    public int GetFeverMultiplier() => feverMultiplier;

    // --- Modifier Integration --- //

    public void ApplyChargeMultiplier(string sourceId, float multiplier)
    {
        chargeMultipliers[sourceId] = Mathf.Max(0.01f, multiplier); // Prevent non-positive multipliers
    }

    public void RemoveChargeMultiplier(string sourceId)
    {
        chargeMultipliers.Remove(sourceId);
    }

    private float CalculateChargeMultiplier()
    {
        if (chargeMultipliers.Count == 0)
        {
            return 1f;
        }

        float logSum = 0.0f;
        foreach (var multiplier in chargeMultipliers.Values)
        {
            logSum += Mathf.Log(multiplier);
        }
        return Mathf.Exp(logSum);
    }
    
    public void ResetState()
    {
        chargeMultipliers.Clear();
        ResetFeverState();
    }
}
