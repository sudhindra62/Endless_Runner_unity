using UnityEngine;
using UnityEngine.UI;

public class PauseIconController : MonoBehaviour
{
    public GameObject pausePanel;
    public Image iconImage;
    public Sprite pauseIcon;
    public Sprite playIcon;

    bool paused;

    public void TogglePause()
    {
        paused = !paused;

        if (paused)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            iconImage.sprite = playIcon;
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            iconImage.sprite = pauseIcon;
        }
    }
}
