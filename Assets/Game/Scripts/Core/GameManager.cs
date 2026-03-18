using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score = 0;
    public int lives = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void LoseLife()
    {
        lives--;
        if (lives <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        // TODO: Implement game over logic
        Debug.Log("Game Over!");
    }
}
