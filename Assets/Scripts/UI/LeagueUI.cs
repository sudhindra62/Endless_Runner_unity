
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class LeagueUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image leagueBadge;
    [SerializeField] private Slider progressBar;
    [SerializeField] private RectTransform promotionZone;
    [SerializeField] private RectTransform demotionZone;
    [SerializeField] private Text countdownTimer;
    [SerializeField] private Transform rewardPreviewContainer;
    [SerializeField] private GameObject rewardPreviewPrefab;

    private void OnEnable()
    {
        LeagueManager.OnLeagueDataUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        LeagueManager.OnLeagueDataUpdated -= UpdateUI;
    }

    private void Update()
    {
        if (LeagueManager.Instance != null)
        {
            TimeSpan timeRemaining = (LeagueManager.Instance.WeeklyCycleStartTime.AddDays(7)) - DateTime.UtcNow;
            if (timeRemaining.TotalSeconds > 0)
            {
                countdownTimer.text = string.Format("Resets in: {0:D2}h {1:D2}m {2:D2}s", timeRemaining.Hours, timeRemaining.Minutes, timeRemaining.Seconds);
            }
            else
            {
                countdownTimer.text = "Resetting...";
            }
        }
    }

    private void UpdateUI()
    {
        if (LeagueManager.Instance == null) return;

        LeagueTier currentLeague = LeagueManager.Instance.CurrentLeague;
        int weeklyBestScore = LeagueManager.Instance.WeeklyBestScore;
        
        // In a real game, you would have different sprites for each league badge
        //leagueBadge.sprite = GetLeagueBadgeSprite(currentLeague);

        // This is a simplified progress bar. A real implementation would be more complex.
        float progress = 0;
        if (leagueData.ContainsKey(currentLeague))
        {
            LeagueManager.LeagueTierData tierData = leagueData[currentLeague];
            progress = (float)weeklyBestScore / tierData.promotionScore;
            progressBar.value = progress;

            float promotionStart = (float)tierData.promotionScore / tierData.promotionScore;
            promotionZone.anchorMin = new Vector2(promotionStart, 0);

            float demotionEnd = (float)tierData.scoreThreshold / tierData.promotionScore;
            demotionZone.anchorMax = new Vector2(demotionEnd, 1);
        }


        PopulateRewardPreview(currentLeague);
    }

    private void PopulateRewardPreview(LeagueTier league)
    {
        foreach (Transform child in rewardPreviewContainer)
        {
            Destroy(child.gameObject);
        }

        if (leagueData.ContainsKey(league))
        {
            List<LeagueManager.LeagueReward> rewards = leagueData[league].rewards;
            foreach (LeagueManager.LeagueReward reward in rewards)
            {
                GameObject rewardGO = Instantiate(rewardPreviewPrefab, rewardPreviewContainer);
                // Set icon and amount on the reward prefab
            }
        }
    }

    //This is a placeholder for the real league data
    private Dictionary<LeagueTier, LeagueManager.LeagueTierData> leagueData = new Dictionary<LeagueTier, LeagueManager.LeagueTierData>
    {
        { LeagueTier.Bronze, new LeagueManager.LeagueTierData(0, 10000, new List<LeagueManager.LeagueReward> { new LeagueManager.LeagueReward(RewardType.Coins, 100) }) },
        { LeagueTier.Silver, new LeagueManager.LeagueTierData(10000, 50000, new List<LeagueManager.LeagueReward> { new LeagueManager.LeagueReward(RewardType.Coins, 250), new LeagueManager.LeagueReward(RewardType.XP, 50) }) },
        { LeagueTier.Gold, new LeagueManager.LeagueTierData(50000, 150000, new List<LeagueManager.LeagueReward> { new LeagueManager.LeagueReward(RewardType.Coins, 500), new LeagueManager.LeagueReward(RewardType.Gems, 10), new LeagueManager.LeagueReward(RewardType.XP, 100) }) },
        { LeagueTier.Platinum, new LeagueManager.LeagueTierData(150000, 500000, new List<LeagueManager.LeagueReward> { new LeagueManager.LeagueReward(RewardType.Coins, 1000), new LeagueManager.LeagueReward(RewardType.Gems, 25), new LeagueManager.LeagueReward(RewardType.XP, 250) }) },
        { LeagueTier.Diamond, new LeagueManager.LeagueTierData(500000, int.MaxValue, new List<LeagueManager.LeagueReward> { new LeagueManager.LeagueReward(RewardType.Coins, 2500), new LeagueManager.LeagueReward(RewardType.Gems, 50), new LeagueManager.LeagueReward(RewardType.Chest, 1), new LeagueManager.LeagueReward(RewardType.XP, 500) }) }
    };
}
