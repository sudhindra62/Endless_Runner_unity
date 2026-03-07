using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public bool isGameOver = false;

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

    public void AddScore(int amount)
    {
        score += amount;
        // Update UI or other game elements with the new score
    }

    public void GameOver()
    {
        isGameOver = true;
        // You can add logic here to show a game over screen, etc.
        // For now, we'll just reload the scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
