using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "ThemeProfileData", menuName = "Theming/Theme Profile Data")]
public class ThemeProfileData : ScriptableObject
{
    [Header("Theme Identification")]
    public string ThemeID;

    [Header("Visuals")]
    public Material Skybox;
    public Color DirectionalLightColor = Color.white;
    public float DirectionalLightIntensity = 1.0f;
    public Vector3 DirectionalLightRotation = new Vector3(50, -30, 0);
    public bool FogEnabled = true;
    public Color FogColor = Color.gray;
    public float FogDensity = 0.01f;
    public VolumeProfile PostProcessingProfile;
    public Color UIAccentColor = Color.white;

    [Header("Materials")]
    public ThemeMaterialRegistry MaterialRegistry;

    [Header("Audio")]
    public ThemeAudioProfile AudioProfile;

    [Header("VFX")]
    public ThemeVisualEffectProfile VFXProfile;

    [Header("Gameplay Overrides")]
    public string WeatherOverlayType;
}
