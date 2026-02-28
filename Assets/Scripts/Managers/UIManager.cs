
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject revivePanel;
    [SerializeField] private GameObject runSummaryPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameManager.OnGameStateChanged += HandleGameStateChange;
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
