using UnityEngine;
using System.Collections;

public class GameDifficultyManager : MonoBehaviour
{
    public static GameDifficultyManager Instance { get; private set; }

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

    // Public properties to be read by other systems like PlayerController and TileSpawner
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
        // Ensure coroutines are stopped when the object is disabled to prevent memory leaks.
        EndRun();
    }

    /// <summary>
    /// Called by the GameManager to start the difficulty progression at the beginning of a run.
    /// </summary>
    public void BeginRun()
    {
        ResetDifficulty();
        
        _difficultyCoroutine = StartCoroutine(ScaleDifficultyOverTime());
        _restPhaseCoroutine = StartCoroutine(ManageRestPhases());
    }

    /// <summary>
    /// Called by the GameManager to stop difficulty progression when the run ends.
    /// </summary>
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
    }

    private IEnumerator ScaleDifficultyOverTime()
    {
        while (true)
        {
            if (!IsInRestPhase)
            {
                // Linearly increase speed multiplier over time until max is reached
                if (SpeedMultiplier < maxSpeedMultiplier)
                {
                    SpeedMultiplier += speedIncreaseRate * Time.deltaTime;
                    SpeedMultiplier = Mathf.Min(SpeedMultiplier, maxSpeedMultiplier);
                }

                // Map the current speed progress to the obstacle gap range
                float difficultyT = Mathf.InverseLerp(initialSpeedMultiplier, maxSpeedMultiplier, SpeedMultiplier);
                ObstacleGap = Mathf.Lerp(initialObstacleGap, minObstacleGap, difficultyT);
            }
            yield return null; // Wait for the next frame
        }
    }

    private IEnumerator ManageRestPhases()
    {
        while (true)
        {
            // Wait for a random duration within the defined interval
            yield return new WaitForSeconds(Random.Range(minRestInterval, maxRestInterval));
            
            // Activate the rest phase, pausing difficulty progression
            IsInRestPhase = true;
            yield return new WaitForSeconds(restDuration);
            IsInRestPhase = false;
        }
    }
}
