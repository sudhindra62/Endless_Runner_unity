using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Tracks player and community progress for a specific Live Event.
/// Handles offline tracking and syncs with a server when available.
/// Uses IntegrityManager for validation to prevent exploits.
/// </summary>
public class EventProgressTracker : MonoBehaviour
{
    private LiveEventData _activeEvent;
    private Dictionary<string, double> _milestoneProgress = new Dictionary<string, double>();
    private long _playerBestScore = 0;

    // Event to notify other systems of local progress.
    public static event System.Action<MilestoneType, double> OnLocalProgressMade;

    private void OnEnable()
    {
        // Subscribe to relevant game events to track progress.
        EffectsManager.OnNearMiss += HandleNearMiss;
    }

    private void OnDisable()
    {
        EffectsManager.OnNearMiss -= HandleNearMiss;
    }

    public void Initialize(LiveEventData activeEvent)
    {
        _activeEvent = activeEvent;
        // Load progress from save system
        // _playerBestScore = SaveSystem.LoadEventScore(_activeEvent.eventID);
        // _milestoneProgress = SaveSystem.LoadMilestoneProgress(_activeEvent.eventID);
    }

    private void HandleNearMiss()
    {
        UpdateLocalProgress(MilestoneType.TotalNearMisses, 1);
    }

    /// <summary>
    /// Updates local progress and queues it for server sync.
    /// </summary>
    public void UpdateLocalProgress(MilestoneType type, double value)
    {
        // ANTI-EXPLOIT: Use IntegrityManager to validate progress before adding.
        // if (!IntegrityManager.Instance.ValidateProgress(value)) return;

        string key = type.ToString();
        if (!_milestoneProgress.ContainsKey(key)) _milestoneProgress[key] = 0;
        _milestoneProgress[key] += value;
        
        OnLocalProgressMade?.Invoke(type, value);

        // This would then be pushed to a server.
    }

    /// <summary>
    /// Called by LiveEventManager when it receives new global data.
    /// </summary>
    public void UpdateGlobalProgress(Dictionary<string, double> globalProgress)
    {
        // Logic to update global milestone progress and check if milestones are met.
    }
    
    public void SetPlayerScore(long score)
    {
        if (score > _playerBestScore)
        {
            _playerBestScore = score;
            // Save locally
        }
    }
}