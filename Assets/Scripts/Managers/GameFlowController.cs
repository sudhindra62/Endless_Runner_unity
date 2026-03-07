
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles high-level game flow, scene transitions, and game state orchestration.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v2 for full functionality.
/// </summary>
public class GameFlowController : MonoBehaviour
{
    public static GameFlowController Instance { get; private set; }

    // Scene names, ideally stored in a more robust config
    private const string MAIN_MENU_SCENE = "MainMenu";
    private const string GAME_SCENE = "Game";
    private const string SHOP_SCENE = "Shop";

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

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDied += HandlePlayerDeath;
        // In a real scenario, you'd have a SceneLoader class with async loading
        // For now, we use Unity's standard SceneManager
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        if (GameManager.Instance != null)
        {
            // First, check if a revive is possible
            if (ReviveManager.Instance != null && ReviveManager.Instance.CanRevive())
            {
                ReviveManager.Instance.PromptRevive();
            }
            else
            {
                GameManager.Instance.SetState(GameManager.GameState.GameOver);
            }
        }
    }

    public void ReviveAccepted()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetState(GameManager.GameState.Playing);
            // Additional logic to reset player state without resetting score might be needed here
        }
    }

    public void ToMainMenu()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetState(GameManager.GameState.MainMenu);
        }
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }

    public void ToGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetState(GameManager.GameState.Playing);
        }
        SceneManager.LoadScene(GAME_SCENE);
    }

    public void ToShop()
    {
        // In a real game, you might pause the current state or handle it differently
        SceneManager.LoadScene(SHOP_SCENE);
    }
}
