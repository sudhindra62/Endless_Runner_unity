
using System.Collections.Generic;
using UnityEngine;



    /// <summary>
    /// Manages the interactive tutorial sequence for the player's first run.
    /// </summary>
    public class TutorialManager : Singleton<TutorialManager>
    {
        [Header("Tutorial Configuration")]
        [SerializeField] private List<TutorialStep> tutorialSteps;
        [SerializeField] private TutorialUI tutorialUI;

        private int currentStepIndex = 0;
        private bool isTutorialActive = false;
        public bool IsTutorialActive => isTutorialActive;

        private void OnEnable()
        {
            GameEvents.OnGameStart += CheckTutorialStatus;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStart -= CheckTutorialStatus;
            UnsubscribeFromInput();
        }

        private void CheckTutorialStatus()
        {
            if (SaveManager.Instance != null && !SaveManager.Instance.Data.hasCompletedTutorial)
            {
                StartTutorial();
            }
        }

        public void StartTutorial()
        {
            if (tutorialUI == null || tutorialSteps.Count == 0) 
            {
                Debug.LogWarning("TUTORIAL_MANAGER: TutorialUI or steps not set. Aborting tutorial.");
                EndTutorial(false);
                return;
            }
            isTutorialActive = true;
            currentStepIndex = 0;
            Time.timeScale = 0; // Pause the game for the tutorial
            ShowCurrentStep();
        }

        private void ShowCurrentStep()
        {
            if(currentStepIndex >= tutorialSteps.Count) return;
            
            TutorialStep currentStep = tutorialSteps[currentStepIndex];
            tutorialUI.ShowStep(currentStepIndex, OnContinueButtonPressed);
            SubscribeToInput(currentStep.trigger);
        }

        private void AdvanceStep()
        {
            UnsubscribeFromInput();
            currentStepIndex++;
            if (currentStepIndex < tutorialSteps.Count)
            {
                ShowCurrentStep();
            }
            else
            {
                EndTutorial(true);
            }
        }

        private void EndTutorial(bool completed)
        {
            isTutorialActive = false;
            Time.timeScale = 1; // Resume game
            if(tutorialUI != null)
            {
                tutorialUI.Hide();
            }

            if (completed && SaveManager.Instance != null)
            {
                SaveManager.Instance.Data.hasCompletedTutorial = true;
                SaveManager.Instance.SaveGame();
                Debug.Log("TUTORIAL_MANAGER: Tutorial completed and saved.");
            }
        }

        private void SubscribeToInput(TutorialTrigger trigger)
        {
            if (InputManager.Instance == null) return;
            UnsubscribeFromInput(); // Ensure no multiple subscriptions

            switch (trigger)
            {
                case TutorialTrigger.SwipeLeft:
                    InputManager.Instance.OnLaneChange += HandleLaneChangeLeft;
                    break;
                case TutorialTrigger.SwipeRight:
                    InputManager.Instance.OnLaneChange += HandleLaneChangeRight;
                    break;
                case TutorialTrigger.SwipeUp:
                    InputManager.Instance.OnJump += HandleJump;
                    break;
            }
        }

        private void UnsubscribeFromInput()
        {
            if (InputManager.Instance == null) return;
            InputManager.Instance.OnLaneChange -= HandleLaneChangeLeft;
            InputManager.Instance.OnLaneChange -= HandleLaneChangeRight;
            InputManager.Instance.OnJump -= HandleJump;
        }

        private void HandleLaneChangeLeft(int direction) { if(direction == -1) AdvanceStep(); }
        private void HandleLaneChangeRight(int direction) { if(direction == 1) AdvanceStep(); }
        private void HandleJump() { AdvanceStep(); }
        private void OnContinueButtonPressed() { AdvanceStep(); }
    }


