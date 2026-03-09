
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [Header("In-Game HUD")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinsText;

    [Header("Game Over Screen")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private Button restartButton;

    protected override void Awake()
    {
        base.Awake();
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(GameManager.Instance.RestartGame);
        }
    }

    private void Start()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged += UpdateScore;
            ScoreManager.Instance.OnCoinsChanged += UpdateCoins;
        }

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateScore;
            ScoreManager.Instance.OnCoinsChanged -= UpdateCoins;
        }
    }

    private void UpdateScore(int newScore)
    {
        if (scoreText != null) scoreText.text = "Score: " + newScore;
    }

    private void UpdateCoins(int newCoins)
    {
        if (coinsText != null) coinsText.text = "Coins: " + newCoins;
    }

    public void ShowGameOverScreen()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (finalScoreText != null && ScoreManager.Instance != null)
        {
            finalScoreText.text = "Final Score: " + ScoreManager.Instance.GetScore();
        }
    }
}
