
using UnityEngine;

/// <summary>
/// Central hub for all data integrity and anti-cheat validation.
/// Orchestrates validation across different modules (Session, Economy, Save).
/// Designed as a Singleton for global access from authoritative managers.
/// </summary>
public class IntegrityManager : Singleton<IntegrityManager>
{
    public SessionValidator sessionValidator { get; private set; }
    public EconomyValidator economyValidator { get; private set; }
    public SaveIntegrityGuard saveIntegrityGuard { get; private set; }

    [Header("Configuration")]
    [Tooltip("Maximum number of revives allowed in a single run.")]
    public int maxRevivesPerRun = 1; // Default, should be fetched from Remote Config

    protected override void Awake()
    {
        base.Awake();
        // Initialize all validation modules
        sessionValidator = new SessionValidator();
        economyValidator = new EconomyValidator();
        saveIntegrityGuard = new SaveIntegrityGuard();
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Reports a detected integrity error.
    /// In a production environment, this would send a detailed report to a backend service.
    /// </summary>
    /// <param name="errorMessage">The specific error message to log.</param>
    public void ReportError(string errorMessage)
    {
        Debug.LogWarning($"[IntegrityManager] Validation Error: {errorMessage}");
        // Here you would hook into a service to send this event to your analytics backend.
        // For example: AnalyticsService.Instance.TrackSecurityEvent(errorMessage);
    }

    /// <summary>
    /// Displays a generic message to the player when data is restored.
    /// </summary>
    public void NotifyPlayerOfDataRestoration()
    {
        Debug.Log("[IntegrityManager] Notifying player of data restoration.");
        // This will call the UIManager to show a non-intrusive popup.
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowSystemMessage("Game data restored to prevent corruption.");
        }
    }
}
