using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event Action OnGamePaused;
    public static event Action OnGameResumed;
    public static event Action OnGameOver;
    public int GetCurrentScore() => 0;
public int GetBestScore() => 0;

public void PauseGame() => OnGamePaused?.Invoke();
public void ResumeGame() => OnGameResumed?.Invoke();
public void RestartGame() => OnGameOver?.Invoke();


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

    // Compatibility triggers (no behavior change)
    public void TriggerPause() => OnGamePaused?.Invoke();
    public void TriggerResume() => OnGameResumed?.Invoke();
    public void TriggerGameOver() => OnGameOver?.Invoke();
}
