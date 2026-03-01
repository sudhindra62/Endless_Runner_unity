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
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
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
        isFeverActive = false;
        currentFeverGauge = 0;
        OnFeverGaugeChanged?.Invoke(0f);
        if(OnFeverEnd != null) OnFeverEnd();
    }

    public bool IsFeverActive() => isFeverActive;
    public float GetFeverSpeedBoost() => isFeverActive ? feverSpeedBoost : 0f;
    public int GetFeverMultiplier() => feverMultiplier;

    // --- Modifier Integration --- //

    public void ApplyChargeMultiplier(string sourceId, float multiplier)
    {
        chargeMultipliers[sourceId] = multiplier;
    }

    public void RemoveChargeMultiplier(string sourceId)
    {
        chargeMultipliers.Remove(sourceId);
    }

    private float CalculateChargeMultiplier()
    {
        float total = 1f;
        foreach (var multiplier in chargeMultipliers.Values)
        {
            total *= multiplier;
        }
        return total;
    }
    
    public void ResetState()
    {
        chargeMultipliers.Clear();
        ResetFeverState();
    }
}
