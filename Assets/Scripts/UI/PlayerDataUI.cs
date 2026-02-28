
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the UI with the player's currency and progression data.
/// This script should be attached to a UI canvas or a manager object in the scene.
/// </summary>
public class PlayerDataUI : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("Text element to display the player's coin balance.")]
    public Text coinText;

    [Tooltip("Text element to display the player's gem balance.")]
    public Text gemText;

    [Tooltip("Text element to display the player's level.")]
    public Text levelText;

    [Tooltip("Slider or image fill element to display the player's XP progress.")]
    public Slider xpSlider;

    private void OnEnable()
    {
        // Subscribe to events from the PlayerDataManager to update the UI.
        PlayerDataManager.OnCoinsChanged += UpdateCoinText;
        PlayerDataManager.OnGemsChanged += UpdateGemText;
        PlayerDataManager.OnLevelChanged += UpdateLevelText;
        PlayerDataManager.OnXPChanged += UpdateXPSlider;
    }

    private void OnDisable()
    {
        // Unsubscribe from events to prevent memory leaks.
        PlayerDataManager.OnCoinsChanged -= UpdateCoinText;
        PlayerDataManager.OnGemsChanged -= UpdateGemText;
        PlayerDataManager.OnLevelChanged -= UpdateLevelText;
        PlayerDataManager.OnXPChanged -= UpdateXPSlider;
    }

    // --- UI Update Methods ---

    private void UpdateCoinText(int newBalance)
    {
        if (coinText != null)
        {
            coinText.text = newBalance.ToString();
        }
    }

    private void UpdateGemText(int newBalance)
    {
        if (gemText != null)
        {
            gemText.text = newBalance.ToString();
        }
    }

    private void UpdateLevelText(int newLevel)
    {
        if (levelText != null)
        {
            levelText.text = "Level " + newLevel.ToString();
        }
    }

    private void UpdateXPSlider(int currentXP, int xpForNextLevel)
    {
        if (xpSlider != null)
        {
            xpSlider.value = (float)currentXP / xpForNextLevel;
        }
    }
}
