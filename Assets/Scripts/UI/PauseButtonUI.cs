
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A simple UI component that triggers the pause functionality.
/// It finds the GameFlowController via the ServiceLocator and calls its PauseGame method.
/// </summary>
[RequireComponent(typeof(Button))]
public class PauseButtonUI : MonoBehaviour
{
    private Button pauseButton;
    private GameFlowController gameFlowController;

    private void Awake()
    {
        pauseButton = GetComponent<Button>();
    }

    private void Start()
    {
        // Resolve dependency using the ServiceLocator.
        gameFlowController = ServiceLocator.Get<GameFlowController>();
        if (gameFlowController == null)
        {
            Debug.LogError("GameFlowController not found in the scene!");
            // Disable the button if the controller is missing to prevent errors.
            pauseButton.interactable = false;
        }
    }

    private void OnEnable()
    {
        pauseButton.onClick.AddListener(OnPauseClicked);
    }

    private void OnDisable()
    {
        pauseButton.onClick.RemoveListener(OnPauseClicked);
    }

    private void OnPauseClicked()
    {
        // It's crucial that this button doesn't handle the pause logic itself.
        // It only sends a request to the central controller.
        gameFlowController?.PauseGame();
    }
}
