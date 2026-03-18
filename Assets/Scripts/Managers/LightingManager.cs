
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    // SECTION 1: LIGHTING SYSTEM
    [Header("Lighting Settings")]
    [SerializeField] private Light directionalLight;
    [SerializeField, Range(0, 24)] private float timeOfDay;
    [SerializeField] private float dayDuration = 120f; // 2 minutes for a full day-night cycle
    [SerializeField] private Gradient skyColor;
    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient sunColor;
    [SerializeField] private AnimationCurve sunIntensity;

    // SECTION 3: SKYBOX SYSTEM
    [Header("Skybox Settings")]
    [SerializeField] private Material skyboxMaterial;

    private void Update()
    {
        // Day/Night Cycle
        if (Application.isPlaying)
        {
            timeOfDay = (timeOfDay + Time.deltaTime / dayDuration) % 24;
        }
        UpdateLighting(timeOfDay / 24f);
    }

    private void UpdateLighting(float timePercent)
    {
        // Set ambient light
        RenderSettings.ambientSkyColor = skyColor.Evaluate(timePercent);
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timePercent);

        // Set directional light
        if (directionalLight != null)
        {
            directionalLight.color = sunColor.Evaluate(timePercent);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
            directionalLight.intensity = sunIntensity.Evaluate(timePercent);
        }

        // Set skybox material
        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetFloat("_AtmosphereThickness", 1.0f - Mathf.Abs(timePercent - 0.5f) * 2.0f);
        }
    }
}
