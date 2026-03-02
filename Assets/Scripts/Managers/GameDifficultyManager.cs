
using UnityEngine;
using System.Collections.Generic;

public class GameDifficultyManager : MonoBehaviour
{
    private readonly Dictionary<string, float> difficultyMultipliers = new Dictionary<string, float>();
    private const string ADAPTIVE_DIFFICULTY_SOURCE_ID = "AdaptiveDifficulty";

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        AdaptiveDifficultyManager.OnDifficultyChanged += HandleAdaptiveDifficultyChange;
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<GameDifficultyManager>();
        AdaptiveDifficultyManager.OnDifficultyChanged -= HandleAdaptiveDifficultyChange;
    }

    public void ApplyDifficultyMultiplier(string sourceId, float multiplier)
    {
        difficultyMultipliers[sourceId] = multiplier;
    }

    public void RemoveDifficultyMultiplier(string sourceId)
    {
        difficultyMultipliers.Remove(sourceId);
    }

    public float GetDifficultyMultiplier()
    {
        float totalMultiplier = 1f;
        foreach (var multiplier in difficultyMultipliers.Values)
        {
            totalMultiplier *= multiplier;
        }
        return totalMultiplier;
    }

    private void HandleAdaptiveDifficultyChange(float adaptiveMultiplier)
    {
        ApplyDifficultyMultiplier(ADAPTIVE_DIFFICULTY_SOURCE_ID, adaptiveMultiplier);
    }

    public void ResetState()
    {
        difficultyMultipliers.Clear();
    }
}
