
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("UI Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject revivePanel;
    [SerializeField] private GameObject runSummaryPanel;
    [SerializeField] private FusionUI fusionUIPrefab; // Assign in Inspector

    private FusionUI fusionUIInstance;

    private void Start()
    {
        GameManager.OnGameStateChanged += HandleGameStateChange;
        CreateFusionUI();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChange;
    }

    private void HandleGameStateChange(GameState newState)
    {
        menuPanel.SetActive(newState == GameState.Menu);
        gameplayPanel.SetActive(newState == GameState.Playing);
        pausePanel.SetActive(newState == GameState.Paused);
        revivePanel.SetActive(newState == GameState.Dead);
        runSummaryPanel.SetActive(newState == GameState.EndOfRun);
    }

    private void CreateFusionUI()
    {
        if (fusionUIPrefab != null)
        {
            fusionUIInstance = Instantiate(fusionUIPrefab, transform);
            fusionUIInstance.gameObject.SetActive(false);
        }
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
