using UnityEngine;
using System.Collections;
using System;

public class GameDifficultyManager : MonoBehaviour
{
    public static GameDifficultyManager Instance { get; private set; }
    public static event Action<float> OnSpeedMultiplierChanged;

    [Header("Speed Settings")]
    [Tooltip("The initial speed multiplier.")]
    public float initialSpeedMultiplier = 1f;
    [Tooltip("The maximum speed multiplier.")]
    public float maxSpeedMultiplier = 2.5f;
    [Tooltip("The rate at which the speed multiplier increases per second.")]
    public float speedIncreaseRate = 0.01f;

    [Header("Rest Phase Settings")]
    [Tooltip("The minimum time between rest phases.")]
    public float minRestInterval = 60f;
    [Tooltip("The maximum time between rest phases.")]
    public float maxRestInterval = 120f;
    [Tooltip("The duration of each rest phase.")]
    public float restDuration = 10f;

    [Header("Obstacle Settings")]
    [Tooltip("The initial gap between obstacles, used by the TileSpawner.")]
    public float initialObstacleGap = 15f;
    [Tooltip("The minimum gap between obstacles at max difficulty.")]
    public float minObstacleGap = 5f;

    public float SpeedMultiplier { get; private set; }
    public float ObstacleGap { get; private set; }
    public bool IsInRestPhase { get; private set; }

    private Coroutine _difficultyCoroutine;
    private Coroutine _restPhaseCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        EndRun();
    }

    public void BeginRun()
    {
        ResetDifficulty();
        
        _difficultyCoroutine = StartCoroutine(ScaleDifficultyOverTime());
        _restPhaseCoroutine = StartCoroutine(ManageRestPhases());
    }

    public void EndRun()
    {
        if (_difficultyCoroutine != null)
        {
            StopCoroutine(_difficultyCoroutine);
            _difficultyCoroutine = null;
        }
        if (_restPhaseCoroutine != null)
        {
            StopCoroutine(_restPhaseCoroutine);
            _restPhaseCoroutine = null;
        }
    }

    private void ResetDifficulty()
    {
        SpeedMultiplier = initialSpeedMultiplier;
        ObstacleGap = initialObstacleGap;
        IsInRestPhase = false;
        OnSpeedMultiplierChanged?.Invoke(SpeedMultiplier);
    }

    private IEnumerator ScaleDifficultyOverTime()
    {
        while (true)
        {
            if (!IsInRestPhase)
            {
                if (SpeedMultiplier < maxSpeedMultiplier)
                {
                    float oldMultiplier = SpeedMultiplier;
                    SpeedMultiplier += speedIncreaseRate * Time.deltaTime;
                    SpeedMultiplier = Mathf.Min(SpeedMultiplier, maxSpeedMultiplier);

                    // Invoke event only if the multiplier has changed
                    if (Mathf.Abs(oldMultiplier - SpeedMultiplier) > Mathf.Epsilon)
                    {
                        OnSpeedMultiplierChanged?.Invoke(SpeedMultiplier);
                    }
                }

                float difficultyT = Mathf.InverseLerp(initialSpeedMultiplier, maxSpeedMultiplier, SpeedMultiplier);
                ObstacleGap = Mathf.Lerp(initialObstacleGap, minObstacleGap, difficultyT);
            }
            yield return null;
        }
    }

    private IEnumerator ManageRestPhases()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minRestInterval, maxRestInterval));
            
            IsInRestPhase = true;
            yield return new WaitForSeconds(restDuration);
            IsInRestPhase = false;
        }
    }
}
