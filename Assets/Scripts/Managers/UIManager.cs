
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject revivePanel;
    [SerializeField] private GameObject runSummaryPanel;

    [Header("Gameplay UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    [Header("Ghost Run UI")]
    [SerializeField] private Toggle ghostToggle;

    [Header("Fusion UI")]
    [SerializeField] private FusionUI fusionUIPrefab;

    private FusionUI fusionUIInstance;

    private void Start()
    {
        GameManager.OnGameStateChanged += HandleGameStateChange;
        ScoreManager.OnScoreChanged += UpdateScoreUI;
        if (PowerUpFusionManager.Instance != null)
        {
            PowerUpFusionManager.Instance.OnFusionActivated += HandleFusionActivation;
            PowerUpFusionManager.Instance.OnFusionDeactivated += HandleFusionDeactivation;
        }

        if(ghostToggle != null)
        {
            ghostToggle.onValueChanged.AddListener(OnGhostToggleChanged);
        }

        CreateFusionUI();
        UpdateScoreUI(0);
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChange;
        ScoreManager.OnScoreChanged -= UpdateScoreUI;
        if (PowerUpFusionManager.Instance != null)
        {
            PowerUpFusionManager.Instance.OnFusionActivated -= HandleFusionActivation;
            PowerUpFusionManager.Instance.OnFusionDeactivated -= HandleFusionDeactivation;
        }
    }

    private void HandleGameStateChange(GameState newState)
    {
        menuPanel.SetActive(newState == GameState.MainMenu);
        gameplayPanel.SetActive(newState == GameState.Gameplay);
        pausePanel.SetActive(newState == GameState.Paused);
        revivePanel.SetActive(newState == GameState.Revive);
        runSummaryPanel.SetActive(newState == GameState.RunSummary);
    }

    private void UpdateScoreUI(long newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = newScore.ToString("D8");
        }
    }

    public void UpdateBestScoreUI(long bestScore)
    {
        if (bestScoreText != null)
        {
            bestScoreText.text = "Best: " + bestScore.ToString("D8");
        }
    }

    private void OnGhostToggleChanged(bool isOn)
    {
        if (GhostRunManager.Instance != null && GhostRunManager.Instance.playback != null)
        {
            GhostRunManager.Instance.playback.gameObject.SetActive(isOn);
        }
    }

    private void CreateFusionUI()
    {
        if (fusionUIPrefab != null)
        {
            fusionUIInstance = Instantiate(fusionUIPrefab, transform);
            fusionUIInstance.gameObject.SetActive(false);
        }
    }

    private void HandleFusionActivation(FusionModifierData data)
    {
        ShowFusionUI(data.Type, data.Duration);
    }

    private void HandleFusionDeactivation()
    {
        HideFusionUI();
    }

    public void ShowFusionUI(FusionType fusionType, float duration)
    {
        if (fusionUIInstance != null) fusionUIInstance.Show(fusionType, duration);
    }

    public void HideFusionUI()
    {
        if (fusionUIInstance != null) fusionUIInstance.Hide();
    }

    public void ShowRevivePopup()
    {
        revivePanel.SetActive(true);
    }

    public void HideRevivePopup()
    {
        revivePanel.SetActive(false);
    }

    public void ShowRunSummary(RunSessionData runData)
    {
        // Logic to populate run summary UI
        runSummaryPanel.SetActive(true);
    }
}
