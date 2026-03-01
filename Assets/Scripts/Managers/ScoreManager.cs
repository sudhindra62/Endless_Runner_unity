using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event Action<int> OnScoreChanged;

    [Header("Scoring Configuration")]
    [SerializeField] private float scorePerSecond = 10f;

    public int CurrentScore { get; private set; }
    
    private int comboMultiplier = 1; 
    private int activeMultiplier = 1;
    private readonly Dictionary<string, int> externalMultipliers = new Dictionary<string, int>();

    private bool isFeverActive = false;
    private bool isPlaying;
    private float scoreTimer;

    private FeverModeManager feverModeManager;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        feverModeManager = ServiceLocator.Get<FeverModeManager>();

        GameManager.OnGameStateChanged += OnGameStateChanged;
        FlowComboManager.OnMultiplierChanged += OnComboMultiplierChanged;
        FeverModeManager.OnFeverStart += OnFeverStart;
        FeverModeManager.OnFeverEnd += OnFeverEnd;
        ResetState();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
        FlowComboManager.OnMultiplierChanged -= OnComboMultiplierChanged;
        FeverModeManager.OnFeverStart -= OnFeverStart;
        FeverModeManager.OnFeverEnd -= OnFeverEnd;
        ServiceLocator.Unregister<ScoreManager>();
    }

    private void Update()
    {
        if (!isPlaying) return;

        scoreTimer += Time.deltaTime;
        if (scoreTimer >= 1f)
        {
            AddScore((int)scorePerSecond);
            scoreTimer = 0f;
        }
    }

    public void AddScore(int baseAmount)
    {
        if (baseAmount <= 0) return;

        int totalMultiplier = activeMultiplier;
        foreach (var multiplier in externalMultipliers.Values)
        {
            totalMultiplier *= multiplier;
        }

        CurrentScore += baseAmount * totalMultiplier;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void ApplyScoreMultiplier(string sourceId, int multiplier)
    {
        externalMultipliers[sourceId] = Mathf.Max(1, multiplier);
    }

    public void RemoveScoreMultiplier(string sourceId)
    {
        externalMultipliers.Remove(sourceId);
    }

    public void ResetState()
    {
        CurrentScore = 0;
        comboMultiplier = 1;
        activeMultiplier = 1;
        isFeverActive = false;
        scoreTimer = 0f;
        externalMultipliers.Clear();
        OnScoreChanged?.Invoke(CurrentScore);
    }

    private void OnGameStateChanged(GameState newState)
    {
        isPlaying = newState == GameState.Playing;
        if (newState == GameState.Menu || newState == GameState.EndOfRun)
        {
            ResetState();
        }
    }

    private void OnComboMultiplierChanged(int newMultiplier)
    {
        comboMultiplier = newMultiplier;
        if (!isFeverActive)
        {
            activeMultiplier = comboMultiplier;
        }
    }

    private void OnFeverStart(float duration)
    {
        isFeverActive = true;
        if (feverModeManager != null)
        {
            activeMultiplier = feverModeManager.GetFeverMultiplier();
        }
    }

    private void OnFeverEnd()
    {
        isFeverActive = false;
        activeMultiplier = comboMultiplier;
    }
}
