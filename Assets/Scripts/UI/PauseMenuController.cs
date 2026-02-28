
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the pause menu UI. It is shown by the UIManager when the game state is Paused.
/// It provides buttons to resume the game or quit the run.
/// </summary>
public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        // Using lambda expressions for brevity, ensuring no duplicate listeners are added
        // since the buttons are configured once here.
        resumeButton.onClick.AddListener(() => GameFlowController.Instance.ResumeGame());
        quitButton.onClick.AddListener(() => GameFlowController.Instance.ReturnToMenu());
    }

    private void OnDestroy()
    {
        // Remove listeners to prevent memory leaks, although singletons might persist
        resumeButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
