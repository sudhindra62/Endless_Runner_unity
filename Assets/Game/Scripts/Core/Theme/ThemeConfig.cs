using UnityEngine;
using EndlessRunner.Utils;

[CreateAssetMenu(fileName = "ThemeConfig", menuName = "Endless Runner/Theme Config", order = 1)]
public class ThemeConfig : ScriptableObject
{
    [Header("Theme Unlock Properties")]
    public ThemeUnlockType unlockType = ThemeUnlockType.Free;
    public int gemPrice = 500;

    [Header("Theme Properties")]
    public Material skybox;
    public GameObject[] environmentPrefabs;
    public GameObject[] obstaclePrefabs;
    public GameObject[] segmentPrefabs; // Added for procedural level generation
    public GameObject coinPrefab;
    public GameObject enemyChaserPrefab;
    public AudioClip music;

    [Header("UI Accents")]
    public Color primaryColor;
    public Color secondaryColor;

    [Header("Lighting")]
    public Light sunLight;
    public Light moonLight;
}
