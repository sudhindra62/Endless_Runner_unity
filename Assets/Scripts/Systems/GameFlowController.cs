
using UnityEngine;

public class GameFlowController : Singleton<GameFlowController>
{
    [Header("Component References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TileSpawner tileSpawner;
    [SerializeField] private RevivePopupUI revivePopupUI;
    [SerializeField] private RunSummaryUI runSummaryUI;

    private RunSessionData runSessionData;
    private PowerUpManager powerUpManager;
    private ScoreManager scoreManager;
    private GameDifficultyManager difficultyManager;
    private ReviveManager reviveManager;
    private PlayerDataManager playerDataManager;
    private BossChaseManager bossChaseManager;

    private float runStartTime;

    protected override void Awake()
    {
        base.Awake();
        runSessionData = new RunSessionData();
    }

    private void Start()
    {
        GameStateManager.OnGameStateChanged += HandleGameStateChange;

        powerUpManager = ServiceLocator.Get<PowerUpManager>();
        scoreManager = ServiceLocator.Get<ScoreManager>();
        difficultyManager = ServiceLocator.Get<GameDifficultyManager>();
        reviveManager = ServiceLocator.Get<ReviveManager>();
        playerDataManager = ServiceLocator.Get<PlayerDataManager>();
        bossChaseManager = BossChaseManager.Instance;

        playerController.OnDeath += PlayerDied;
    }

    private void OnDestroy()
    {
        GameStateManager.OnGameStateChanged -= HandleGameStateChange;
        if (playerController != null)
        {
            playerController.OnDeath -= PlayerDied;
        }
    }

    public void StartRun()
    {
        runSessionData.Reset();
        scoreManager.ResetScore();
        difficultyManager.ResetDifficulty();
        powerUpManager.ResetPowerUps();
        tileSpawner.ResetTiles();

        runStartTime = Time.time;

        GameStateManager.CurrentState = GameState.Playing;
    }

    private void PlayerDied()
    {
        runSessionData.time = Time.time - runStartTime;
        if (bossChaseManager != null) 
        {
            bossChaseManager.OnPlayerDied();
        }
        GameStateManager.CurrentState = GameState.Dead;
    }

    private void HandleGameStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.Playing:
                scoreManager.Resume();
                difficultyManager.Resume();
                break;
            case GameState.Dead:
                scoreManager.Pause();
                difficultyManager.Pause();
                powerUpManager.PauseAllPowerUps();
                if (reviveManager.CanRevive(runSessionData))
                {
                    revivePopupUI.Show();
                }
                else
                {
                    EndRun();
                }
                break;

            case GameState.EndOfRun:
                FinalizeRun();
                break;
        }
    }

    public void OnReviveAccepted(ReviveManager.ReviveType reviveType)
    {
        revivePopupUI.Hide();
        reviveManager.StartRevive(runSessionData, playerController, reviveType, () =>
        {
            runSessionData.hasRevived = true;
            // Power-Up Revive Policy (Option B): Resume power-ups
            powerUpManager.ResumeAllPowerUps();
            GameStateManager.CurrentState = GameState.Playing;
        });
    }

    public void OnReviveDeclined()
    {
        revivePopupUI.Hide();
        EndRun();
    }

    private void EndRun()
    {
        GameStateManager.CurrentState = GameState.EndOfRun;
    }

    private void FinalizeRun()
    {
        runSessionData.score = scoreManager.CurrentScore;

        playerDataManager.UpdateBestScore(runSessionData.score);
        playerDataManager.UpdateBestTime(runSessionData.time);
        
        runSummaryUI.Show(runSessionData);
    }
}
