
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Visualizes the player's progression, including level, rank, and XP bar.
/// Subscribes to the PlayerProgression manager to stay in sync.
/// </summary>
public class PlayerProgressionUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private Image rankIcon;
    [SerializeField] private Slider xpSlider;

    private void OnEnable()
    {
        // Subscribe to events
        PlayerProgression.OnLevelUp += UpdateLevelUI;
        PlayerProgression.OnXPGained += UpdateXPBar;
        PlayerProgression.OnRankUp += UpdateRankUI;

        // Initial population of UI
        if (PlayerProgression.Instance != null)
        {
            UpdateAllUI(PlayerProgression.Instance);
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        PlayerProgression.OnLevelUp -= UpdateLevelUI;
        PlayerProgression.OnXPGained -= UpdateXPBar;
        PlayerProgression.OnRankUp -= UpdateRankUI;
    }

    /// <summary>
    /// Updates all UI elements at once from a PlayerProgression instance.
    /// </summary>
    private void UpdateAllUI(PlayerProgression progressionSystem)
    {
        UpdateLevelUI(progressionSystem.GetCurrentLevel());
        UpdateRankUI(progressionSystem.GetCurrentRank());
        UpdateXPBar(progressionSystem.GetCurrentXP());
    }

    private void UpdateLevelUI(int level)
    {
        if (levelText != null)
        {
            levelText.text = $"Level {level}";
        }
    }

    private void UpdateRankUI(PlayerRank newRank)
    {
        if (rankText != null)
        {
            rankText.text = newRank.rankName;
        }
        if (rankIcon != null)
        {
            rankIcon.sprite = newRank.rankIcon;
        }
    }

    private void UpdateXPBar(long currentXP)
    {
        if (xpSlider == null || PlayerProgression.Instance == null) return;

        long xpForNext = PlayerProgression.Instance.GetXPForNextLevel();
        
        if (xpForNext > 0)
        {
            xpSlider.value = (float)currentXP / xpForNext;
        }
        else
        {
            xpSlider.value = 1f; // Max level
        }
    }
}
