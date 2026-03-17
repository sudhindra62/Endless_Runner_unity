
using EndlessRunner.UI;
using EndlessRunner.Themes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EndlessRunner.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public ExtraCoinsUI extraCoinsUI;

        public enum GameState { MainMenu, Playing, Paused, GameOver }
        private GameState currentState;

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

        public void StartGame()
        {
            SceneManager.LoadScene("GameScene"); // Or your main game scene name
            SetState(GameState.Playing);
        }

        public void SetState(GameState newState)
        {
            currentState = newState;
            switch (currentState)
            {
                case GameState.MainMenu:
                    Time.timeScale = 1f;
                    SceneManager.LoadScene("MainMenu");
                    break;
                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;
                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;
                case GameState.GameOver:
                    Time.timeScale = 0f;
                    if (extraCoinsUI != null) extraCoinsUI.ShowExtraCoinsScreen();
                    break;
            }
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
