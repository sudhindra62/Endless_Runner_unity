using UnityEngine;
using System;

public class LiveEventManager : MonoBehaviour
{
    public static event Action<float> OnEventBoostChanged;

    // This would be driven by a live event system, fetching data from a server.
    // For now, we'll simulate a boost.
    private float currentEventBoost = 1.2f;

    private void Start()
    {
        // In a real scenario, this would be called when live event data is fetched and parsed.
        OnEventBoostChanged?.Invoke(currentEventBoost);
    }
}
