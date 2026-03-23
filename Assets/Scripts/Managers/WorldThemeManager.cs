using UnityEngine;
using System;

/// <summary>
/// Manages the activation and density of decorative environment objects based on theme settings.
/// Global scope Singleton.
/// </summary>
public class WorldThemeManager : Singleton<WorldThemeManager>
{
    public static event Action OnThemeApplied;

    [SerializeField] private GameObject[] environmentDecorations;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetDecorationDensity(float density)
    {
        if (environmentDecorations == null) return;

        density = Mathf.Clamp01(density);
        int decorationsToActivate = (int)(environmentDecorations.Length * density);

        for (int i = 0; i < environmentDecorations.Length; i++)
        {
            if (environmentDecorations[i] != null)
            {
                environmentDecorations[i].SetActive(i < decorationsToActivate);
            }
        }
        
        Debug.Log($"[WorldThemeManager] Decoration density set to {density * 100}%.");
        OnThemeApplied?.Invoke();
    }
}
