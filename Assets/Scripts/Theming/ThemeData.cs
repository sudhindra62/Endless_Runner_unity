using UnityEngine;

/// <summary>
/// A ScriptableObject that defines a complete visual and auditory theme for the game world.
/// This allows designers to easily create and configure themes as data assets.
/// </summary>
[CreateAssetMenu(fileName = "ThemeData", menuName = "Theming/Theme Data")]
public class ThemeData : ScriptableObject
{
    [Header("Theme Assets")]
    public string themeName;
    public Material skyboxMaterial;
    public Color lightingColor = Color.white;
    public GameObject particleOverlayPrefab;
    public Material obstacleMaterialVariant;
    public AudioClip musicTrack;
}
