
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Represents a single tier in the Battle Pass UI.
/// </summary>
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
    [SerializeField] private GameObject rewardItemPrefab; // A simple prefab with an Image and Text

    private int tierIndex;
    private BattlePassTier tierData;

    public void Setup(int index, BattlePassTier data)
    {
        tierIndex = index;
        tierData = data;
        tierNumberText.text = (index + 1).ToString();

        // Populate rewards
        PopulateRewardContainer(freeRewardContainer.transform, data.freeRewards);
        PopulateRewardContainer(premiumRewardContainer.transform, data.premiumRewards);

        claimFreeButton.onClick.AddListener(() => ClaimReward(false));
        claimPremiumButton.onClick.AddListener(() => ClaimReward(true));

        Refresh();
    }

    public void Refresh()
    {
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
        BattlePassManager.Instance.ClaimReward(tierIndex, isPremium);
        Refresh(); // Update UI after claiming
    }

    private void PopulateRewardContainer(Transform container, System.Collections.Generic.List<RewardItem> rewards)
    {
        foreach (var reward in rewards)
        {
            GameObject itemGO = Instantiate(rewardItemPrefab, container);
            itemGO.GetComponent<Image>().sprite = reward.icon;
            itemGO.GetComponentInChildren<TMP_Text>().text = reward.quantity.ToString();
        }
    }
    private void OnDestroy()
    {
        claimFreeButton.onClick.RemoveAllListeners();
        claimPremiumButton.onClick.RemoveAllListeners();
    }
}
