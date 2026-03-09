
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Guides new players through core mechanics in a scripted, sequential manner.
/// Reconstructed and fortified by Supreme Guardian Architect v12 to align with project architecture.
/// This system now correctly interfaces with the InputManager, ensuring a flawless onboarding sequence.
/// </summary>
public class TutorialManager : Singleton<TutorialManager>
{
    public static event Action<string> OnTutorialStepStart; // string: prompt text
    public static event Action OnTutorialStepComplete;
    public static event Action OnTutorialComplete;

    [Tooltip("The sequence of steps for the interactive tutorial.")]
    [SerializeField]
    private List<TutorialStep> tutorialSteps;
    private int currentStepIndex = 0;
    private bool isTutorialActive = false;

    /// <summary>
    /// Starts the tutorial sequence. Intended to be called by the GameManager for a new player's first run.
    /// </summary>
    public void StartTutorial()
    {
        if (tutorialSteps == null || tutorialSteps.Count == 0)
        {
            Debug.LogWarning("Guardian Architect Warning: No tutorial steps have been assigned. Completing tutorial immediately.");
            CompleteTutorial();
            return;
        }

        isTutorialActive = true;
        currentStepIndex = 0;
        StartCoroutine(TutorialSequence());
    }

    private IEnumerator TutorialSequence()
    {
        // Globally disable player input to ensure a controlled, scripted sequence.
        if (InputManager.Instance != null) InputManager.Instance.SetInputEnabled(false);

        while (currentStepIndex < tutorialSteps.Count)
        {
            TutorialStep currentStep = tutorialSteps[currentStepIndex];
            
            // --- A-TO-Z CONNECTIVITY: Fire event for a UI listener to display the instruction prompt. ---
            OnTutorialStepStart?.Invoke(currentStep.instruction);
            
            // Wait until the player performs the correct, specified action.
            yield return StartCoroutine(WaitForStepCompletion(currentStep));
            
            // --- A-TO-Z CONNECTIVITY: Fire event for a UI listener to hide the instruction prompt. ---
            OnTutorialStepComplete?.Invoke();
            
            currentStepIndex++;
            yield return new WaitForSeconds(0.5f); // A brief, controlled pause between steps for better pacing.
        }

        CompleteTutorial();
    }

    private IEnumerator WaitForStepCompletion(TutorialStep step)
    {
        bool stepCompleted = false;
        Action<SwipeDirection> swipeHandler = null;

        // --- DEPENDENCY_FIX: Temporarily enable the specific input required for the current tutorial step. ---
        if (InputManager.Instance != null) InputManager.Instance.SetInputEnabled(true, step.requiredAction);

        // --- LOGIC_RESTORATION: Correctly handle all swipe-based actions through the single OnSwipe event. ---
        switch (step.requiredAction)
        {
            case TutorialAction.SwipeLeft:
                swipeHandler = (dir) => { if (dir == SwipeDirection.Left) stepCompleted = true; };
                break;
            case TutorialAction.SwipeRight:
                swipeHandler = (dir) => { if (dir == SwipeDirection.Right) stepCompleted = true; };
                break;
            case TutorialAction.Jump: // Swipe Up
                swipeHandler = (dir) => { if (dir == SwipeDirection.Up) stepCompleted = true; };
                break;
            case TutorialAction.Slide: // Swipe Down
                swipeHandler = (dir) => { if (dir == SwipeDirection.Down) stepCompleted = true; };
                break;
            case TutorialAction.None: // For timed pauses or non-interactive sequences.
                stepCompleted = true;
                break;
        }

        if (swipeHandler != null)
        {
            InputManager.OnSwipe += swipeHandler;
        }

        // Wait here until the correct action is performed.
        yield return new WaitUntil(() => stepCompleted);

        // --- A-TO-Z CONNECTIVITY: Immediately disable input and clean up listeners to prevent misfires. ---
        if (InputManager.Instance != null) InputManager.Instance.SetInputEnabled(false);
        if (swipeHandler != null) InputManager.OnSwipe -= swipeHandler;
    }

    private void CompleteTutorial()
    {
        isTutorialActive = false;
        Debug.Log("Guardian Architect: Tutorial Complete! Full player input is now enabled.");
        OnTutorialComplete?.Invoke();

        // Globally re-enable all player input.
        if (InputManager.Instance != null) InputManager.Instance.SetInputEnabled(true);

        // --- PERSISTENCE_HOOK: In a full implementation, this status would be saved to the player's profile. ---
        // Example: SaveManager.Instance.SavePlayerData("tutorialCompleted", true);
    }

    /// <summary>
    /// Public accessor to check if the tutorial is currently running.
    /// </summary>
    public bool IsTutorialActive()
    {
        return isTutorialActive;
    }
}

/// <summary>
/// Defines a single step in the tutorial sequence, including the instruction text and the required player action.
/// </summary>
[System.Serializable]
public class TutorialStep
{
    [TextArea(3, 5)]
    [Tooltip("The instructional text to display to the player for this step.")]
    public string instruction;

    [Tooltip("The specific action the player must perform to complete this step.")]
    public TutorialAction requiredAction;
}
