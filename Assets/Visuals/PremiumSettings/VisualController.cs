using UnityEngine;

public class VisualController : MonoBehaviour
{
    public Material pbrMaterial;
    public Light directionalLight;

    void Start()
    {
        // Enhance visuals
        RenderSettings.skybox = new Material(Shader.Find("Skybox/Procedural"));
        directionalLight.intensity = 1.5f;
        directionalLight.shadows = LightShadows.Soft;

        // Apply PBR material to all renderers
        Renderer[] renderers = FindObjectsByType<Renderer>(FindObjectsSortMode.None);
        foreach (Renderer renderer in renderers)
        {
            renderer.material = pbrMaterial;
        }
    }
}
