
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EndlessRunner.Data;
using EndlessRunner.Core;
using EndlessRunner.Managers;

/// <summary>
/// Manages the step-by-step tutorial sequence for new players.
/// This system is now fully integrated with the UIManager to provide visual cues.
/// Logic restored and connected by Supreme Guardian Architect v13.
/// </summary>
public class TutorialManager : Singleton<TutorialManager>
{
    [Header("Tutorial Configuration")]
    [SerializeField] private List<TutorialStep> tutorialSteps;
    [SerializeField] private float timeBetweenSteps = 1.0f;

    private int _currentStepIndex = 0;
    private bool _isTutorialActive = false;
    private bool _actionWasPerformed = false;

    // --- UNITY LIFECYCLE & EVENT SUBSCRIPTION ---

    protected override void Awake()
    {
        base.Awake();
        // The TutorialManager's activation is now controlled by a higher authority (e.g., HomeSceneSetup).
        // This Awake method remains for the Singleton pattern, but does not self-disable.
    }

    private void OnEnable()
    {
        // Subscribe to input events when the manager is active.
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnSwipe += OnSwipe;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks when the manager is inactive or destroyed.
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnSwipe -= OnSwipe;
        }
    }

    // --- PUBLIC API ---

    /// <summary>
    /// Begins the tutorial sequence. This is the primary entry point.
    /// It's assumed that the controlling entity has already checked if the tutorial should run.
    /// </summary>
    public void StartTutorial()
    {
        if (tutorialSteps == null || tutorialSteps.Count == 0)
        {
            Debug.LogWarning("Guardian Architect Warning: TutorialManager has no tutorial steps assigned.");
            return;
        }

        if (_isTutorialActive) return; // Prevent starting if already running

        _isTutorialActive = true;
        _currentStepIndex = 0;
        Debug.Log("Guardian Architect: Starting Tutorial Sequence.");
        StartCoroutine(TutorialSequenceCoroutine());
    }

    // --- COROUTINE & SEQUENCE LOGIC ---

    /// <summary>
    /// The main coroutine that drives the tutorial step-by-step.
    /// </summary>
    private IEnumerator TutorialSequenceCoroutine()
    {
        // Wait until the game is in the 'Playing' state before starting the first step.
        yield return new WaitUntil(() => GameManager.Instance != null && GameManager.Instance.CurrentState == GameManager.GameState.Playing);
        
        Debug.Log("Guardian Architect: Game is in Playing state. Beginning tutorial steps.");

        while(_currentStepIndex < tutorialSteps.Count)
        {
            _actionWasPerformed = false;
            TutorialStep currentStep = tutorialSteps[_currentStepIndex];
            
            // DELEGATION: Command the UIManager to display the instruction.
            UIManager.Instance.ShowTutorialMessage(currentStep.instructionText, currentStep.duration);
            
            // Wait until the required action is performed (or if no action is needed).
            if (currentStep.requiredAction != TutorialAction.None)
            {
                yield return new WaitUntil(() => _actionWasPerformed);
            }
            else
            {
                // If no action is required, just wait for the specified duration.
                yield return new WaitForSeconds(currentStep.duration);
            }

            // DELEGATION: Command the UIManager to hide the message.
            UIManager.Instance.HideTutorialMessage();
            yield return new WaitForSeconds(timeBetweenSteps);
            
            _currentStepIndex++;
        }

        EndTutorial();
    }

    /// <summary>
    /// Finalizes the tutorial, marks it as complete in the save data, and deactivates itself.
    /// </summary>
    private void EndTutorial()
    {
        _isTutorialActive = false;
        
        Debug.Log("Guardian Architect: Tutorial complete. Saving progress.");
        // PERSISTENCE: Mark tutorial as completed in the player's save data.
        if (SaveManager.Instance != null)
        {
            SaveData data = SaveManager.Instance.LoadData(); // Use the appropriate method from your SaveManager
            data.TutorialCompleted = true;
            SaveManager.Instance.SaveData(data);
        }

        // The manager has fulfilled its purpose and can be disabled.
        gameObject.SetActive(false);
    }
    
    // --- INPUT HANDLING ---

    /// <summary>
    /// Listens for swipe input and checks if it matches the current tutorial step's requirement.
    /// </summary>
    private void OnSwipe(SwipeDirection direction)
    {
        if (!_isTutorialActive) return;

        TutorialStep currentStep = tutorialSteps[_currentStepIndex];
        if (currentStep.requiredAction == TutorialAction.SwipeLeft && direction == SwipeDirection.Left) _actionWasPerformed = true;
        if (currentStep.requiredAction == TutorialAction.SwipeRight && direction == SwipeDirection.Right) _actionWasPerformed = true;
        if (currentStep.requiredAction == TutorialAction.Jump && direction == SwipeDirection.Up) _actionWasPerformed = true;
        if (currentStep.requiredAction == TutorialAction.Slide && direction == SwipeDirection.Down) _actionWasPerformed = true;
    }
}
