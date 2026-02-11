using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText, bestScoreText, timerText, bestTimeText, comboText;
    public GameObject restartButton;

    public int survivalScoreRate = 5;

    int score, bestScore, combo = 1;
    float timer, bestTime;
    bool isGameOver;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestTime = PlayerPrefs.GetFloat("BestTime", 0);
        ResetRun();
    }

    void Update()
    {
        if (isGameOver) return;

        timer += Time.deltaTime;
        score += Mathf.RoundToInt(Time.deltaTime * survivalScoreRate * combo);
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount * combo;
        UpdateUI();
    }

    public void GameOver()
    {
        isGameOver = true;
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
    }

    public void RestartGame()
    {
        if (Time.timeScale == 0f)
            Time.timeScale = 1f;

        ResetRun();

        // 🔒 SAFE REPLACEMENTS (same behavior, non-obsolete)
        FindFirstObjectByType<PlayerController>().ResetPlayer();
        FindFirstObjectByType<TileSpawner>().ResetTiles();
        FindFirstObjectByType<ObstacleSpawner>().ResetSpawner();

        restartButton.SetActive(false);
    }

    void ResetRun()
    {
        score = 0;
        timer = 0;
        combo = 1;
        isGameOver = false;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (bestScoreText != null)
            bestScoreText.text = "Best: " + bestScore;

        if (timerText != null)
            timerText.text = "Time: " + Mathf.FloorToInt(timer);

        if (bestTimeText != null)
            bestTimeText.text = "Best Time: " + Mathf.FloorToInt(bestTime);

        if (comboText != null)
            comboText.text = "x" + combo;
    }
}
