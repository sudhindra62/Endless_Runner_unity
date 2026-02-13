using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the in-game Pause button.
/// It handles player input to toggle the pause state and updates its own icon.
/// </summary>
public class PauseButtonUI : MonoBehaviour
{
    [Header("Icon Sprites")]
    [Tooltip("The icon to display when the game is playing (i.e., the Pause icon).")]
    [SerializeField] private Sprite pauseIcon;

    [Tooltip("The icon to display when the game is paused (i.e., the Play icon).")]
    [SerializeField] private Sprite playIcon;

    private Button pauseButton;
    private Image buttonImage;
    private bool isPaused = false;

    void Awake()
    {
        pauseButton = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        pauseButton.onClick.AddListener(TogglePause);
    }

    void Start()
    {
        // Set the initial icon to the pause symbol
        buttonImage.sprite = pauseIcon;
    }

    /// <summary>
    /// Called when the player clicks the pause button.
    /// </summary>
    private void TogglePause()
    {
        isPaused = !isPaused;

        // Notify the central notifier of the state change
        PlayerStatusNotifier.Instance.NotifyPauseState(isPaused);

        // Update the button's icon to reflect the new state
        buttonImage.sprite = isPaused ? playIcon : pauseIcon;
    }
}
