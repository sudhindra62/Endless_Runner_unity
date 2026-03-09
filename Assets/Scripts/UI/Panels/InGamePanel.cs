
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : UIPanel
{
    public override UIPanelType PanelType => UIPanelType.InGame;

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _coinText;

    private void OnEnable()
    {
        ScoreManager.OnScoreUpdated += UpdateScore;
        ScoreManager.OnCoinsUpdated += UpdateCoins;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreUpdated -= UpdateScore;
        ScoreManager.OnCoinsUpdated -= UpdateCoins;
    }

    private void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    private void UpdateCoins(int coins)
    {
        _coinText.text = "Coins: " + coins;
    }
}
