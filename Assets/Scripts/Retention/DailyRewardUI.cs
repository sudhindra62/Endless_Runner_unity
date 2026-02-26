using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DailyRewardUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Button claimButton;
    [SerializeField] private TMP_Text claimButtonText;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private TMP_Text rewardText;

    [Header("Reward Sprites")]
    [SerializeField] private Sprite coinSprite;
    [SerializeField] private Sprite gemSprite;
    [SerializeField] private Sprite chestSprite;
    [SerializeField] private Sprite megaChestSprite;

    private void OnEnable()
    {
        if (DailyRewardManager.Instance == null)
        {
            Debug.LogError("DailyRewardUI: DailyRewardManager instance not found!");
            gameObject.SetActive(false);
            return;
        }

        DailyRewardManager.OnRewardStateChanged += UpdateUI;
        claimButton.onClick.AddListener(OnClaimButtonPressed);
        UpdateUI();
    }

    private void OnDisable()
    {
        if (DailyRewardManager.Instance != null)
        {
            DailyRewardManager.OnRewardStateChanged -= UpdateUI;
        }
        claimButton.onClick.RemoveListener(OnClaimButtonPressed);
    }

    private void Update()
    {
        if (!DailyRewardManager.Instance.IsRewardAvailable())
        {
            UpdateTimer();
        }
    }

    private void UpdateUI()
    {
        bool isAvailable = DailyRewardManager.Instance.IsRewardAvailable();

        claimButton.interactable = isAvailable;

        if (isAvailable)
        {
            if (claimButtonText) claimButtonText.text = "Claim";
            if (timerText) timerText.text = "Your reward is ready!";
        }
        else
        {
            if (claimButtonText) claimButtonText.text = "Claimed";
            UpdateTimer(); 
        }

        DisplayReward(DailyRewardManager.Instance.GetCurrentReward());
    }

    private void UpdateTimer()
    {
        if (timerText == null) return;

        TimeSpan timeUntilNextClaim = DailyRewardManager.Instance.GetNextClaimTime() - DateTime.UtcNow;

        if (timeUntilNextClaim.TotalSeconds > 0)
        {
            timerText.text = $"Next reward in: {timeUntilNextClaim.Hours:D2}:{timeUntilNextClaim.Minutes:D2}:{timeUntilNextClaim.Seconds:D2}";
        }
        else
        {
            UpdateUI();
        }
    }

    private void DisplayReward(DailyReward reward)
    {
        if (rewardText != null) rewardText.text = GetRewardDescription(reward);
        if (rewardIcon != null) rewardIcon.sprite = GetRewardSprite(reward.Type);
    }

    private void OnClaimButtonPressed()
    {
        DailyRewardManager.Instance.ClaimReward();
    }
    
    private string GetRewardDescription(DailyReward reward)
    {
        switch (reward.Type)
        {
            case RewardType.Coins: return $"{reward.Amount} Coins";
            case RewardType.Gems: return $"{reward.Amount} Gems";
            case RewardType.Chest: return "A Basic Chest";
            case RewardType.MegaChest: return "A Mega Chest!";
            default: return "";
        }
    }

    private Sprite GetRewardSprite(RewardType type)
    {
        switch (type)
        {
            case RewardType.Coins: return coinSprite;
            case RewardType.Gems: return gemSprite;
            case RewardType.Chest: return chestSprite;
            case RewardType.MegaChest: return megaChestSprite;
            default: return null;
        }
    }
}
