
using UnityEngine;

/// <summary>
/// A centralized singleton for triggering haptic feedback.
/// It respects the user's vibration setting and is safe to call on any platform.
/// </summary>
public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance { get; private set; }

#if UNITY_IOS || UNITY_ANDROID
    private bool canVibrate = true;
#else
    private bool canVibrate = false;
#endif

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

    private void Vibrate()
    {
        // Guard: Check both the user setting and the platform capability.
        if (SettingsManager.Instance.IsVibrationEnabled() && canVibrate)
        {
            Handheld.Vibrate();
        }
    }
    
    // --- Public Methods with Different Intensities (Placeholder) ---
    // Note: The standard Handheld.Vibrate() does not support intensity control.
    // These are named for clarity and future extension with more advanced haptic libraries.

    /// <summary>
    /// Triggers a light vibration, suitable for minor feedback like button clicks.
    /// </summary>
    public void VibrateLight()
    {
        // FUTURE HOOK: With a library like Lofelt Haptics, you would play a light clip here.
        Vibrate();
    }

    /// <summary>
    /// Triggers a medium vibration, suitable for events like collecting a power-up.
    /// </summary>
    public void VibrateMedium()
    {
        Vibrate();
    }

    /// <summary>
    /// Triggers a heavy vibration, suitable for major events like hitting an obstacle.
    /// </summary>
    public void VibrateHeavy()
    { 
        // FUTURE HOOK: With a library like Lofelt Haptics, you would play a heavy clip here.
        Vibrate();
    }

    // --- Safe Event Hooks for Gameplay ---
    // These can be called from gameplay systems without direct coupling.
    public void OnObstacleHit() => VibrateHeavy();
    public void OnShieldBreak() => VibrateMedium();
    public void OnCoinCollected() => VibrateLight(); // Note: This might be too much; use sparingly.
    public void OnButtonClick() => VibrateLight();
}
