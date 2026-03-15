
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EndlessRunner.Managers;

namespace EndlessRunner.UI
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI instructionText;
        [SerializeField] private GameObject tutorialPanel;

        private void Start()
        {
            // The UIManager will be responsible for showing/hiding this panel.
            // So we start with it hidden.
            tutorialPanel.SetActive(false);
        }

        public void ShowTutorialStep(string message)
        {
            instructionText.text = message;
            tutorialPanel.SetActive(true);
        }

        public void HideTutorialUI()
        {
            tutorialPanel.SetActive(false);
        }
    }
}
