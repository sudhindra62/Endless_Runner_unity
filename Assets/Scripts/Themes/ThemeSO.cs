using UnityEngine;

public enum ThemeUnlockType
{
    Free,
    GemUnlock,
    PremiumSubscription,
    Premium = PremiumSubscription
}

[System.Serializable]
public class ThemeConfig
{
    public float baseMoveSpeed = 10f;
    public float maxMoveSpeed = 25f;
    public float speedIncrement = 0.1f;
    public float obstacleProbability = 0.5f;

    [Header("Game Element Prefabs")]
    public GameObject coinPrefab;
    public GameObject enemyChaserPrefab;
    public GameObject[] obstaclePrefabs;
}

[CreateAssetMenu(fileName = "ThemeSO", menuName = "EndlessRunner/ThemeSO", order = 0)]
public class ThemeSO : ScriptableObject
{
    [Header("Theme Config")]
    public ThemeConfig config;
    public GameObject coinPrefab => config != null ? config.coinPrefab : null;

    [Header("Theme Info")]
    public string themeName;
    public Sprite themeIcon;
    public GameObject themePreviewPrefab;

    [Header("Unlock Conditions")]
    public ThemeUnlockType unlockType;
    public int gemPrice;

    [Header("Visuals")]
    public Material skybox;
    public Color lightingColor = Color.white;
    public float lightingIntensity = 1.0f;
    public Color fogColor = Color.grey;
    public float fogDensity = 0.01f;

    [Header("MATERIALS")]
    public Material groundMaterial;
    public Material obstacleMaterial;
    
    [Header("PREFABS & MODULES - MISSING FIELDS")]
    public GameObject enemyChaserPrefab; // ADDED: Referenced in errors
    public GameObject[] environmentModules; // ADDED: Missing environment variation
    public GameObject themeManagerPrefab; // ADDED: Theme-specific manager

    [Header("Audio")]
    public AudioClip backgroundMusic;
    public AudioClip coinSound;
    
    [Header("UI")]
    public Color uiAccentColor = Color.white;
    public Sprite uiPanelSprite;
    public Sprite[] animatedBackgroundFrames;
    
    [Header("Lighting Instances")]
    public Light sunLight;
    public Light moonLight;
}
