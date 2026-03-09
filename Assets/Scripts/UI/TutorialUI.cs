
using UnityEngine;
using TMPro; // Using TextMeshPro for modern, scalable text.

/// <summary>
/// Manages the visual presentation of the tutorial prompts.
/// Listens to events from the TutorialManager and updates the UI accordingly.
/// Fortified and wired by the Supreme Guardian Architect v12.
/// </summary>
public class TutorialUI : MonoBehaviour
{
    [Header("UI Component References")]
    [Tooltip("The main parent object for the tutorial UI elements.")]
    [SerializeField] private GameObject tutorialPanel;

    [Tooltip("The TextMeshProUGUI component used to display the instruction text.")]
    [SerializeField] private TextMeshProUGUI instructionText;

    private void OnEnable()
    {
        // --- A-TO-Z CONNECTIVITY: Subscribe to the TutorialManager events. ---
        TutorialManager.OnTutorialStepStart += HandleTutorialStepStart;
        TutorialManager.OnTutorialStepComplete += HandleTutorialStepComplete;
        TutorialManager.OnTutorialComplete += HandleTutorialComplete;
    }

    private void OnDisable()
    {
        // --- DEPENDENCY_FIX: Unsubscribe to prevent memory leaks and dangling references. ---
        TutorialManager.OnTutorialStepStart -= HandleTutorialStepStart;
        TutorialManager.OnTutorialStepComplete -= HandleTutorialStepComplete;
        TutorialManager.OnTutorialComplete -= HandleTutorialComplete;
    }

    private void Start()
    {
        // Ensure the tutorial panel is hidden by default.
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Called when a new tutorial step begins. Displays the UI panel and sets the instruction text.
    /// </summary>
    /// <param name="prompt">The instruction text for the current step.</param>
    private void HandleTutorialStepStart(string prompt)
    {
        if (tutorialPanel == null || instructionText == null)
        {
            Debug.LogError("Guardian Architect Error: TutorialUI components are not assigned in the Inspector!");
            return;
        }
        
        instructionText.text = prompt;
        tutorialPanel.SetActive(true);
    }

    /// <summary>
    /// Called when the current tutorial step is successfully completed. Hides the UI panel.
    /// </summary>
    private void HandleTutorialStepComplete()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
    }
    
    /// <summary>
    /// Called when the entire tutorial sequence is finished. Hides the UI panel permanently.
    /// </summary>
    private void HandleTutorialComplete()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
        Debug.Log("Guardian Architect Log: Tutorial UI has been deactivated upon tutorial completion.");
    }
}
