using UnityEngine;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour
{
    private readonly Dictionary<string, float> durationMultipliers = new Dictionary<string, float>();

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<PowerUpManager>();
    }

    public void ApplyDurationMultiplier(string sourceId, float multiplier)
    {
        durationMultipliers[sourceId] = multiplier;
    }

    public void RemoveDurationMultiplier(string sourceId)
    {
        durationMultipliers.Remove(sourceId);
    }

    public float GetDurationMultiplier()
    {
        float totalMultiplier = 1f;
        foreach (var multiplier in durationMultipliers.Values)
        {
            totalMultiplier *= multiplier;
        }
        return totalMultiplier;
    }

    public void ResetState()
    {
        durationMultipliers.Clear();
    }
}
