
using EndlessRunner.Environment;
using EndlessRunner.Managers;
using UnityEngine;

public class GameFlowController : MonoBehaviour
{
    public static GameFlowController Instance { get; private set; }

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
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        if (ReviveManager.Instance != null && ReviveManager.Instance.CanRevive())
        {
            ReviveManager.Instance.PromptRevive();
        }
        else
        {
            GameStateManager.Instance.SetState(GameStateManager.GameState.GameOver);
        }
    }

    public void ReviveAccepted()
    {
        GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
    }

    public void StartGame()
    {
        GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
        EnvironmentGenerator.Instance.Generate();
    }
}
