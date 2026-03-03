
using UnityEngine;
using System.Collections;

public class EffectsManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Animator nearMissUIAnimator; // Assign the "NEAR MISS!" UI Animator here
    [SerializeField] private CameraShake cameraShake; // Assign a CameraShake script here
    [SerializeField] private TimeControlManager timeControlManager; // Assign a TimeControlManager here

    [Header("Near-Miss Effects Settings")]
    [SerializeField] private float slowMotionDuration = 0.15f;
    [SerializeField] private float slowMotionScale = 0.5f;

    private readonly int nearMissTriggerHash = Animator.StringToHash("NearMiss");

    private void OnEnable()
    {
        NearMissManager.OnNearMiss += PlayNearMissEffects;
    }

    private void OnDisable()
    {
        NearMissManager.OnNearMiss -= PlayNearMissEffects;
    }

    private void PlayNearMissEffects()
    {
        // 1. "NEAR MISS!" popup
        if (nearMissUIAnimator != null)
        {
            nearMissUIAnimator.SetTrigger(nearMissTriggerHash);
        }

        // 2. Subtle camera shake
        if (cameraShake != null)
        {
            cameraShake.Shake();
        }

        // 3. Optional slow-motion effect
        if (timeControlManager != null)
        {
            timeControlManager.DoSlowMotion(slowMotionScale, slowMotionDuration);
        }

        // 4. Subtle screen pulse (Could be a shader effect or a simple UI flash)
        // For simplicity, we'll just log this for now. A UI dev would implement the visual.
        Debug.Log("Near-Miss Effect: Screen Pulse Activated!");
    }
}
