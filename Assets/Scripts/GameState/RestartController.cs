using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartController : MonoBehaviour
{
    private bool isRestarting = false;
    private const string GameplaySceneName = "MainGameplay";

    private RunSessionData runSessionData;
    private MetaProgressionManager metaProgressionManager;

    void Awake()
    {
        runSessionData = FindFirstObjectByType<RunSessionData>();
        metaProgressionManager = FindFirstObjectByType<MetaProgressionManager>();
    }

    public void RestartGame()
    {
        if (isRestarting) return;

        if (GameStateManager.Instance != null)
        {
            var currentState = GameStateManager.Instance.CurrentState;
            if (currentState != GameState.GameOver && currentState != GameState.Paused)
                return;

            GameStateManager.Instance.SetState(GameState.Restarting);
        }

        isRestarting = true;

        runSessionData?.Reset();
        metaProgressionManager?.OnRunStart();

        if (ObstacleRegistry.Instance != null)
        {
            ObstacleRegistry.ClearAll();

        }

        SceneManager.LoadScene(GameplaySceneName);
    }
}
