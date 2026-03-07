
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI instructionText;

    private void OnEnable()
    {
        TutorialManager.OnTutorialStepStart += ShowTutorialStep;
        TutorialManager.OnTutorialStepComplete += HideTutorial;
        TutorialManager.OnTutorialComplete += HideTutorial;
    }

    private void OnDisable()
    {
        TutorialManager.OnTutorialStepStart -= ShowTutorialStep;
        TutorialManager.OnTutorialStepComplete -= HideTutorial;
        TutorialManager.OnTutorialComplete -= HideTutorial;
    }

    private void Start()
    {
        // Start with the panel hidden
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
    }

    private void ShowTutorialStep(string instruction)
    {
        if (tutorialPanel != null && instructionText != null)
        {
            instructionText.text = instruction;
            tutorialPanel.SetActive(true);
        }
    }

    private void HideTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
    }
}
