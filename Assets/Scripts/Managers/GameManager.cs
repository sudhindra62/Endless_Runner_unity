
using UnityEngine;
using UnityEngine.SceneManagement;
using EndlessRunner.Themes;

namespace EndlessRunner.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public enum GameState { MainMenu, Playing, Paused, GameOver }
        private GameState currentState;
        public ThemeManager themeManager; 

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

            themeManager = FindObjectOfType<ThemeManager>();
        }

        private void Start()
        {
            if (themeManager != null)
            {
                themeManager.SetTheme(0);
            }
            SetState(GameState.Playing);
        }

        public void SetState(GameState newState)
        {
            currentState = newState;
            switch (currentState)
            {
                case GameState.MainMenu:
                    // Handle main menu logic
                    break;
                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;
                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;
                case GameState.GameOver:
                    Time.timeScale = 0f;
                    // Handle game over logic (e.g., show game over screen)
                    break;
            }
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
