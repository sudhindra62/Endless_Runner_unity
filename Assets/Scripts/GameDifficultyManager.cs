using UnityEngine;
using System;
using System.Collections;

public class GameDifficultyManager : MonoBehaviour
{
    public static GameDifficultyManager Instance { get; private set; }

    public static event Action<float> OnSpeedMultiplierChanged;

    [Header("Difficulty Settings")]
    [Tooltip("An animation curve to define the speed multiplier over time.")]
    public AnimationCurve speedCurve = AnimationCurve.EaseInOut(0, 1, 300, 2.5f);
    [Tooltip("An animation curve to define the spawn rate multiplier over time.")]
    public AnimationCurve spawnRateCurve = AnimationCurve.EaseInOut(0, 1, 240, 2);

    [Header("Rest Phase Settings")]
    [Tooltip("The minimum time between rest phases.")]
    public float minRestInterval = 60f;
    [Tooltip("The maximum time between rest phases.")]
    public float maxRestInterval = 120f;
    [Tooltip("The duration of each rest phase.")]
    public float restDuration = 10f;

    public float SpeedMultiplier { get; private set; } = 1f;
    public float SpawnRateMultiplier { get; private set; } = 1f;
    public bool IsResting { get; private set; } = false;

    private float timeElapsed;
    private float unscaledTimeElapsed;
    private float nextRestTime;

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
        ResetDifficulty();
    }

    private void Update()
    {
        if (IsResting) return;

        timeElapsed += Time.deltaTime;
        unscaledTimeElapsed += Time.unscaledDeltaTime;

        UpdateDifficulty();
        CheckForRestPhase();
    }

    private void UpdateDifficulty()
    {
        float newSpeedMultiplier = speedCurve.Evaluate(timeElapsed);
        if (Mathf.Abs(newSpeedMultiplier - SpeedMultiplier) > Mathf.Epsilon)
        {
            SpeedMultiplier = newSpeedMultiplier;
            OnSpeedMultiplierChanged?.Invoke(SpeedMultiplier);
        }

        SpawnRateMultiplier = spawnRateCurve.Evaluate(timeElapsed);
    }

    private void CheckForRestPhase()
    {
        if (unscaledTimeElapsed >= nextRestTime)
        {
            StartCoroutine(RestPhaseRoutine());
        }
    }

    private IEnumerator RestPhaseRoutine()
    {
        IsResting = true;
        SetNextRestTime();

        float originalSpeedMultiplier = SpeedMultiplier;
        
        SpeedMultiplier = 1f;
        OnSpeedMultiplierChanged?.Invoke(SpeedMultiplier);

        SpawnRateMultiplier = 0.5f;

        yield return new WaitForSeconds(restDuration);

        SpeedMultiplier = originalSpeedMultiplier;
        OnSpeedMultiplierChanged?.Invoke(SpeedMultiplier);

        IsResting = false;
    }

    public void ResetDifficulty()
    {
        timeElapsed = 0;
        unscaledTimeElapsed = 0;
        
        float newSpeedMultiplier = speedCurve.Evaluate(0);
        if (Mathf.Abs(newSpeedMultiplier - SpeedMultiplier) > Mathf.Epsilon)
        {
            SpeedMultiplier = newSpeedMultiplier;
            OnSpeedMultiplierChanged?.Invoke(SpeedMultiplier);
        }

        SpawnRateMultiplier = spawnRateCurve.Evaluate(0);
        IsResting = false;
        SetNextRestTime();
    }

    private void SetNextRestTime()
    {
        nextRestTime = unscaledTimeElapsed + Random.Range(minRestInterval, maxRestInterval);
    }
}
