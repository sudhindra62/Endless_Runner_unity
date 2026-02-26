using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Scoring Settings")]
    public float distanceScoreMultiplier = 1f;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;

    private int score;
    private float distanceTraveled;
    private Vector3 lastPosition;

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

    private void Start()
    {
        if (PlayerController.Instance != null)
        {
            lastPosition = PlayerController.Instance.transform.position;
        }
        UpdateScoreUI();
    }

    private void Update()
    {
        if (PlayerController.Instance != null && !PlayerController.Instance.IsDead)
        {
            float distance = Vector3.Distance(PlayerController.Instance.transform.position, lastPosition);
            distanceTraveled += distance;
            lastPosition = PlayerController.Instance.transform.position;

            AddScore(Mathf.FloorToInt(distance * distanceScoreMultiplier));
        }
    }

    public void AddScore(int amount)
    {
        if (amount <= 0) return;

        int multiplier = 1;
        if (StyleManager.Instance != null)
        {
            multiplier = StyleManager.Instance.ScoreMultiplier;
        }

        score += amount * multiplier;
        UpdateScoreUI();
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        distanceTraveled = 0;
        if (PlayerController.Instance != null)
        {
            lastPosition = PlayerController.Instance.transform.position;
        }
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
