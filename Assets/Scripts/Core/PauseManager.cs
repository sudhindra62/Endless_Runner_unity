using UnityEngine;

/// <summary>
/// Unified PauseManager
/// Preserves:
/// - Singleton architecture
/// - HUD button integration
/// - Pause panel UI
/// - Time.timeScale control
/// - Safe event cleanup
/// </summary>
public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [Header("UI")]
    public GameObject pausePanel;

    public bool IsPaused { get; private set; }

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
        TryBindPauseButton();
    }

    private void TryBindPauseButton()
    {
        if (GameHUDController.Instance != null &&
            GameHUDController.Instance.PauseButton != null)
        {
            GameHUDController.Instance.PauseButton.onClick.AddListener(TogglePause);
        }
    }

    public void TogglePause()
    {
        if (IsPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        if (IsPaused) return;

        IsPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void Resume()
    {
        if (!IsPaused) return;

        IsPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (GameHUDController.Instance != null &&
            GameHUDController.Instance.PauseButton != null)
        {
            GameHUDController.Instance.PauseButton.onClick.RemoveListener(TogglePause);
        }
    }
}
