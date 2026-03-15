
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EndlessRunner.Data;

namespace EndlessRunner.UI
{
    /// <summary>
    /// Manages the visual presentation of the tutorial steps.
    /// </summary>
    public class TutorialUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private TextMeshProUGUI instructionText;
        [SerializeField] private Image instructionImage; // For swipe icons, etc.
        [SerializeField] private Button continueButton;

        private void Start()
        {
            Hide();
        }

        public void ShowStep(TutorialStep step, System.Action onContinuePressed)
        {
            if (tutorialPanel == null) return;

            instructionText.text = step.instructionText;

            if (instructionImage != null)
            {
                if (step.instructionSprite != null)
                {
                    instructionImage.sprite = step.instructionSprite;
                    instructionImage.gameObject.SetActive(true);
                }
                else
                {
                    instructionImage.gameObject.SetActive(false);
                }
            }

            if (continueButton != null)
            {
                continueButton.onClick.RemoveAllListeners();
                if (step.waitForButtonPress)
                {
                    continueButton.gameObject.SetActive(true);
                    continueButton.onClick.AddListener(() => onContinuePressed?.Invoke());
                }
                else
                {
                    continueButton.gameObject.SetActive(false);
                }
            }

            tutorialPanel.SetActive(true);
        }

        public void Hide()
        {
            if (tutorialPanel != null)
            {
                tutorialPanel.SetActive(false);
            }
        }
    }
}
