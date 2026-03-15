
using EndlessRunner.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EndlessRunner.UI
{
    public class GameOverUI : MonoBehaviour
    {
        public GameObject gameOverScreen;
        public Text scoreText;
        public Button restartButton;

        private void Start()
        {
            gameOverScreen.SetActive(false);
            restartButton.onClick.AddListener(RestartGame);
        }

        public void ShowGameOverScreen(int score)
        {
            gameOverScreen.SetActive(true);
            scoreText.text = "Score: " + score;
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
