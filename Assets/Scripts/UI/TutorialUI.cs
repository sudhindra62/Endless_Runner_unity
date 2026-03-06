
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        continueButton.onClick.AddListener(TutorialManager.Instance.OnContinueClicked);
    }

    public void ShowTutorialStep(TutorialStep step)
    {
        tutorialPanel.SetActive(true);
        tutorialText.text = step.message;
        // Logic to highlight UI elements would go here
    }

    public void HideTutorial()
    {
        tutorialPanel.SetActive(false);
    }
}
