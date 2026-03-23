using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    public static CameraShakeController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Shake(float duration, float magnitude)
    {
        // Redirect to CameraController if it exists
        var camController = CameraController.Instance;
        if (camController != null)
        {
            camController.ShakeCamera(); // We can ignore duration/magnitude for now or extend CameraController
        }
    }
}
