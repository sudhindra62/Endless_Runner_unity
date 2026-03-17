using UnityEngine;

public abstract class ThemeManagerBase : MonoBehaviour
{
    [Header("Theme Properties")]
    public Material skybox;
    public GameObject[] environmentPrefabs;
    public GameObject[] obstaclePrefabs;
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
