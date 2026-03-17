
using EndlessRunner.Themes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EndlessRunner.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

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

        private void Start()
        {
            // Start the game in the MainMenu state
            SetState(GameState.MainMenu);
        }

        public void StartGame()
        {
            // This will be called by the PreRunCinematicManager
            SetState(GameState.Playing);
        }

        public void PauseGame(bool isPaused)
        {
            SetState(isPaused ? GameState.Paused : GameState.Playing);
        }

        public void EndGame()
        {
            SetState(GameState.GameOver);
        }

        public void SetState(GameState newState)
        {
            currentState = newState;
            switch (currentState)
            {
                case GameState.MainMenu:
                    Time.timeScale = 1f;
                    if (UIManager.Instance != null) UIManager.Instance.ShowHomeScreen();
                    break;
                case GameState.Playing:
                    Time.timeScale = 1f;
                    if (UIManager.Instance != null) UIManager.Instance.ShowGameScreen();
                    break;
                case GameState.Paused:
                    Time.timeScale = 0f;
                    if (UIManager.Instance != null) UIManager.Instance.ShowPauseScreen();
                    break;
                case GameState.GameOver:
                    Time.timeScale = 0f;
                    // Logic for a game over screen would go here
                    break;
            }
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
