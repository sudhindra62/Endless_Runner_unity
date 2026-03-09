
using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Manages the state and timing of a boss chase event.
/// Created by Supreme Guardian Architect v12 to provide a centralized and robust boss encounter system.
/// </summary>
public class BossChaseManager : Singleton<BossChaseManager>
{
    // --- EVENTS ---
    public static event Action<float> OnBossChaseStart; // Parameter: chase duration
    public static event Action OnBossChaseEnd;
    public static event Action<string> OnRewardAwarded; // Parameter: reward message

    // --- PUBLIC PROPERTIES ---
    public float RemainingChaseTime { get; private set; }
    public bool IsChaseActive { get; private set; }

    // --- PRIVATE STATE ---
    private Coroutine _chaseCoroutine;

    #region Public API

    /// <summary>
    /// Starts the boss chase sequence.
    /// </summary>
    /// <param name="chaseDuration">The duration of the chase in seconds.</param>
    public void StartChase(float chaseDuration)
    {
        if (IsChaseActive)
        {
            Debug.LogWarning("Guardian Architect Warning: A boss chase is already active. Cannot start a new one.");
            return;
        }

        IsChaseActive = true;
        RemainingChaseTime = chaseDuration;
        
        Debug.Log($"<color=red>Guardian Architect: Boss Chase STARTED! Duration: {chaseDuration} seconds.</color>");

        // Fire the event for UI and other systems to react to
        OnBossChaseStart?.Invoke(chaseDuration);

        // Start the timer coroutine
        _chaseCoroutine = StartCoroutine(ChaseTimer(chaseDuration));
    }

    /// <summary>
    /// Ends the current boss chase, either by success or failure.
    /// </summary>
    public void EndChase(bool successfully)
    {
        if (!IsChaseActive) return;

        if (_chaseCoroutine != null)
        {
            StopCoroutine(_chaseCoroutine);
            _chaseCoroutine = null;
        }

        IsChaseActive = false;
        RemainingChaseTime = 0f;
        
        Debug.Log("<color=green>Guardian Architect: Boss Chase ENDED.</color>");

        // Fire the event for UI and other systems
        OnBossChaseEnd?.Invoke();

        if (successfully)
        {
            // Award the player
            AwardReward("Boss Defeated! Reward: 1000 Gems"); // Example reward
        }
    }

    #endregion

    #region Private Logic

    /// <summary>
    /// Coroutine that counts down the chase timer.
    /// </summary>
    private IEnumerator ChaseTimer(float duration)
    {
        float timer = duration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            RemainingChaseTime = timer;
            yield return null;
        }

        // If the coroutine completes, the player failed to defeat the boss in time
        EndChase(false);
    }

    /// <summary>
    /// Triggers the reward event.
    /// </summary>
    private void AwardReward(string message)
    { 
        OnRewardAwarded?.Invoke(message);
        // In a real implementation, this would call an inventory or currency manager.
        Debug.Log($"Guardian Architect: {message}");
    }

    #endregion
}
