using UnityEngine;
using System;

/// <summary>
/// Placeholder for a future event manager that would broadcast
/// environment-specific events, potentially influenced by the current theme.
/// </summary>
public class EnvironmentEventManager : Singleton<EnvironmentEventManager>
{
    public static event Action<string> OnWeatherEffectTriggered;

    public void TriggerWeatherEffect(string effectName)
    {
        OnWeatherEffectTriggered?.Invoke(effectName);
    }
}
