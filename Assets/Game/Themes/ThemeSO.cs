
using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "EndlessRunner/Theme", order = 1)]
public class ThemeSO : ScriptableObject
{
    [Header("Theme Assets")]
    public GameObject[] environmentPrefabs;
    public Material skyboxMaterial;
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;
    public GameObject enemyChaserPrefab;

    [Header("Theme Settings")]
    public Color fogColor = Color.white;
    public float fogDensity = 0.02f;
    public LightmappingMode lightingMode = LightmappingMode.Realtime;
    public Color lightColor = Color.white;
    public float lightIntensity = 1.0f;
    public AudioClip music;

    [Header("UI")]
    public Color uiAccentColor = Color.white;
}
