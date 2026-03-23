using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Updates UI elements on the Home Screen to display the player's current rank,
/// best score, and progress towards the next rank.
/// </summary>
public class RankUI : MonoBehaviour
{
    [Header("Main Rank Display")]
    [SerializeField] private Image rankBadgeImage;
    [SerializeField] private TextMeshProUGUI rankNameText;

    [Header("Progress to Next Rank")]
    [SerializeField] private GameObject progressGroup; // Parent object for progress elements
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI nextRankScoreText;

    private void OnEnable()
    {
        // Subscribe to RankManager events
        RankManager.OnRankPromoted += HandleRankPromotion;
        RankManager.OnBestScoreUpdated += HandleBestScoreUpdate;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        RankManager.OnRankPromoted -= HandleRankPromotion;
        RankManager.OnBestScoreUpdated -= HandleBestScoreUpdate;
    }

    private void Start()
    {
        // Initialize the UI with the player's current data on start
        UpdateAllUI();
    }

    private void HandleRankPromotion(LeagueTier newRank)
    {
        // A promotion happened, update everything
        UpdateAllUI();
        // You could also trigger a special animation or popup here
    }

    private void HandleBestScoreUpdate(int newBestScore)
    {
        // Only the score and progress bar need updating
        UpdateProgressUI();
    }

    /// <summary>
    /// Updates all UI components to reflect the current rank and progress.
    /// </summary>
    private void UpdateAllUI()
    {
        UpdateRankDisplay();
        UpdateProgressUI();
    }

    private void UpdateRankDisplay()
    {
        RankData currentRank = RankManager.Instance.GetCurrentRankData();
        if (currentRank != null)
        { 
            rankBadgeImage.sprite = currentRank.rankBadgeSprite;
            rankBadgeImage.color = currentRank.rankColor;
            rankNameText.text = currentRank.rankName;
        }
    }

    private void UpdateProgressUI()
    {
        int bestScore = RankManager.Instance.GetBestScore();
        RankData currentRank = RankManager.Instance.GetCurrentRankData();
        RankData nextRank = RankManager.Instance.GetNextRankData();

        bestScoreText.text = $"Best: {bestScore}";

        if (nextRank == null) // Player is at the maximum rank
        {
            progressGroup.SetActive(false);
            return;
        }

        progressGroup.SetActive(true);

        int scoreForCurrentRank = currentRank != null ? currentRank.scoreThreshold : 0;
        int scoreForNextRank = nextRank.scoreThreshold;

        nextRankScoreText.text = scoreForNextRank.ToString();

        int progressTowardsNext = bestScore - scoreForCurrentRank;
        int totalNeededForNext = scoreForNextRank - scoreForCurrentRank;

        progressSlider.value = (float)progressTowardsNext / totalNeededForNext;
    }
}
