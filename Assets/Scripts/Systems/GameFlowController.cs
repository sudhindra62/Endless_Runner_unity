using UnityEngine;

public class GameFlowController : MonoBehaviour
{
    private GameStateManager gameStateManager;
    private ScoreManager scoreManager;
    private ReviveManager reviveManager;
    private PlayerDataManager playerDataManager;
    private PlayerController playerController;
    private NewCollectibleSpawner collectibleSpawner;

    private void Awake()
    {
        gameStateManager = ServiceLocator.Get<GameStateManager>();
        scoreManager = ServiceLocator.Get<ScoreManager>();
        reviveManager = ServiceLocator.Get<ReviveManager>();
        playerDataManager = ServiceLocator.Get<PlayerDataManager>();
        playerController = ServiceLocator.Get<PlayerController>();
        collectibleSpawner = ServiceLocator.Get<NewCollectibleSpawner>();
    }

    public void StartRun()
    {
        gameStateManager.SetState(GameState.Playing);
        scoreManager.ResetScore();
        // Reset other run-specific data
    }

    public void Pause()
    {
        gameStateManager.SetState(GameState.Paused);
    }

    public void Resume()
    {
        gameStateManager.SetState(GameState.Playing);
    }

    public void EndRun()
    {
        gameStateManager.SetState(GameState.GameOver);
        playerDataManager.SavePlayerScore(scoreManager.CurrentScore);
    }

    public void GoToMainMenu()
    {
        gameStateManager.SetState(GameState.MainMenu);
    }

    public void PlayerDied()
    {
        if (reviveManager.CanRevive())
        {
            reviveManager.OfferRevive();
        }
        else
        {
            EndRun();
        }
    }

    public void PlayerRevived()
    {
        playerController.health = 100; // Or some other revival logic
        gameStateManager.SetState(GameState.Playing);
    }
}