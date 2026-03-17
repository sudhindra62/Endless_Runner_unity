using UnityEngine;

public enum ThemeUnlockType
{
    Free,
    GemUnlock,
    PremiumSubscription
}

[CreateAssetMenu(fileName = "Theme", menuName = "Theme/New Theme")]
public class ThemeSO : ScriptableObject
{
    [Header("Theme Info")]
    public string themeName;
    public Sprite themeIcon;

    [Header("Unlock Conditions")]
    public ThemeUnlockType unlockType;
    public int gemPrice;

    [Header("Environment Settings")]
    public Material skyboxMaterial;
    public Color fogColor = Color.white;
    public float fogDensity = 0.01f;
    public GameObject[] environmentPrefabs;

    [Header("Gameplay Elements")]
    public GameObject[] obstaclePrefabs;
    public GameObject coinPrefab;
    public GameObject enemyChaserPrefab;
    public GameObject[] powerUpPrefabs;

    [Header("Audio")]
    public AudioClip backgroundMusic;
    public AudioClip coinSound;
    public AudioClip powerUpSound;

    [Header("UI Style")]
    public Color uiAccentColor = Color.white;
    public Sprite panelSprite;
}
