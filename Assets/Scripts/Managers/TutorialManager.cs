
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [SerializeField] private TutorialUI tutorialUIPrefab;
    [SerializeField] private TutorialStep[] tutorialSteps;

    private int currentStep = 0;
    private TutorialUI tutorialUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 0)
        {
            if (tutorialUIPrefab != null)
            {
                tutorialUI = Instantiate(tutorialUIPrefab);
                StartTutorial();
            }
        }
    }

    public void StartTutorial()
    {
        currentStep = 0;
        if (tutorialUI != null)
        {
            ShowCurrentStep();
        }
    }

    public void OnContinueClicked()
    {
        currentStep++;
        if (currentStep < tutorialSteps.Length)
        {
            if (tutorialUI != null)
            {
                ShowCurrentStep();
            }
        }
        else
        {
            EndTutorial();
        }
    }

    private void ShowCurrentStep()
    {
        tutorialUI.ShowTutorialStep(tutorialSteps[currentStep]);
    }

    private void EndTutorial()
    {
        if (tutorialUI != null)
        {
            tutorialUI.HideTutorial();
        }
        PlayerPrefs.SetInt("TutorialCompleted", 1);
    }
}
