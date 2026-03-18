
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public static SkyboxManager Instance { get; private set; }

    [Header("Theme Skyboxes")]
    [SerializeField] private Material futuristicSky;
    [SerializeField] private Material heavenClouds;
    [SerializeField] private Material hellFireSky;
    [SerializeField] private Material jungleCanopy;
    [SerializeField] private Material oceanHorizon;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSkybox(string theme)
    {
        switch (theme)
        {
            case "Futuristic":
                RenderSettings.skybox = futuristicSky;
                break;
            case "Heaven":
                RenderSettings.skybox = heavenClouds;
                break;
            case "Hell":
                RenderSettings.skybox = hellFireSky;
                break;
            case "Jungle":
                RenderSettings.skybox = jungleCanopy;
                break;
            case "WaterPark":
                RenderSettings.skybox = oceanHorizon;
                break;
            default:
                break;
        }
    }
}
