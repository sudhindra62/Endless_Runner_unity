
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EndlessRunner.Data;
using System;

namespace EndlessRunner.UI
{
    public class TutorialUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private TextMeshProUGUI instructionText;
        [SerializeField] private Button continueButton;

        private Action onContinueCallback;

        private void Awake()
        {
            continueButton.onClick.AddListener(OnContinueClicked);
            Hide();
        }

        public void ShowStep(TutorialStep step, Action onContinue)
        {
            onContinueCallback = onContinue;
            instructionText.text = step.instructionText;

            // Only show the continue button if the step doesn't have a specific input trigger
            continueButton.gameObject.SetActive(step.trigger == TutorialTrigger.None);

            tutorialPanel.SetActive(true);
        }

        public void Hide()
        {
            tutorialPanel.SetActive(false);
        }

        private void OnContinueClicked()
        {
            onContinueCallback?.Invoke();
        }
    }
}
