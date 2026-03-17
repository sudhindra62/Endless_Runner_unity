using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score;
    public float gameSpeed = 1f;

    [Header("Difficulty Scaling")]
    [SerializeField] private float _speedIncreaseRate = 0.01f;
    [SerializeField] private float _difficultyIncreaseInterval = 30f;

    private float _timeSinceLastDifficultyIncrease = 0f;

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

    private void Update()
    {
        // Increase game speed over time
        gameSpeed += _speedIncreaseRate * Time.deltaTime;

        // Increase pattern complexity over time
        _timeSinceLastDifficultyIncrease += Time.deltaTime;
        if (_timeSinceLastDifficultyIncrease >= _difficultyIncreaseInterval)
        {   
            if (GameStateManager.Instance != null) 
            {
                GameStateManager.Instance.IncreaseDifficulty();
            }
            _timeSinceLastDifficultyIncrease = 0f;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScore(score);
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over! Your score: " + score);
        Time.timeScale = 0f; // Pause the game
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOverPanel();
        }
    }

    public void RestartGame()
    {
        // Reset static states to ensure a clean restart
        Time.timeScale = 1f;
        score = 0;
        gameSpeed = 1f; // Reset game speed

        // The new architecture no longer requires manual destruction/re-creation of managers.
        // Reloading the scene will handle re-initialization correctly.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
