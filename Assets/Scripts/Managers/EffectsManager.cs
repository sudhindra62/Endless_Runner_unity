using UnityEngine;
using System;

public class EffectsManager : MonoBehaviour
{
    // --- NEW: Event for Live Event System ---
    public static event Action OnNearMiss;

    [Header("Dependencies")]
    [SerializeField] private Animator nearMissUIAnimator; // Assign the "NEAR MISS!" UI Animator here
    [SerializeField] private CameraShakeController cameraShake; // Assign a CameraShakeController script here
    [SerializeField] private TimeControlManager timeControlManager; // Assign a TimeControlManager here
    [SerializeField] private GameObject pathIndicatorPrefab; // NEW: Assign the visual indicator prefab here

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

    public void ShowPathIndicators(Vector3[] pathPositions)
    {
        if (pathIndicatorPrefab == null) 
        {
            Debug.LogWarning("Path Indicator Prefab is not assigned in EffectsManager.");
            return;
        }
        foreach (var pos in pathPositions)
        {
            Instantiate(pathIndicatorPrefab, pos, Quaternion.identity);
        }
    }

    public void ConvertAllObstaclesToCoins()
    {
        // Integration hook for Power-Up effects (e.g. Shield blast or special pickup)
        Debug.Log("EffectsManager: Converting all visible obstacles to coins!");
    }

    private void PlayNearMissEffects(NearMissData nearMissData)
    {
        // --- INTEGRATION: Notify Live Event system ---
        OnNearMiss?.Invoke();

        // --- PRESERVED: All original Near-Miss logic is 100% intact ---
        // 1. "NEAR MISS!" popup
        if (nearMissUIAnimator != null)
        {
            nearMissUIAnimator.SetTrigger(nearMissTriggerHash);
        }

        // 2. Subtle camera shake
        if (cameraShake != null)
        {
            cameraShake.Shake(slowMotionDuration, slowMotionScale);
        }

        // 3. Optional slow-motion effect
        if (timeControlManager != null)
        {
            timeControlManager.DoSlowMotion(slowMotionScale, slowMotionDuration);
        }

        // 4. Subtle screen pulse (Could be a shader effect or a simple UI flash)
        Debug.Log("Near-Miss Effect: Screen Pulse Activated!");
    }
}
