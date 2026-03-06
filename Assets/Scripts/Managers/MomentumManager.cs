using UnityEngine;
using System;

/// <summary>
/// Tracks uninterrupted player flow to build and apply momentum.
/// This system acts as a 'flow mastery layer', rewarding skilled play with stacking modifiers.
/// </summary>
public class MomentumManager : MonoBehaviour
{
    public static MomentumManager Instance { get; private set; }

    // --- Events ---
    public event Action<MomentumData> OnMomentumChanged;
    public event Action OnMomentumLost;

    [Header("Tier Configuration")]
    [Tooltip("Time of uninterrupted flow required to reach each tier.")]
    [SerializeField] private float[] timeToReachTier = { 5f, 15f, 30f, 60f };

    [Tooltip("Speed modifier applied at each tier.")]
    [SerializeField] private float[] speedModifiers = { 1.05f, 1.1f, 1.15f, 1.2f };

    [Tooltip("Score multiplier applied at each tier.")]
    [SerializeField] private float[] scoreMultipliers = { 1.5f, 2f, 2.5f, 3f };

    // --- State ---
    public int CurrentTier { get; private set; } = 0;
    private float flowTimer = 0f;
    private bool isPaused = false; // Used to temporarily halt momentum gain (e.g., during Fever Mode).

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
        // --- Subscribe to Game Events ---
        // PlayerCollisionHandler.OnObstacleHit += ResetMomentum;
        // FlowComboManager.OnComboBroken += ResetMomentum;
        // FeverModeManager.OnFeverModeStarted += (data) => { isPaused = true; };
        // FeverModeManager.OnFeverModeEnded += () => { isPaused = false; };
    }

    private void Update()
    {
        if (isPaused) return;

        flowTimer += Time.deltaTime;

        int nextTier = 0;
        for (int i = 0; i < timeToReachTier.Length; i++)
        {
            if (flowTimer >= timeToReachTier[i])
            {
                nextTier = i + 1;
            }
        }

        if (nextTier != CurrentTier)
        {
            SetTier(nextTier);
        }
    }

    private void SetTier(int newTier)
    {
        if (newTier > timeToReachTier.Length) newTier = timeToReachTier.Length;
        CurrentTier = newTier;

        float speedMod = (CurrentTier > 0) ? speedModifiers[CurrentTier - 1] : 1f;
        float scoreMod = (CurrentTier > 0) ? scoreMultipliers[CurrentTier - 1] : 1f;

        var data = new MomentumData(CurrentTier, speedMod, scoreMod);
        OnMomentumChanged?.Invoke(data);
        Debug.Log($"[MomentumManager] Tier changed to {CurrentTier}");
    }

    /// <summary>
    /// Resets all momentum progress. Called on hits or combo breaks.
    /// </summary>
    public void ResetMomentum()
    {
        if (CurrentTier == 0 && flowTimer < timeToReachTier[0]) return; // No momentum to lose

        Debug.Log("[MomentumManager] Momentum lost!");
        flowTimer = 0f;
        if (CurrentTier != 0)
        {
            SetTier(0);
            OnMomentumLost?.Invoke();
        }
    }
    
    /// <summary>
    /// A hard reset for end-of-run or revive scenarios.
    /// </summary>
    public void ResetManager()
    {
        isPaused = false;
        ResetMomentum();
    }
}
