using UnityEngine;

public class Lighting : MonoBehaviour
{
    void OnEnable()
    {
        ThemeManager.OnThemeChanged += UpdateLighting;
    }

    void OnDisable()
    {
        ThemeManager.OnThemeChanged -= UpdateLighting;
    }

    void Start()
    {
        // Initial lighting setup
        if (ThemeManager.Instance != null)
        {
            UpdateLighting(ThemeManager.Instance.CurrentConfig);
        }
    }

    private void UpdateLighting(ThemeConfig config)
    {
        if (config == null) return;

        RenderSettings.skybox = config.skyboxMaterial;
        RenderSettings.fog = true;
        RenderSettings.fogColor = config.fogColor;
        RenderSettings.fogDensity = config.fogDensity;
        DynamicGI.UpdateEnvironment();
    }
}
