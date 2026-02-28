
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

/// <summary>
/// Manages the run summary screen, displayed at the EndOfRun state.
/// It fetches the final run data from the GameFlowController and animates the final score.
/// It uses the GameFlowController to navigate to other states (Menu or Playing).
/// </summary>
public class RunSummaryUI : MonoBehaviour
{
    [Header("Text Components")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI coinsCollectedText;
    [SerializeField] private TextMeshProUGUI gemsCollectedText; // Added this field
    [SerializeField] private TextMeshProUGUI styleScoreText; // Added this field
    [SerializeField] private TextMeshProUGUI timeSurvivedText;
    [SerializeField] private TextMeshProUGUI reviveUsedText;

    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button homeButton;

    [Header("Animation Settings")]
    [SerializeField] private float scoreAnimationDuration = 1.5f;
    [SerializeField] private AnimationCurve scoreAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine animationCoroutine;
    private GameFlowController gameFlowController;
    private ScoreManager scoreManager;

    // Caching for performance
    private readonly StringBuilder stringBuilder = new StringBuilder(16);

    private void Start()
    {
        gameFlowController = ServiceLocator.Get<GameFlowController>();
        scoreManager = ServiceLocator.Get<ScoreManager>();

        restartButton.onClick.AddListener(OnRestart);
        homeButton.onClick.AddListener(OnHome);
    }

    private void OnDestroy()
    {
        restartButton.onClick.RemoveListener(OnRestart);
        homeButton.onClick.RemoveListener(OnHome);
    }

    private void OnRestart()
    {
        // Tell the GameFlowController to start a new run.
        gameFlowController.StartNewRun();
    }

    private void OnHome()
    {
        // Tell the GameFlowController to return to the menu.
        gameFlowController.ReturnToMenu();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        // Populate the UI with the final data from the just-ended run.
        PopulateSummary(gameFlowController.GetCurrentRunData());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void PopulateSummary(RunSessionData data)
    {
        // Update the best score if the new score is higher
        if (data.Score > scoreManager.BestScore)
        {
            scoreManager.BestScore = data.Score;
        }
        bestScoreText.text = $"Best: {scoreManager.BestScore}";
        
        SetDistanceText(data.Distance);
        SetCoinsText(data.CoinsCollected);
        SetGemsText(data.GemsCollected);
        SetTimeText(data.TimeSurvived);
        reviveUsedText.text = data.HasRevived ? "Revive Used: Yes" : "Revive Used: No";
        
        // Placeholder for Style Score as the system isn't implemented
        styleScoreText.text = "Style: --";

        // Animate the final score value.
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AnimateScoreRoutine(data.Score));
    }

    private void SetDistanceText(float distance)
    {
        stringBuilder.Clear();
        stringBuilder.Append(distance.ToString("F1")).Append("m");
        distanceText.SetText(stringBuilder);
    }

    private void SetCoinsText(int coins)
    {
        stringBuilder.Clear();
        stringBuilder.Append(coins);
        coinsCollectedText.SetText(stringBuilder);
    }

    private void SetGemsText(int gems)
    {
        stringBuilder.Clear();
        stringBuilder.Append(gems);
        gemsCollectedText.SetText(stringBuilder);
    }

    private void SetTimeText(float timeInSeconds)
    {
        int minutes = (int)(timeInSeconds / 60f);
        int seconds = (int)(timeInSeconds % 60f);
        stringBuilder.Clear();
        stringBuilder.Append(minutes.ToString("D2")).Append(":").Append(seconds.ToString("D2"));
        timeSurvivedText.SetText(stringBuilder);
    }

    private IEnumerator AnimateScoreRoutine(long targetScore)
    {
        long startScore = 0;
        float journey = 0f;

        while (journey < scoreAnimationDuration)
        {
            journey += Time.unscaledDeltaTime; // Use unscaled time to animate even when paused
            float percent = Mathf.Clamp01(journey / scoreAnimationDuration);
            float curvePercent = scoreAnimationCurve.Evaluate(percent);
            long currentScore = (long)Mathf.LerpUnclamped(startScore, targetScore, curvePercent);
            
            stringBuilder.Clear();
            stringBuilder.Append(currentScore);
            finalScoreText.SetText(stringBuilder);

            yield return null;
        }

        // Ensure the final score is exactly the target score.
        stringBuilder.Clear();
        stringBuilder.Append(targetScore);
        finalScoreText.SetText(stringBuilder);
    }
}
