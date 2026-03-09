
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core;
using Managers;

/// <summary>
/// Manages the step-by-step tutorial sequence for new players.
/// This system is now fully integrated with the UIManager to provide visual cues.
/// Logic restored and connected by Supreme Guardian Architect v12.
/// </summary>
public class TutorialManager : Singleton<TutorialManager>
{
    [Header("Tutorial Configuration")]
    [SerializeField] private List<TutorialStep> tutorialSteps;
    [SerializeField] private float timeBetweenSteps = 1.5f;

    private int currentStepIndex = 0;
    private bool isTutorialActive = false;
    private bool actionWasPerformed = false;

    protected override void Awake()
    {
        base.Awake();
        InputManager.Instance.OnSwipe += OnSwipe;
    }

    private void OnDestroy()
    {
        if(InputManager.Instance != null)
        {
            InputManager.Instance.OnSwipe -= OnSwipe;
        }
    }

    private void OnSwipe(SwipeDirection direction)
    {
        if (!isTutorialActive) return;

        TutorialStep currentStep = tutorialSteps[currentStepIndex];
        if (currentStep.requiredAction == TutorialAction.SwipeLeft && direction == SwipeDirection.Left) actionWasPerformed = true;
        if (currentStep.requiredAction == TutorialAction.SwipeRight && direction == SwipeDirection.Right) actionWasPerformed = true;
        if (currentStep.requiredAction == TutorialAction.SwipeUp && direction == SwipeDirection.Up) actionWasPerformed = true;
        if (currentStep.requiredAction == TutorialAction.SwipeDown && direction == SwipeDirection.Down) actionWasPerformed = true;
    }

    private void Start()
    {
        if (SaveManager.Instance.GetPlayerData().tutorialCompleted)
        {
            isTutorialActive = false;
            gameObject.SetActive(false);
        } 
        else
        {
            StartTutorial();
        }
    }

    public void StartTutorial()
    {
        if (tutorialSteps == null || tutorialSteps.Count == 0) return;

        isTutorialActive = true;
        currentStepIndex = 0;
        StartCoroutine(TutorialSequence());
    }

    private IEnumerator TutorialSequence()
    {
        yield return new WaitUntil(() => GameManager.Instance.GetCurrentState() == GameManager.GameState.Playing);

        while(currentStepIndex < tutorialSteps.Count)
        {
            actionWasPerformed = false;
            TutorialStep currentStep = tutorialSteps[currentStepIndex];
            
            UIManager.Instance.ShowTutorialMessage(currentStep.instructionText, currentStep.duration);
            
            yield return new WaitUntil(() => actionWasPerformed);

            UIManager.Instance.HideTutorialMessage();
            yield return new WaitForSeconds(timeBetweenSteps);
            
            currentStepIndex++;
        }

        EndTutorial();
    }

    private void EndTutorial()
    {
        isTutorialActive = false;
        
        PlayerData data = SaveManager.Instance.GetPlayerData();
        data.tutorialCompleted = true;
        SaveManager.Instance.SavePlayerData(data);
        gameObject.SetActive(false);
    }
}

[System.Serializable]
public class TutorialStep
{
    [TextArea(3, 5)]
    public string instructionText;
    public float duration = 3f;
    public TutorialAction requiredAction;
}

public enum TutorialAction
{
    None, 
    SwipeLeft,
    SwipeRight,
    SwipeUp,
    SwipeDown
}
