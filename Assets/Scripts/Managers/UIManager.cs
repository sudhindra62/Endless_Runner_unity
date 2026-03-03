
using UnityEngine;
using TMPro; // Added for TextMeshPro integration

public class UIManager : Singleton<UIManager>
{
    [Header("UI Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject revivePanel;
    [SerializeField] private GameObject runSummaryPanel;

    [Header("Gameplay UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText; // Assign the gameplay score text element in the Inspector

    [Header("Fusion UI")]
    [SerializeField] private FusionUI fusionUIPrefab; // Assign in Inspector

    private FusionUI fusionUIInstance;

    private void Start()
    {
        // Subscribe to Game State, Score, and Fusion events
        GameManager.OnGameStateChanged += HandleGameStateChange;
        ScoreManager.OnScoreChanged += UpdateScoreUI;
        PowerUpFusionManager.OnFusionActivated += HandleFusionActivation; // Subscribe to fusion activation
        PowerUpFusionManager.OnFusionDeactivated += HandleFusionDeactivation; // Subscribe to fusion deactivation

        CreateFusionUI();
        
        // Initialize score text on game start
        UpdateScoreUI(0); 
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        GameManager.OnGameStateChanged -= HandleGameStateChange;
        ScoreManager.OnScoreChanged -= UpdateScoreUI;
        if (PowerUpFusionManager.Instance != null) {
            PowerUpFusionManager.OnFusionActivated -= HandleFusionActivation;
            PowerUpFusionManager.OnFusionDeactivated -= HandleFusionDeactivation;
        }
    }

    private void HandleGameStateChange(GameState newState)
    {
        menuPanel.SetActive(newState == GameState.Menu);
        gameplayPanel.SetActive(newState == GameState.Playing);
        pausePanel.SetActive(newState == GameState.Paused);
        revivePanel.SetActive(newState == GameState.Dead);
        runSummaryPanel.SetActive(newState == GameState.EndOfRun);
    }
    
    private void UpdateScoreUI(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = newScore.ToString("D8"); 
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

    // --- Fusion UI Handlers ---
    private void HandleFusionActivation(FusionModifierData data)
    {
        ShowFusionUI(data.Type, data.Duration);
    }

    private void HandleFusionDeactivation(FusionType fusionType)
    {
        // The parameter is passed from the event, but our Hide method doesn't need it.
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

    // --- Other UI Methods ---
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
