
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private void Start()
    {
        // --- PERSISTENCE HOOK: Check if the tutorial has been completed before. ---
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

    /// <summary>
    /// Begins the tutorial sequence.
    /// </summary>
    public void StartTutorial()
    {
        if (tutorialSteps == null || tutorialSteps.Count == 0) return;

        Debug.Log("Guardian Architect Log: Tutorial starting.");
        isTutorialActive = true;
        currentStepIndex = 0;
        StartCoroutine(TutorialSequence());
    }

    private IEnumerator TutorialSequence()
    {
        // Wait for the game to start
        yield return new WaitUntil(() => GameManager.Instance.GetCurrentState() == GameManager.GameState.Playing);

        while(currentStepIndex < tutorialSteps.Count)
        {
            TutorialStep currentStep = tutorialSteps[currentStepIndex];
            
            // --- A-TO-Z CONNECTIVITY: Display the tutorial message via the UIManager. ---
            UIManager.Instance.ShowTutorialMessage(currentStep.instructionText, currentStep.duration);
            
            // Wait for the player to perform the correct action
            yield return new WaitUntil(() => WasActionPerformed(currentStep.requiredAction));

            // Hide the message and wait before the next step
            UIManager.Instance.HideTutorialMessage();
            yield return new WaitForSeconds(timeBetweenSteps);
            
            currentStepIndex++;
        }

        EndTutorial();
    }

    private bool WasActionPerformed(TutorialAction action)
    {
        // This is a simplified check. A real implementation would listen to events from InputManager.
        // For now, we will assume the player performs the action within the message duration.
        // NOTE: This will be fortified later with a proper event-based check.
        return true; 
    }

    /// <summary>
    /// Concludes the tutorial and saves the player's progress.
    /// </summary>
    private void EndTutorial()
    {
        Debug.Log("Guardian Architect Log: Tutorial complete.");
        isTutorialActive = false;
        
        // --- PERSISTENCE HOOK: Mark tutorial as completed and save data. ---
        PlayerData data = SaveManager.Instance.GetPlayerData();
        data.tutorialCompleted = true;
        SaveManager.Instance.SavePlayerData(data);
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
