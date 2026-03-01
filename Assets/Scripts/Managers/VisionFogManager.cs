
using UnityEngine;

public class VisionFogManager : MonoBehaviour
{
    public static VisionFogManager Instance { get; private set; }

    // Example: Could be a reference to a post-processing volume or a shader property
    [SerializeField] private float defaultFogIntensity = 0.2f;

    private void Awake()
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

    public void ActivateDarkVision(float intensity)
    {
        // Logic to increase fog density, reduce light intensity, etc.
        Debug.Log($"Dark Vision activated. Fog intensity set to: {intensity}");
    }

    public void DeactivateDarkVision()
    {
        // Revert to default lighting and fog settings
        Debug.Log($"Dark Vision deactivated. Fog intensity reset to: {defaultFogIntensity}");
    }
}
