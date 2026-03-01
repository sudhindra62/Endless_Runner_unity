using UnityEngine;

public class VisionFogManager : MonoBehaviour
{
    // This would typically control a post-processing effect or a shader.
    // For this example, we'll just log the state.

    private bool isFogActive = false;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<VisionFogManager>();
    }

    public void SetFogState(bool isActive)
    {
        isFogActive = isActive;
        Debug.Log($"Vision Fog is now {(isActive ? "ON" : "OFF")}");
        // In a real implementation, you would enable/disable a fog volume
        // or set a shader parameter here.
    }

    public void ResetState()
    {
        SetFogState(false);
    }
}
