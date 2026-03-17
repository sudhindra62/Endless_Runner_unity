
using EndlessRunner.Managers;
using EndlessRunner.Monetization;
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
        public Button reviveButton;

        private void Start()
        {
            gameOverScreen.SetActive(false);
            restartButton.onClick.AddListener(RestartGame);
            reviveButton.onClick.AddListener(RevivePlayer);
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

        private void RevivePlayer()
        {
            AdManager.Instance.ShowRewardedAd(RewardType.Revive, (rewardType) =>
            {
                if (rewardType == RewardType.Revive)
                {
                    GameManager.Instance.SetState(GameManager.GameState.Playing);
                    gameOverScreen.SetActive(false);
                }
            });
        }
    }
}
