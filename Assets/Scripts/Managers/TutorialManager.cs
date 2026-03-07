
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Guides new players through core mechanics in a scripted, sequential manner.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v2 to fulfill the [PLANNED] feature mandate.
/// </summary>
public class TutorialManager : Singleton<TutorialManager>
{
    public static event Action<string> OnTutorialStepStart; // string: prompt text
    public static event Action OnTutorialStepComplete;
    public static event Action OnTutorialComplete;

    [SerializeField]
    private List<TutorialStep> tutorialSteps;
    private int currentStepIndex = 0;
    private bool isTutorialActive = false;

    // To be called by the GameManager when a new player starts their first run
    public void StartTutorial()
    {
        if (tutorialSteps == null || tutorialSteps.Count == 0)
        {
            Debug.LogWarning("TutorialManager: No tutorial steps have been assigned.");
            CompleteTutorial();
            return;
        }

        isTutorialActive = true;
        currentStepIndex = 0;
        StartCoroutine(TutorialSequence());
    }

    private IEnumerator TutorialSequence()
    {
        // Globally disable player input to ensure a controlled sequence
        if (InputManager.Instance != null) InputManager.Instance.SetInputEnabled(false);

        while (currentStepIndex < tutorialSteps.Count)
        {
            TutorialStep currentStep = tutorialSteps[currentStepIndex];
            
            // Fire event for UI to display instruction
            OnTutorialStepStart?.Invoke(currentStep.instruction);
            
            // Wait until the player performs the correct action
            yield return StartCoroutine(WaitForStepCompletion(currentStep));
            
            // Fire event for UI to hide instruction
            OnTutorialStepComplete?.Invoke();
            
            currentStepIndex++;
            yield return new WaitForSeconds(0.5f); // A brief pause between steps for better pacing
        }

        CompleteTutorial();
    }

    private IEnumerator WaitForStepCompletion(TutorialStep step)
    {
        bool stepCompleted = false;
        Action<SwipeDirection> swipeHandler = null;
        Action jumpHandler = null;

        // Temporarily enable specific input required for the current step
        if (InputManager.Instance != null) InputManager.Instance.SetInputEnabled(true, step.requiredAction);

        switch (step.requiredAction)
        {
            case TutorialAction.SwipeLeft:
                swipeHandler = (dir) => { if (dir == SwipeDirection.Left) stepCompleted = true; };
                InputManager.OnSwipe += swipeHandler;
                break;
            case TutorialAction.SwipeRight:
                swipeHandler = (dir) => { if (dir == SwipeDirection.Right) stepCompleted = true; };
                InputManager.OnSwipe += swipeHandler;
                break;
            case TutorialAction.Jump: // Typically swipe up
                jumpHandler = () => { stepCompleted = true; };
                InputManager.OnJump += jumpHandler;
                break;
            case TutorialAction.Slide: // Typically swipe down
                 swipeHandler = (dir) => { if (dir == SwipeDirection.Down) stepCompleted = true; };
                InputManager.OnSwipe += swipeHandler;
                break;
            case TutorialAction.None: // For timed pauses or animated sequences
                stepCompleted = true;
                break;
        }

        // Wait here until the correct action is performed
        yield return new WaitUntil(() => stepCompleted);

        // Immediately disable input again and clean up event listeners to prevent misfires
        if (InputManager.Instance != null) InputManager.Instance.SetInputEnabled(false);
        if (swipeHandler != null) InputManager.OnSwipe -= swipeHandler;
        if (jumpHandler != null) InputManager.OnJump -= jumpHandler;
    }

    private void CompleteTutorial()
    {
        isTutorialActive = false;
        Debug.Log("Tutorial Complete! Full player input is now enabled.");
        OnTutorialComplete?.Invoke();

        // Globally re-enable all player input
        if (InputManager.Instance != null) InputManager.Instance.SetInputEnabled(true);

        // In a full implementation, you would save this status to the player's profile
        // PlayerProfile.MarkTutorialAsCompleted();
    }

    public bool IsTutorialActive()
    {
        return isTutorialActive;
    }
}

[System.Serializable]
public class TutorialStep
{
    [TextArea(3, 5)]
    public string instruction;
    public TutorialAction requiredAction;
}

// Defines the specific actions the tutorial will wait for.
// Mirrors the actions available in the InputManager.
public enum TutorialAction
{
    None,
    SwipeLeft,
    SwipeRight,
    Jump,
    Slide
}
