
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Represents a single tier in the Battle Pass UI.
/// Logic fully restored and fortified by Supreme Guardian Architect v12.
/// </summary>
[AddComponentMenu("UI/Battle Pass/Battle Pass Tier UI")]
public class BattlePassTierUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text tierNumberText;
    [SerializeField] private GameObject freeRewardContainer;
    [SerializeField] private GameObject premiumRewardContainer;
    [SerializeField] private Button claimFreeButton;
    [SerializeField] private Button claimPremiumButton;
    [SerializeField] private GameObject freeClaimedOverlay;
    [SerializeField] private GameObject premiumClaimedOverlay;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private RewardItemUI rewardItemPrefab; // Prefab with a RewardItemUI component

    private int tierIndex;
    private BattlePassTier tierData;

    private void OnDestroy()
    {
        // --- MEMORY_MANAGEMENT_MANDATE: Unsubscribe from events ---
        claimFreeButton.onClick.RemoveAllListeners();
        claimPremiumButton.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Sets up the tier UI with data from a BattlePassTier asset.
    /// </summary>
    public void Setup(int index, BattlePassTier data)
    {
        tierIndex = index;
        tierData = data;
        tierNumberText.text = (index + 1).ToString();

        // --- ERROR_HANDLING_POLICY: Validate prefab assignment ---
        if (rewardItemPrefab == null)
        {
            Debug.LogError("Guardian Architect FATAL_ERROR: rewardItemPrefab is not assigned in BattlePassTierUI. Disabling.", this);
            enabled = false;
            return;
        }

        // Populate reward containers with the new, robust RewardItemUI prefab
        PopulateRewardContainer(freeRewardContainer.transform, data.freeRewards);
        PopulateRewardContainer(premiumRewardContainer.transform, data.premiumRewards);

        // Add listeners for the claim buttons
        claimFreeButton.onClick.AddListener(() => ClaimReward(false));
        claimPremiumButton.onClick.AddListener(() => ClaimReward(true));

        // Initial visual state update
        Refresh();
    }

    /// <summary>
    /// Refreshes the visual state of the tier (locks, claimed overlays, button visibility).
    /// </summary>
    public void Refresh()
    {
        // --- SINGLE_SOURCE_OF_TRUTH: Query the manager for all state ---
        bool isUnlocked = BattlePassManager.Instance.GetCurrentTier() >= tierIndex;
        bool hasPremium = BattlePassManager.Instance.HasPremium();

        // Free Track
        bool freeClaimed = BattlePassManager.Instance.IsRewardClaimed(tierIndex, false);
        freeClaimedOverlay.SetActive(freeClaimed);
        claimFreeButton.gameObject.SetActive(isUnlocked && !freeClaimed);

        // Premium Track
        bool premiumClaimed = BattlePassManager.Instance.IsRewardClaimed(tierIndex, true);
        premiumClaimedOverlay.SetActive(premiumClaimed);
        claimPremiumButton.gameObject.SetActive(isUnlocked && hasPremium && !premiumClaimed);
        lockIcon.SetActive(!hasPremium);
    }

    private void ClaimReward(bool isPremium)
    {
        // --- DELEGATION_MANDATE: All logic is handled by the manager ---
        BattlePassManager.Instance.ClaimReward(tierIndex, isPremium);
        Refresh(); // Refresh this specific tier UI after a claim action
    }

    /// <summary>
    /// Instantiates and sets up RewardItemUI prefabs in the specified container.
    /// </summary>
    private void PopulateRewardContainer(Transform container, System.Collections.Generic.List<RewardItem> rewards)
    {
        // Clear any existing children
        foreach (Transform child in container) { Destroy(child.gameObject); }

        foreach (var reward in rewards)
        {
            RewardItemUI newRewardUI = Instantiate(rewardItemPrefab, container);
            newRewardUI.Setup(reward); // Use the dedicated Setup method
        }
    }
}
