
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image tutorialImage;
    [SerializeField] private Button continueButton;

    private void Start()
    {
        continueButton.onClick.AddListener(TutorialManager.Instance.OnContinueClicked);
    }

    public void ShowTutorialStep(TutorialStep step)
    {
        titleText.text = step.title;
        descriptionText.text = step.description;
        tutorialImage.sprite = step.image;
        gameObject.SetActive(true);
    }

    public void HideTutorial()
    {
        gameObject.SetActive(false);
    }
}
