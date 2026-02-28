
using System;
using UnityEngine;

/// <summary>
/// A safe, decoupled bridge for gameplay scripts to report player status changes to the UI.
/// Gameplay scripts can call the static methods on this notifier without any knowledge of the UI layer.
/// UI scripts can subscribe to its events to react to changes.
/// </summary>
public class PlayerStatusNotifier : Singleton<PlayerStatusNotifier>
{
    // Event for shield status changes
    public static event Action<bool> OnShieldStatusChanged;

    // Event for pausing/resuming the game
    public static event Action<bool> OnPauseStateChanged;

    /// <summary>
    /// Called by gameplay scripts (e.g., PlayerController, PowerUpManager) when the shield is activated or deactivated.
    /// </summary>
    public void NotifyShieldStatus(bool isShieldActive)
    {
        OnShieldStatusChanged?.Invoke(isShieldActive);
    }

    /// <summary>
    /// Called by UI scripts (e.g., a PauseButton) to request a pause or resume.
    /// </summary>
    public void NotifyPauseState(bool isPaused)
    {
        OnPauseStateChanged?.Invoke(isPaused);
    }
}
