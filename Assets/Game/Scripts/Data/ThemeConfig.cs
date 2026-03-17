using UnityEngine;

[CreateAssetMenu(fileName = "ThemeConfig", menuName = "Endless Runner/Theme Config", order = 1)]
public class ThemeConfig : ScriptableObject
{
    public string themeName;
    public Sprite themeIcon;

    [Header("Environment")]
    public Material skyboxMaterial;
    public Color fogColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    [Range(0.001f, 0.1f)]
    public float fogDensity = 0.01f;
    public GameObject[] environmentPrefabs;

    [Header("Gameplay Elements")]
    public GameObject[] segmentPrefabs;
    public GameObject[] obstaclePrefabs;
    public GameObject coinPrefab;
    public GameObject enemyChaserPrefab;
    public GameObject[] powerUpPrefabs;

    [Header("Audio")]
    public AudioClip backgroundMusic;
    public AudioClip coinSound;
    public AudioClip powerUpSound;

    [Header("UI")]
    public Color uiAccentColor = Color.white;
    public Sprite panelSprite;
}
