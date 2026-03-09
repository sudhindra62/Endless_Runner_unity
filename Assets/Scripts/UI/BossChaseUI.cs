
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Use TextMeshPro
using System.Collections; // Required for IEnumerator

/// <summary>
/// Manages all UI elements related to the boss chase event.
/// Fully refactored and fortified by Supreme Guardian Architect v12.
/// </summary>
[AddComponentMenu("UI/Boss Chase UI Controller")]
public class BossChaseUI : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("A flashing warning indicator shown at the start of the chase.")]
    [SerializeField] private GameObject warningIndicator;
    
    [Tooltip("A slider representing the remaining time in the chase.")]
    [SerializeField] private Slider timerBar;
    
    [Tooltip("An animated image that pulses at the edge of the screen during the chase.")]
    [SerializeField] private Image screenEdgePulse;
    
    [Tooltip("A TextMeshProUGUI element to display reward notifications.")]
    [SerializeField] private TextMeshProUGUI rewardNotification;

    #region Unity Lifecycle & Event Subscription

    private void OnEnable()
    {
        // Subscribe to BossChaseManager events
        BossChaseManager.OnBossChaseStart += HandleBossChaseStart;
        BossChaseManager.OnBossChaseEnd += HandleBossChaseEnd;
        BossChaseManager.OnRewardAwarded += ShowRewardNotification;

        // Initial UI state
        InitializeUI();
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        BossChaseManager.OnBossChaseStart -= HandleBossChaseStart;
        BossChaseManager.OnBossChaseEnd -= HandleBossChaseEnd;
        BossChaseManager.OnRewardAwarded -= ShowRewardNotification;
        
        // Ensure coroutines are stopped when the object is disabled
        StopAllCoroutines();
    }

    private void Update()
    {
        // Update the timer bar if the chase is active
        if (BossChaseManager.Instance != null && BossChaseManager.Instance.IsChaseActive)
        {
            if (timerBar != null)
            {
                // The manager now holds the remaining time, making the UI purely for display
                timerBar.value = BossChaseManager.Instance.RemainingChaseTime;
            }
        }
    }

    #endregion

    #region Event Handlers

    private void HandleBossChaseStart(float chaseDuration)
    {
        if (warningIndicator != null) StartCoroutine(FlashWarningRoutine());
        
        if (timerBar != null) 
        {
            timerBar.gameObject.SetActive(true);
            timerBar.maxValue = chaseDuration;
            timerBar.value = chaseDuration;
        }
        
        if (screenEdgePulse != null) screenEdgePulse.gameObject.SetActive(true); // Animation would be handled by an Animator component

        // Example: Trigger music change via an AudioManager
        // AudioManager.Instance.PlayBossMusic();
    }

    private void HandleBossChaseEnd()
    {
        if (warningIndicator != null) warningIndicator.SetActive(false);
        if (timerBar != null) timerBar.gameObject.SetActive(false);
        if (screenEdgePulse != null) screenEdgePulse.gameObject.SetActive(false);

        // Example: Revert music change
        // AudioManager.Instance.PlayDefaultMusic();
    }

    #endregion

    #region UI Logic

    private void InitializeUI()
    {
        // Ensure all elements are in a known-default state on enable
        if (warningIndicator != null) warningIndicator.SetActive(false);
        if (timerBar != null) timerBar.gameObject.SetActive(false);
        if (screenEdgePulse != null) screenEdgePulse.gameObject.SetActive(false);
        if (rewardNotification != null) rewardNotification.gameObject.SetActive(false);
    }

    /// <summary>
    /// Displays a reward message for a short duration.
    /// </summary>
    public void ShowRewardNotification(string message)
    {
        if (rewardNotification != null)
        {
            rewardNotification.text = message;
            rewardNotification.gameObject.SetActive(true);
            // Use a coroutine for the delay to avoid issues with Invoke stopping on disable
            StartCoroutine(HideRewardNotificationRoutine());
        }
    }

    private IEnumerator HideRewardNotificationRoutine()
    {
        yield return new WaitForSeconds(3f); 
        if (rewardNotification != null) rewardNotification.gameObject.SetActive(false);
    }

    /// <summary>
    /// A coroutine to flash the warning indicator a few times.
    /// </summary>
    private IEnumerator FlashWarningRoutine()
    {
        if (warningIndicator == null) yield break;

        // Flash 3 times
        for(int i = 0; i < 3; i++)
        {
            warningIndicator.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            warningIndicator.SetActive(false);
            yield return new WaitForSeconds(0.4f);
        }
    }

    #endregion
}
