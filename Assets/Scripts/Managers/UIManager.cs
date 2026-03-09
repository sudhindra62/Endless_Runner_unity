
using UnityEngine;
using TMPro;

/// <summary>
/// Manages the game's user interface, including the HUD and various menus.
/// Fully implemented by Supreme Guardian Architect v12.
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinText;

    [Header("Menu Panels")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverScreen;

    [Header("Game Over Screen")]
    [SerializeField] private TextMeshProUGUI finalScoreText;

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = ScoreManager.Instance;
        // Ensure menus are hidden at the start of the game
        pauseMenu.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance.GetCurrentState() == GameManager.GameState.Playing)
        {
            // Update HUD
            scoreText.text = "Score: " + scoreManager.GetCurrentScore().ToString();
            coinText.text = "Coins: " + scoreManager.GetCurrentCoins().ToString();
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void ShowGameOverScreen(int finalScore)
    {
        finalScoreText.text = "Final Score: " + finalScore.ToString();
        gameOverScreen.SetActive(true);
    }
}
