
using TMPro;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages the Heads-Up Display for all in-game stats, including score, combos, and Fever Mode.
/// It subscribes to manager events to update its values, acting as a passive view with high performance.
/// </summary>
public class GameHUDController : MonoBehaviour
{
    [Header("Text Components")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI gemText;
    [SerializeField] private TextMeshProUGUI comboText; // For Flow Combo
    [SerializeField] private TextMeshProUGUI feverCountdownText; // For Fever Countdown

    [Header("UI Elements")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Image feverGaugeBar; // For Fever Gauge
    [SerializeField] private GameObject feverActiveOverlay; // For Fever Visual Effect

    [Header("Animations")]
    [SerializeField] private Animator multiplierAnimator;
    private readonly int multiplierIncreaseTrigger = Animator.StringToHash("MultiplierUp");

    [Header("Sub-Controllers")]
    [SerializeField] private PowerUpHUDController powerUpHUDController;

    // Caching for performance
    private readonly StringBuilder scoreBuilder = new StringBuilder(16);
    private readonly StringBuilder timeBuilder = new StringBuilder(8);
    private readonly StringBuilder multiplierBuilder = new StringBuilder(4);
    private readonly StringBuilder currencyBuilder = new StringBuilder(10);
    private readonly StringBuilder comboBuilder = new StringBuilder(12);

    private ScoreManager scoreManager;
    private CurrencyManager currencyManager;
    private GameFlowController gameFlowController;
    
    private bool isPaused = false;
    private float runTime = 0f;
    private Coroutine feverCountdownCoroutine;

    private void Start()
    {
        // Resolve dependencies
        scoreManager = ServiceLocator.Get<ScoreManager>();
        currencyManager = ServiceLocator.Get<CurrencyManager>();
        gameFlowController = ServiceLocator.Get<GameFlowController>();

        // Initial state setup
        ResetHUD();
    }

    private void OnEnable()
    {
        // Subscribe to all relevant events
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        ScoreManager.OnScoreChanged += UpdateScoreText;
        CurrencyManager.OnCoinsChanged += UpdateCoinText;
        CurrencyManager.OnGemsChanged += UpdateGemText;

        FlowComboManager.OnComboChanged += UpdateComboText;
        FlowComboManager.OnMultiplierChanged += UpdateMultiplierText;
        FlowComboManager.OnComboBroken += OnComboBroken;

        FeverModeManager.OnFeverGaugeChanged += UpdateFeverGauge;
        FeverModeManager.OnFeverStart += OnFeverStart;
        FeverModeManager.OnFeverEnd += OnFeverEnd;

        if (pauseButton != null) pauseButton.onClick.AddListener(OnPauseClicked);
    }

    private void OnDisable()
    {
        // Unsubscribe from all events
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        ScoreManager.OnScoreChanged -= UpdateScoreText;
        CurrencyManager.OnCoinsChanged -= UpdateCoinText;
        CurrencyManager.OnGemsChanged -= UpdateGemText;

        FlowComboManager.OnComboChanged -= UpdateComboText;
        FlowComboManager.OnMultiplierChanged -= UpdateMultiplierText;
        FlowComboManager.OnComboBroken -= OnComboBroken;
        
        FeverModeManager.OnFeverGaugeChanged -= UpdateFeverGauge;
        FeverModeManager.OnFeverStart -= OnFeverStart;
        FeverModeManager.OnFeverEnd -= OnFeverEnd;

        if (pauseButton != null) pauseButton.onClick.RemoveListener(OnPauseClicked);
    }

    private void Update()
    {
        if (!isPaused)
        {
            runTime += Time.deltaTime;
            UpdateTimerText(runTime);
        }
    }

    private void HandleGameStateChanged(GameState newState)
    {
        isPaused = (newState == GameState.Paused);
        if (newState == GameState.Menu || newState == GameState.EndOfRun)
        {
            ResetHUD();
        }
    }

    private void OnPauseClicked()
    {
        gameFlowController?.PauseGame();
    }

    private void UpdateScoreText(int newScore)
    {
        scoreBuilder.Clear();
        scoreBuilder.Append(newScore);
        scoreText.SetText(scoreBuilder);
    }

    private void UpdateMultiplierText(int newMultiplier)
    {
        multiplierBuilder.Clear();
        multiplierBuilder.Append("x").Append(newMultiplier);
        multiplierText.SetText(multiplierBuilder);
        
        if (newMultiplier > 1 && multiplierAnimator != null)
        {
            multiplierAnimator.SetTrigger(multiplierIncreaseTrigger);
        }
    }

    private void UpdateComboText(int comboCount)
    {
        if (comboCount > 1)
        {
            comboBuilder.Clear();
            comboBuilder.Append("COMBO ").Append(comboCount);
            comboText.SetText(comboBuilder);
            comboText.enabled = true;
        }
        else
        {
            comboText.enabled = false;
        }
    }

    private void OnComboBroken()
    {
        comboText.enabled = false;
        // Future: Play combo break particle effect or animation here
    }

    private void UpdateFeverGauge(float progress)
    {
        if (feverGaugeBar != null) feverGaugeBar.fillAmount = progress;
    }

    private void OnFeverStart(float duration)
    {
        if (feverActiveOverlay != null) feverActiveOverlay.SetActive(true);
        if (feverCountdownText != null) feverCountdownText.enabled = true;
        if (feverCountdownCoroutine != null) StopCoroutine(feverCountdownCoroutine);
        feverCountdownCoroutine = StartCoroutine(FeverCountdownRoutine(duration));
    }

    private void OnFeverEnd()
    {
        if (feverActiveOverlay != null) feverActiveOverlay.SetActive(false);
        if (feverCountdownText != null) feverCountdownText.enabled = false;
        if (feverCountdownCoroutine != null)
        {
            StopCoroutine(feverCountdownCoroutine);
            feverCountdownCoroutine = null;
        }
    }

    private IEnumerator FeverCountdownRoutine(float duration)
    {
        float timer = duration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            feverCountdownText.SetText(Mathf.CeilToInt(timer).ToString());
            yield return null;
        }
    }

    private void UpdateCoinText(int amount)
    {
        currencyBuilder.Clear();
        currencyBuilder.Append(amount);
        coinText.SetText(currencyBuilder);
    }

    private void UpdateGemText(int amount)
    {
        currencyBuilder.Clear();
        currencyBuilder.Append(amount);
        gemText.SetText(currencyBuilder);
    }

    private void UpdateTimerText(float timeInSeconds)
    {
        int minutes = (int)(timeInSeconds / 60f);
        int seconds = (int)(timeInSeconds % 60f);

        timeBuilder.Clear();
        timeBuilder.Append(minutes.ToString("D2"));
        timeBuilder.Append(":");
        timeBuilder.Append(seconds.ToString("D2"));

        timerText.SetText(timeBuilder);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        if (powerUpHUDController != null) powerUpHUDController.ShowAll();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        if (powerUpHUDController != null) powerUpHUDController.HideAll();
    }

    public void ResetHUD()
    {
        runTime = 0f;
        UpdateTimerText(0);
        if (scoreManager != null) UpdateScoreText(0);
        UpdateMultiplierText(1);
        UpdateComboText(0);

        if (feverGaugeBar != null) feverGaugeBar.fillAmount = 0;
        if (feverActiveOverlay != null) feverActiveOverlay.SetActive(false);
        if (feverCountdownText != null) feverCountdownText.enabled = false;
        if (feverCountdownCoroutine != null) StopCoroutine(feverCountdownCoroutine);

        powerUpHUDController?.ResetIcons();
    }
}
