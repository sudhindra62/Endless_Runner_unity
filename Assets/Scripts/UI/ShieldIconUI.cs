using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the visibility of the Shield Icon on the game HUD.
/// It listens for events to know when the player's shield status changes.
/// </summary>
[RequireComponent(typeof(Image))]
public class ShieldIconUI : MonoBehaviour
{
    private Image shieldIconImage;

    void Awake()
    {
        shieldIconImage = GetComponent<Image>();
        shieldIconImage.enabled = false; // Start with the icon hidden
    }

    void OnEnable()
    {
        // Subscribe to the event that signals a change in shield status
        PlayerStatusNotifier.OnShieldStatusChanged += OnShieldStatusChanged;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        PlayerStatusNotifier.OnShieldStatusChanged -= OnShieldStatusChanged;
    }

    /// <summary>
    /// Callback function to show or hide the icon.
    /// </summary>
    /// <param name="isShieldActive">The new status of the shield.</param>
    private void OnShieldStatusChanged(bool isShieldActive)
    {
        if (shieldIconImage != null)
        {
            shieldIconImage.enabled = isShieldActive;
        }
    }
}
