using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;

    public void Pause()
    {
        Time.timeScale = 0f;
        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }
}
