
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattlePassUIController : MonoBehaviour
{
    [SerializeField] private GameObject battlePassPanel;
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button buyPremiumButton;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private Text levelText;
    [SerializeField] private Transform freeTrackContainer;
    [SerializeField] private Transform premiumTrackContainer;
    [SerializeField] private GameObject rewardIconPrefab; // Prefab for displaying a reward

    void Start()
    {
        openButton.onClick.AddListener(() => battlePassPanel.SetActive(true));
        closeButton.onClick.AddListener(() => battlePassPanel.SetActive(false));
        buyPremiumButton.onClick.AddListener(OnBuyPremiumClicked);

        BattlePassManager.OnXPUpdated += UpdateXPUI;
        BattlePassManager.OnLevelUp += OnLevelUp;

        battlePassPanel.SetActive(false);
        InitializeUI();
    }

    private void OnDestroy()
    {
        BattlePassManager.OnXPUpdated -= UpdateXPUI;
        BattlePassManager.OnLevelUp -= OnLevelUp;
    }

    private void InitializeUI()
    {
        var data = BattlePassManager.Instance.GetBattlePassData();
        UpdateXPUI(data.currentXP, 100); // Assuming 100 XP per level
        levelText.text = $"Level {data.currentLevel}";
        buyPremiumButton.interactable = !data.hasPremiumPass;
        PopulateRewardTracks();
    }

    private void PopulateRewardTracks()
    {
        List<BattlePassLevelReward> rewards = BattlePassManager.Instance.GetSeasonRewards();

        foreach (var reward in rewards)
        {
            // Instantiate and setup free track reward icons
            if (reward.freeTrackReward != null)
            {
                GameObject icon = Instantiate(rewardIconPrefab, freeTrackContainer);
                // Configure the icon with reward details
            }

            // Instantiate and setup premium track reward icons
            if (reward.premiumTrackReward != null)
            {
                GameObject icon = Instantiate(rewardIconPrefab, premiumTrackContainer);
                // Configure the icon with reward details, and a lock if not premium
            }
        }
    }

    private void UpdateXPUI(int currentXP, int xpToNextLevel)
    {
        xpSlider.value = (float)currentXP / xpToNextLevel;
    }

    private void OnLevelUp(int newLevel)
    {
        levelText.text = $"Level {newLevel}";
        // You might want to play a level-up animation
    }

    private void OnBuyPremiumClicked()
    {
        BattlePassManager.Instance.PurchasePremiumPass();
        buyPremiumButton.interactable = false;
        // Refresh UI to show unlocked premium track
    }
}
