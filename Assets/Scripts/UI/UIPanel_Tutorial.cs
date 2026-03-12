
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages the display of the tutorial UI elements, including instruction text and panel visibility.
/// This script is controlled by the UIManager to ensure a centralized UI management strategy.
/// Created and fully integrated by Supreme Guardian Architect v13.
/// </summary>
public class UIPanel_Tutorial : MonoBehaviour
{
    [Header("Component References")]
    [Tooltip("The Text component used to display tutorial instructions.")]
    [SerializeField] private Text instructionText;

    [Tooltip("The main container for the tutorial UI elements, used to show/hide the entire panel.")]
    [SerializeField] private GameObject panelContainer;

    private Coroutine _hideCoroutine;

    void Awake()
    {
        // Default to self if container is not assigned in inspector
        if (panelContainer == null)
        {
            panelContainer = this.gameObject;
        }
        // Start with the panel hidden
        panelContainer.SetActive(false);
    }

    /// <summary>
    /// Shows the tutorial panel with a specific instruction message.
    /// An optional duration can be provided to automatically hide the panel after a delay.
    /// </summary>
    /// <param name="message">The instruction text to display.</param>
    /// <param name="duration">The time in seconds before the panel automatically hides. If 0, it remains visible.</param>
    public void Show(string message, float duration)
    {
        if (instructionText != null)
        {
            instructionText.text = message;
        }
        
        panelContainer.SetActive(true);

        // If a hide coroutine is already running, stop it to handle the new message.
        if (_hideCoroutine != null)
        {
            StopCoroutine(_hideCoroutine);
        }

        // If a duration is provided, start a new coroutine to hide the panel after the specified time.
        if (duration > 0)
        {
            _hideCoroutine = StartCoroutine(HideAfterDelay(duration));
        }
    }

    /// <summary>
    /// Immediately hides the tutorial panel.
    /// </summary>
    public void Hide()
    {
        if (_hideCoroutine != null)
        {
            StopCoroutine(_hideCoroutine);
            _hideCoroutine = null;
        }
        panelContainer.SetActive(false);
    }

    /// <summary>
    /// A coroutine that waits for a specified delay and then hides the panel.
    /// </summary>
    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Hide();
        _hideCoroutine = null;
    }
}
