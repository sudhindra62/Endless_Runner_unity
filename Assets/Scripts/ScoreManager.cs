using UnityEngine;
using TMPro;

public partial class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public static ScoreManager instance { get { return Instance; } }

    [Header("HUD References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private GameObject restartButton;

    [Header("Firebase Extension")]
    public int survivalScoreRate = 5; // preserved (not auto used)

    private int score;
    private int bestScore;
    private int combo = 1;

    private float timer;
    private float bestTime;
    private float scoreFromDistance;

    private bool isGameOver;

    private PlayerController playerController;

    private int lastDisplayedScore = -1;
    private int lastDisplayedTime = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            bestScore = PlayerPrefs.GetInt("BestScore", 0);
            bestTime = PlayerPrefs.GetFloat("BestTime", 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterPlayer(PlayerController player)
    {
        playerController = player;
    }

    private void Update()
    {
        if (playerController == null || playerController.IsDead || isGameOver)
            return;

        timer += Time.deltaTime;
        UpdateTimerUI();
    }

    public void AddScoreFromDistance(float distance)
    {
        scoreFromDistance += distance;
        int scoreToAdd = Mathf.FloorToInt(scoreFromDistance);

        if (scoreToAdd > 0)
        {
            AddScore(scoreToAdd);
            scoreFromDistance -= scoreToAdd;
        }
    }

    public void AddScore(int amount)
    {
        if (amount <= 0) return;

        score += amount * combo;
        UpdateScoreUI();
    }

    public void GameOver()
    {
        isGameOver = true;

        if (restartButton != null)
            restartButton.SetActive(true);

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }

        if (timer > bestTime)
        {
            bestTime = timer;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        UpdateUI();
    }

    public void RestartGame()
    {
        ResetScore();

        if (Time.timeScale == 0f)
            Time.timeScale = 1f;

        var player = FindFirstObjectByType<PlayerController>();
        var tiles = FindFirstObjectByType<TileSpawner>();
        var obstacles = FindFirstObjectByType<ObstacleSpawner>();

        if (player != null) player.ResetPlayer();
        if (tiles != null) tiles.ResetTiles();
        if (obstacles != null) obstacles.ResetSpawner();

        if (restartButton != null)
            restartButton.SetActive(false);

        isGameOver = false;
    }

    public void ResetScore()
    {
        score = 0;
        timer = 0f;
        combo = 1;
        scoreFromDistance = 0f;
        lastDisplayedScore = -1;
        lastDisplayedTime = -1;
        UpdateUI();
    }

    public int GetCurrentScore() => score;

    private void UpdateUI()
    {
        UpdateScoreUI();
        UpdateTimerUI();

        if (bestScoreText != null)
            bestScoreText.text = bestScore.ToString();

        if (bestTimeText != null)
            bestTimeText.text = Mathf.FloorToInt(bestTime).ToString();

        if (comboText != null)
            comboText.text = "x" + combo;
    }

    private void UpdateScoreUI()
    {
        if (score == lastDisplayedScore) return;

        var hud = GameHUDController.Instance;
        if (hud != null && hud.ScoreText != null)
            hud.ScoreText.text = score.ToString();

        lastDisplayedScore = score;
    }

    private void UpdateTimerUI()
    {
        int currentTime = Mathf.FloorToInt(timer);
        if (currentTime == lastDisplayedTime) return;

        var hud = GameHUDController.Instance;
        if (hud != null && hud.TimerText != null)
        {
            int minutes = currentTime / 60;
            int seconds = currentTime % 60;
            hud.TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        lastDisplayedTime = currentTime;
    }
    // --- Compatibility Method (DO NOT REMOVE) ---
public int GetScore()
{
    return score;
}

}
