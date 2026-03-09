
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    // Assuming a GameManager and UIManager exist
    // [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        if (playButton != null) playButton.onClick.AddListener(StartGame);
        if (settingsButton != null) settingsButton.onClick.AddListener(ShowSettings);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        // GameManager.Instance.StartGame();
    }

    private void ShowSettings()
    {
        // UIManager.Instance.ShowSettings();
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
