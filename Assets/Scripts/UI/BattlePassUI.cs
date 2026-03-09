
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Manages the UI for the Battle Pass screen, displaying tiers and progress.
/// Logic fully restored and fortified by Supreme Guardian Architect v12.
/// </summary>
[AddComponentMenu("UI/Battle Pass/Battle Pass UI Controller")]
public class BattlePassUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text currentTierText;
    [SerializeField] private BattlePassTierUI tierItemPrefab;
    [SerializeField] private Transform tierContainer;

    // --- PRIVATE STATE ---
    private List<BattlePassTierUI> _tierItems = new List<BattlePassTierUI>();

    #region Unity Lifecycle & Event Subscription

    private void Start()
    {
        // --- ERROR_HANDLING_POLICY: Validate prefab assignment ---
        if (tierItemPrefab == null)
        {
            Debug.LogError("Guardian Architect FATAL_ERROR: tierItemPrefab is not assigned in BattlePassUI. Disabling.", this);
            enabled = false;
            return;
        }

        GenerateTierVisuals();
        SubscribeToEvents();
        RefreshAllUI();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        // Subscribe to the correct, existing events in the manager
        BattlePassManager.OnXPAdded += HandleXPAdded;
        BattlePassManager.OnTierUnlocked += HandleTierUnlocked;
        BattlePassManager.OnRewardClaimed += HandleRewardClaimed;
    }

    private void UnsubscribeFromEvents()
    {
        // Unsubscribe from the correct, existing events in the manager
        BattlePassManager.OnXPAdded -= HandleXPAdded;
        BattlePassManager.OnTierUnlocked -= HandleTierUnlocked;
        BattlePassManager.OnRewardClaimed -= HandleRewardClaimed;
    }

    #endregion

    #region UI Generation and Refresh

    /// <summary>
    /// Creates the UI visuals for all tiers from the manager'''s data.
    /// </summary>
    private void GenerateTierVisuals()
    {
        // Clear any placeholder children
        foreach (Transform child in tierContainer) { Destroy(child.gameObject); }
        _tierItems.Clear();

        int maxTiers = BattlePassManager.Instance.GetMaxTiers();
        for (int i = 0; i < maxTiers; i++)
        {
            BattlePassTier tierData = BattlePassManager.Instance.GetTierData(i);
            if (tierData != null)
            {
                BattlePassTierUI newTierUI = Instantiate(tierItemPrefab, tierContainer);
                newTierUI.Setup(i, tierData);
                _tierItems.Add(newTierUI);
            }
        }
        Debug.Log($"Guardian Architect: Generated {_tierItems.Count} Battle Pass tier UI items.");
    }

    /// <summary>
    /// Refreshes the entire Battle Pass UI, including progress and all tier states.
    /// </summary>
    private void RefreshAllUI()
    {
        if (BattlePassManager.Instance == null) return;
        RefreshXPProgress();
        foreach (var item in _tierItems)
        {
            item.Refresh();
        }
    }

    /// <summary>
    /// Updates the main XP slider and text.
    /// </summary>
    private void RefreshXPProgress()
    {
        int currentXP = BattlePassManager.Instance.GetCurrentXP() % BattlePassManager.Instance.GetXPForNextTier();
        int xpForNext = BattlePassManager.Instance.GetXPForNextTier();
        int currentTier = BattlePassManager.Instance.GetCurrentTier();
        int maxTiers = BattlePassManager.Instance.GetMaxTiers();

        currentTierText.text = $"Tier {currentTier + 1}";

        if (currentTier >= maxTiers - 1) // At max tier
        {
            xpSlider.value = 1f;
            xpText.text = "MAX TIER";
        }
        else
        {
            xpSlider.value = (float)currentXP / xpForNext;
            xpText.text = $"{currentXP} / {xpForNext} XP";
        }
    }

    #endregion

    #region Event Handlers

    private void HandleXPAdded(int totalXP) => RefreshXPProgress();
    
    private void HandleTierUnlocked(int tierIndex)
    {
        Debug.Log($"Guardian Architect: UI received OnTierUnlocked event for tier {tierIndex + 1}. Refreshing all tiers.");
        RefreshAllUI(); // A new tier unlock might affect multiple tiers''' states (e.g. claimable)
    }

    private void HandleRewardClaimed(int tierIndex, bool isPremium)
    {
        if (tierIndex >= 0 && tierIndex < _tierItems.Count)
        {
            Debug.Log($"Guardian Architect: UI received OnRewardClaimed event for tier {tierIndex + 1}. Refreshing specific tier.");
            _tierItems[tierIndex].Refresh(); // Only refresh the tier that was changed
        }
    }

    #endregion
}
