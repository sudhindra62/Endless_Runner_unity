using UnityEngine;
using TMPro; // ✅ REQUIRED for TextMeshProUGUI

/// <summary>
/// Manages the player's score and the game timer.
/// Optimized to decouple score calculation from its own Update loop and to reduce UI updates.
/// </summary>
public partial class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public static ScoreManager instance { get { return Instance; } } // legacy compatibility
[SerializeField] private TextMeshProUGUI scoreText;
[SerializeField] private TextMeshProUGUI bestScoreText;
[SerializeField] private TextMeshProUGUI timerText;
[SerializeField] private TextMeshProUGUI bestTimeText;
[SerializeField] private TextMeshProUGUI comboText;
[SerializeField] private GameObject restartButton;


    private int score;
    private float timer;
    private float scoreFromDistance; // Accumulates fractional score from distance.

    private PlayerController playerController; // Set via registration for performance.

    // --- UI Update Optimization ---
    private int lastDisplayedScore = -1;
    private int lastDisplayedTime = -1;
    // ---

    public void RestartGame()
{
    ResetScore();
}
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

    /// <summary>
    /// The PlayerController calls this to register itself, avoiding FindObjectOfType.
    /// </summary>
    public void RegisterPlayer(PlayerController player)
    {
        playerController = player;
    }

    private void Update()
    {
        if (playerController == null || playerController.IsDead) return;

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
        score += amount;
        UpdateScoreUI();
    }

    public void ResetScore()
    {
        score = 0;
        timer = 0f;
        scoreFromDistance = 0f;
        lastDisplayedScore = -1;
        lastDisplayedTime = -1;
        UpdateUI();
    }

    public int GetCurrentScore()
    {
        return score;
    }

    private void UpdateUI()
    {
        UpdateScoreUI();
        UpdateTimerUI();
    }

    private void UpdateScoreUI()
    {
        if (score == lastDisplayedScore) return;

        var hud = GameHUDController.Instance;
        if (hud != null && hud.ScoreText != null)
        {
            hud.ScoreText.text = score.ToString();
            lastDisplayedScore = score;
        }
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
            lastDisplayedTime = currentTime;
        }
    }
}
