
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
        // Check if the tutorial has been completed before
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 0)
        {
            tutorialUI = Instantiate(tutorialUIPrefab);
            StartTutorial();
        }
    }

    public void StartTutorial()
    {
        currentStep = 0;
        ShowCurrentStep();
    }

    public void OnContinueClicked()
    {
        currentStep++;
        if (currentStep < tutorialSteps.Length)
        {
            ShowCurrentStep();
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
        tutorialUI.HideTutorial();
        PlayerPrefs.SetInt("TutorialCompleted", 1);
    }
}
