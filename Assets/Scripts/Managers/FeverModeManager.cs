using UnityEngine;
using System;

/// <summary>
/// Manages the activation, duration, and effects of Fever Mode.
/// It does not directly modify player or game stats, but rather broadcasts
/// events with a FeverModeData payload that other managers can subscribe to.
/// </summary>
public class FeverModeManager : MonoBehaviour
{
    public static FeverModeManager Instance { get; private set; }

    // --- Events ---
    public event Action<FeverModeData> OnFeverModeStarted;
    public event Action OnFeverModeEnded;

    [Header("Fever Configuration")]
    [SerializeField] private float feverDuration = 10f;
    [SerializeField] private float feverCooldown = 20f;
    [SerializeField] private float scoreMultiplier = 3f;
    [SerializeField] private float speedBoost = 1.2f; // 20% speed increase

    // --- State ---
    public bool IsFeverActive { get; private set; }
    private float cooldownTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Attempts to activate Fever Mode. This is the primary public entry point.
    /// </summary>
    public void TryActivateFeverMode()
    {
        if (IsFeverActive || cooldownTimer > 0)
        {
            Debug.Log("[FeverModeManager] Cannot activate: Already active or on cooldown.");
            return;
        }

        StartFeverMode();
    }

    private void StartFeverMode()
    {
        Debug.Log("[FeverModeManager] Fever Mode ACTIVATED!");
        IsFeverActive = true;

        var feverData = new FeverModeData(scoreMultiplier, speedBoost, isInvincible: true);
        OnFeverModeStarted?.Invoke(feverData);

        Invoke(nameof(EndFeverMode), feverDuration);
    }

    private void EndFeverMode()
    {
        Debug.Log("[FeverModeManager] Fever Mode ENDED.");
        IsFeverActive = false;
        cooldownTimer = feverCooldown;

        OnFeverModeEnded?.Invoke();
    }

    /// <summary>
    /// Immediately stops any active fever mode and resets cooldown.
    /// Essential for clean state changes on player death or run end.
    /// </summary>
    public void ResetManager()
    {
        if (IsFeverActive)
        {
            // Cancel the scheduled EndFeverMode invoke to prevent it from firing after a reset.
            CancelInvoke(nameof(EndFeverMode));
            EndFeverMode();
        }
        cooldownTimer = 0f;
    }
}
