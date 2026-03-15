using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance { get; private set; }

    [SerializeField]
    private ThemeSO[] themes;
    private ThemeSO currentTheme;
    private int currentThemeIndex = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetTheme(int themeIndex)
    {
        if (themeIndex < 0 || themeIndex >= themes.Length)
        {
            Debug.LogError("Invalid theme index: " + themeIndex);
            return;
        }

        currentThemeIndex = themeIndex;
        currentTheme = themes[currentThemeIndex];
        ApplyTheme();
    }

    public ThemeSO GetCurrentTheme()
    {
        return currentTheme;
    }

    private void ApplyTheme()
    {
        if (currentTheme == null) return;

        RenderSettings.skybox = currentTheme.skyboxMaterial;
        RenderSettings.fogColor = currentTheme.fogColor;
        RenderSettings.fogDensity = currentTheme.fogDensity;
        // Further lighting settings would be applied here.
    }

    public void NextTheme()
    {
        int nextThemeIndex = (currentThemeIndex + 1) % themes.Length;
        SetTheme(nextThemeIndex);
    }
}
