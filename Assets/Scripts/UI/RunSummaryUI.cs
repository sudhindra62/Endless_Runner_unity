
using UnityEngine;
using TMPro;

/// <summary>
/// Displays the end-of-run summary, including the final score, coins collected, and any bonuses.
/// </summary>
public class RunSummaryUI : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject summaryPanel;

    [Header("UI Text Elements")]
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text coinsCollectedText;
    [SerializeField] private TMP_Text styleBonusText;

    private void Start()
    {
        // Hide the panel by default
        summaryPanel.SetActive(false);
    }

    /// <summary>
    /// Shows the run summary panel with the final stats.
    /// </summary>
    /// <param name="finalScore">The player's final score.</param>
    /// <param name="coinsCollected">The number of coins collected during the run.</param>
    /// <param name="styleBonus">The style bonus awarded.</param>
    public void Show(int finalScore, int coinsCollected, int styleBonus)
    {
        finalScoreText.text = "Final Score: " + finalScore;
        coinsCollectedText.text = "Coins: " + coinsCollected;
        styleBonusText.text = "Style Bonus: " + styleBonus;

        summaryPanel.SetActive(true);
    }
}
