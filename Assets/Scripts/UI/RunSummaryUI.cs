
using UnityEngine;
using UnityEngine.UI;

public class RunSummaryUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Text coinsCollectedText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        if (restartButton != null) restartButton.onClick.AddListener(OnRestart);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(OnMainMenu);
    }

    public void ShowSummary(int finalScore, int coinsCollected)
    {
        if (finalScoreText != null) finalScoreText.text = "Final Score: " + finalScore;
        if (coinsCollectedText != null) coinsCollectedText.text = "Coins: " + coinsCollected;
        gameObject.SetActive(true);
    }

    private void OnRestart()
    {
        // Assuming a GameManager exists
        // GameManager.Instance.RestartGame();
        gameObject.SetActive(false);
    }

    private void OnMainMenu()
    {
        // GameManager.Instance.GoToMainMenu();
        gameObject.SetActive(false);
    }
}
