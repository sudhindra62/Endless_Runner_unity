
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    // SECTION 1: LIGHTING SYSTEM
    [Header("Lighting Settings")]
    [SerializeField] private Light directionalLight;
    [SerializeField, Range(0, 24)] private float timeOfDay;
    [SerializeField] private float dayDuration = 120f; // 2 minutes for a full day-night cycle
    
    [Header("Futuristic City Theme")]
    [SerializeField] private Gradient skyColorFuturistic;
    [SerializeField] private Gradient equatorColorFuturistic;
    [SerializeField] private Gradient sunColorFuturistic;
    [SerializeField] private Color neonGlowColor = new Color(0, 0.8f, 1, 1); // Cyan-like glow

    private Gradient skyColor;
    private Gradient equatorColor;
    private Gradient sunColor;

    // SECTION 3: SKYBOX SYSTEM
    [Header("Skybox Settings")]
    [SerializeField] private Material skyboxMaterial;

    private void Start()
    {
        // Default to futuristic theme for now
        SetFuturisticTheme();
    }

    private void Update()
    {
        // Day/Night Cycle
        if (Application.isPlaying)
        {
            timeOfDay = (timeOfDay + Time.deltaTime / dayDuration) % 24;
        }
        UpdateLighting(timeOfDay / 24f);
    }

    public void SetFuturisticTheme()
    {
        skyColor = skyColorFuturistic;
        equatorColor = equatorColorFuturistic;
        sunColor = sunColorFuturistic;
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
        }

        // Update neon glow for futuristic city materials
        float emission = Mathf.PingPong(Time.time * 0.5f, 0.5f) + 0.5f;
        RenderSettings.reflectionIntensity = emission;
        Shader.SetGlobalColor("_NeonGlowColor", neonGlowColor * emission);

        // Set skybox material
        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetFloat("_AtmosphereThickness", 1.0f - Mathf.Abs(timePercent - 0.5f) * 2.0f);
        }
    }
}
