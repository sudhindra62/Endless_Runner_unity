
using EndlessRunner.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EndlessRunner.System
{
    public class SceneLoader : MonoBehaviour
    {
        private void Awake()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameStateManager.GameState newState)
        {
            switch (newState)
            {
                case GameStateManager.GameState.MainMenu:
                    SceneManager.LoadScene("MainMenu");
                    break;
                case GameStateManager.GameState.Playing:
                    SceneManager.LoadScene("Game");
                    break;
            }
        }
    }
}
