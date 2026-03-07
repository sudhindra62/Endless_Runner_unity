
using UnityEngine;

/// <summary>
/// Manages all UI panels and their visibility.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v1 for a modular, event-driven architecture.
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [Header("UI Panels")]
    [SerializeField] private UIPanel_MainMenu mainMenuPanel;
    [SerializeField] private UIPanel_InGame inGamePanel;
    [SerializeField] private UIPanel_GameOver gameOverPanel;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameManager.GameState newState)
    {
        // Hide all panels first
        mainMenuPanel.Hide();
        inGamePanel.Hide();
        gameOverPanel.Hide();

        // Show the correct panel based on the new game state
        switch (newState)
        {
            case GameManager.GameState.MainMenu:
                mainMenuPanel.Show();
                break;
            case GameManager.GameState.Playing:
                inGamePanel.Show();
                break;
            case GameManager.GameState.GameOver:
                gameOverPanel.Show();
                break;
            case GameManager.GameState.Paused:
                // You might want a separate pause panel
                inGamePanel.Show(); // Or just keep the in-game UI visible
                break;
        }
    }
}
