
using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class MultiplierTier
{
    public float styleThreshold;
    public int multiplier;
}

/// <summary>
/// Manages the score multiplier.
/// </summary>
public class ScoreMultiplierManager : MonoBehaviour
{
    public static event Action<float> OnMultiplierChanged;

    public static ScoreMultiplierManager Instance { get; private set; }

    [Header("Multiplier Settings")]
    [Tooltip("The amount of style points gained for a perfect dodge.")]
    public float perfectDodgeBonus = 10f;
    [Tooltip("The rate at which the style meter decays over time.")]
    public float styleDecayRate = 2f;
    [Tooltip("The maximum amount of style points.")]
    public float maxStyle = 100f;

    [Header("Multiplier Tiers")]
    [Tooltip("The list of multiplier tiers. Make sure to order them by styleThreshold in ascending order.")]
    public List<MultiplierTier> multiplierTiers = new List<MultiplierTier>();

    public float CurrentStyle { get; private set; }
    public int ScoreMultiplier { get; private set; } = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ReviveManager.OnReviveSuccess += ResetMultiplier;
        ResetMultiplier();
    }

    private void OnDestroy()
    {
        ReviveManager.OnReviveSuccess -= ResetMultiplier;
    }

    private void Update()
    {
        // Decay the style meter over time
        if (CurrentStyle > 0)
        {
            CurrentStyle -= styleDecayRate * Time.deltaTime;
            CurrentStyle = Mathf.Max(0, CurrentStyle);
        }

        UpdateScoreMultiplier();
    }

    public void PerfectDodge()
    {
        CurrentStyle += perfectDodgeBonus;
        CurrentStyle = Mathf.Min(CurrentStyle, maxStyle);
    }

    public void ResetMultiplier()
    {
        CurrentStyle = 0;
        UpdateScoreMultiplier();
    }

    private void UpdateScoreMultiplier()
    {
        int oldMultiplier = ScoreMultiplier;

        ScoreMultiplier = 1;
        for (int i = multiplierTiers.Count - 1; i >= 0; i--)
        {
            if (CurrentStyle >= multiplierTiers[i].styleThreshold)
            {
                ScoreMultiplier = multiplierTiers[i].multiplier;
                break;
            }
        }

        if (oldMultiplier != ScoreMultiplier)
        {
            OnMultiplierChanged?.Invoke(ScoreMultiplier);
        }
    }

    public float GetCurrentMultiplier()
    {
        return ScoreMultiplier;
    }
}
