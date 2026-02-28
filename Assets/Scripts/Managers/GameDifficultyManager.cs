
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the dynamic difficulty of the game.
/// It adjusts player speed, obstacle frequency, and obstacle complexity over time.
/// </summary>
public class GameDifficultyManager : MonoBehaviour
{
    [Header("Difficulty Settings")]
    [Tooltip("The ScriptableObject that defines the difficulty curve.")]
    [SerializeField] private DifficultyProfile difficultyProfile;

    private float timeAlive;
    private int currentDifficultyStep;

    public float CurrentPlayerSpeed { get; private set; }
    public float CurrentObstacleFrequency { get; private set; }

    private void Start()
    {
        ServiceLocator.Register<GameDifficultyManager>(this);
        ResetDifficulty();
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<GameDifficultyManager>();
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;
        UpdateDifficulty();
    }

    private void UpdateDifficulty()
    {
        // Find the current difficulty step
        while (currentDifficultyStep < difficultyProfile.difficultySteps.Count - 1 &&
               timeAlive > difficultyProfile.difficultySteps[currentDifficultyStep + 1].timeThreshold)
        {
            currentDifficultyStep++;
        }

        // Interpolate between the current and next difficulty steps
        if (currentDifficultyStep < difficultyProfile.difficultySteps.Count - 1)
        {
            DifficultyStep currentStep = difficultyProfile.difficultySteps[currentDifficultyStep];
            DifficultyStep nextStep = difficultyProfile.difficultySteps[currentDifficultyStep + 1];

            float t = (timeAlive - currentStep.timeThreshold) / (nextStep.timeThreshold - currentStep.timeThreshold);

            CurrentPlayerSpeed = Mathf.Lerp(currentStep.playerSpeed, nextStep.playerSpeed, t);
            CurrentObstacleFrequency = Mathf.Lerp(currentStep.obstacleFrequency, nextStep.obstacleFrequency, t);
        }
        else // Max difficulty
        {
            DifficultyStep maxStep = difficultyProfile.difficultySteps[currentDifficultyStep];
            CurrentPlayerSpeed = maxStep.playerSpeed;
            CurrentObstacleFrequency = maxStep.obstacleFrequency;
        }
    }

    public List<ObstacleData> GetObstaclesForCurrentDifficulty()
    {
        List<ObstacleData> possibleObstacles = new List<ObstacleData>();
        // This logic will need to be implemented based on the ObstacleData's min/max difficulty levels
        return possibleObstacles;
    }

    public void ResetDifficulty()
    {
        timeAlive = 0;
        currentDifficultyStep = 0;
        CurrentPlayerSpeed = difficultyProfile.difficultySteps[0].playerSpeed;
        CurrentObstacleFrequency = difficultyProfile.difficultySteps[0].obstacleFrequency;
    }
}

[System.Serializable]
public class DifficultyStep
{
    public float timeThreshold;
    public float playerSpeed;
    public float obstacleFrequency;
}

[CreateAssetMenu(fileName = "DifficultyProfile", menuName = "Endless Runner/Difficulty Profile")]
public class DifficultyProfile : ScriptableObject
{
    public List<DifficultyStep> difficultySteps;
}
