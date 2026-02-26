using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // ADDED

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
    public int survivalScoreRate = 5;

    private int score;
    private int bestScore;
    private int combo = 1;

    private float timer;
    private float bestTime;
    private float scoreFromDistance;

    private bool isGameOver;

    private PlayerController playerController;

private void Awake()
{
    if (Instance == null)
    {
        Instance = this;

        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestTime = PlayerPrefs.GetFloat("BestTime", 0);
    }
    else
    {
        Destroy(gameObject);
    }
}
    // ADDED: Auto rebind UI after scene loads
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindUIReferences();
        UpdateUI();
    }

    // ADDED: Safe rebind method
  // MODIFIED SAFE REBIND (SPECIFIC BY NAME)
private void RebindUIReferences()
{
    Canvas activeCanvas = FindFirstObjectByType<Canvas>();

    if (activeCanvas == null)
        return;

    var texts = activeCanvas.GetComponentsInChildren<TextMeshProUGUI>(true);

    foreach (var txt in texts)
    {
        if (txt.name == "CoinText")
            scoreText = txt;
        else if (txt.name == "BestScoreText")
            bestScoreText = txt;
        else if (txt.name == "TimerText")
            timerText = txt;
        else if (txt.name == "BestTimeText")
            bestTimeText = txt;
        else if (txt.name == "ComboText")
            comboText = txt;
    }

    if (restartButton == null)
    {
        var btn = activeCanvas.transform.Find("RestartButton");
        if (btn != null)
            restartButton = btn.gameObject;
    }
}


    private void Start()
    {
        UpdateUI();
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

    Debug.Log("SCORE VARIABLE NOW: " + score);

    UpdateUI();   // MUST be this
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
        //Temporarily added line
        Debug.Log("RESET SCORE CALLED");

    }

    public void ResetScore()
    {
        score = 0;
        timer = 0f;
        combo = 1;
        scoreFromDistance = 0f;
             // Temporarily added line
        Debug.Log("RESET SCORE CALLED");


        UpdateUI();
    }

    public int GetCurrentScore() => score;

private void UpdateUI()
{
    // SCORE (Top Left)
    if (scoreText != null)
        scoreText.text = $"SCORE: {score}";

    // BEST SCORE (Small Below)
    if (bestScoreText != null)
        bestScoreText.text = $"BEST SCORE: {bestScore}";

    // TIMER (Top Right)
    if (timerText != null)
    {
        int minutes = Mathf.FloorToInt(timer) / 60;
        int seconds = Mathf.FloorToInt(timer) % 60;
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    // BEST TIME (Small Below)
    if (bestTimeText != null)
    {
        int bestMinutes = Mathf.FloorToInt(bestTime) / 60;
        int bestSeconds = Mathf.FloorToInt(bestTime) % 60;
        bestTimeText.text = $"BEST TIME: {bestMinutes:00}:{bestSeconds:00}";
    }
}
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int currentTime = Mathf.FloorToInt(timer);
            int minutes = currentTime / 60;
            int seconds = currentTime % 60;
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public int GetScore()
    {
        return score;
    }
}
