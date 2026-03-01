using UnityEngine;
using System.Collections.Generic;

public class GameDifficultyManager : MonoBehaviour
{
    private readonly Dictionary<string, float> difficultyMultipliers = new Dictionary<string, float>();

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<GameDifficultyManager>();
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

    public void ResetState()
    {
        difficultyMultipliers.Clear();
    }
}
