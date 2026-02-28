
using UnityEngine;

/// <summary>
/// Adjusts a RectTransform to respect the safe area of the screen.
/// This is essential for ensuring UI elements are not obscured by notches, camera holes, or rounded corners on modern mobile devices.
/// It should be attached to a top-level canvas or specific panels that need to be constrained.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class SafeAreaHandler : MonoBehaviour
{
    private RectTransform panelRectTransform;
    private Rect lastSafeArea = new Rect(0, 0, 0, 0);
    private ScreenOrientation lastScreenOrientation;

    private void Awake()
    {
        panelRectTransform = GetComponent<RectTransform>();
        lastScreenOrientation = Screen.orientation;
        ApplySafeArea();
    }

    private void Update()
    {
        // The safe area can change, for example, when the device is rotated.
        // We only need to re-apply the safe area if it has actually changed.
        if (Screen.safeArea != lastSafeArea || Screen.orientation != lastScreenOrientation)
        {
            lastSafeArea = Screen.safeArea;
            lastScreenOrientation = Screen.orientation;
            ApplySafeArea();
        }
    }

    /// <summary>
    /// Applies the screen's safe area to the RectTransform's anchors.
    /// This effectively shrinks the RectTransform to fit within the safe area.
    /// </summary>
    private void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        // Convert the safe area from screen space to normalized viewport coordinates.
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // Apply the normalized anchors to the RectTransform.
        panelRectTransform.anchorMin = anchorMin;
        panelRectTransform.anchorMax = anchorMax;

        // Log the details for debugging purposes.
        Debug.Log($"Applied Safe Area. Anchor Min: {anchorMin}, Anchor Max: {anchorMax}");
    }
}
