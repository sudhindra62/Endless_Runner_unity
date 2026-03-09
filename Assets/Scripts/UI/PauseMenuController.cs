
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    // Assuming a UIManager and GameManager exist
    // [SerializeField] private GameObject optionsPanel;

    private void Start()
    {
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
        if (optionsButton != null) optionsButton.onClick.AddListener(ShowOptions);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private void ShowOptions()
    {
        // UIManager.Instance.ShowOptions();
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        // GameManager.Instance.GoToMainMenu();
    }
}
