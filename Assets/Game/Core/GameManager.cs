using UnityEngine;

/// <summary>
/// Manages the overall game state, including the main menu, gameplay, and game-over states.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { MainMenu, Gameplay, GameOver }
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
        ChangeState(GameState.MainMenu);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case GameState.MainMenu:
                // Handle main menu logic
                break;
            case GameState.Gameplay:
                // Handle gameplay logic
                break;
            case GameState.GameOver:
                // Handle game over logic
                break;
        }
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }
}
