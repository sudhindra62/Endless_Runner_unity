
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A visual component that displays an achievement'''s tier (e.g., Bronze, Gold) as a colored badge.
/// Logic fortified by Supreme Guardian Architect v12 to ensure robust visual feedback and Inspector-driven configuration.
/// This component is designed to be a "dumb" view, controlled by a higher-level UI controller like AchievementUIController.
/// </summary>
[AddComponentMenu("UI/Achievements/Achievement Badge")] // Makes it easier to find in the Add Component menu
public class AchievementBadge : MonoBehaviour
{
    // --- ENUMERATION: Defines the visual ranking of an achievement ---
    // This enum is now nested for better encapsulation, per Guardian protocol.
    public enum AchievementTier
    {
        Bronze,
        Silver,
        Gold,
        Diamond
    }

    [Header("UI References")]
    [Tooltip("The Image component whose color will be changed to reflect the achievement tier.")]
    [SerializeField] private Image badgeImage;

    [Header("Tier Color Configuration")]
    [Tooltip("The color representing the Bronze tier.")]
    [SerializeField] private Color bronzeColor = new Color(0.8f, 0.5f, 0.2f);
    
    [Tooltip("The color representing the Silver tier.")]
    [SerializeField] private Color silverColor = new Color(0.75f, 0.75f, 0.75f);
    
    [Tooltip("The color representing the Gold tier.")]
    [SerializeField] private Color goldColor = new Color(1f, 0.84f, 0f);
    
    [Tooltip("The color representing the Diamond tier.")]
    [SerializeField] private Color diamondColor = new Color(0.7f, 0.95f, 1f);

    [Header("Editor Preview")]
    [Tooltip("Set this in the Inspector to preview the badge color during design time.")]
    [SerializeField] private AchievementTier previewTier;

    /// <summary>
    /// Guardian Architect's Sanity Check: This method runs in the editor whenever the script is loaded or a value is changed in the Inspector.
    /// It allows for immediate visual feedback without entering Play Mode.
    /// </summary>
    private void OnValidate()
    {
        // Auto-find the Image component on this GameObject if it'''s not assigned.
        if (badgeImage == null)
        {
            badgeImage = GetComponent<Image>();
        }
        SetTier(previewTier);
    }
    
    /// <summary>
    /// Sets the visual tier of the badge, updating its color accordingly.
    /// This is the primary public API for this component.
    /// </summary>
    /// <param name="tier">The achievement tier to display.</param>
    public void SetTier(AchievementTier tier)
    {
        // --- ERROR_HANDLING_POLICY: Adherence to mandatory null-check protocol ---
        if (badgeImage == null)
        {
            // Error is logged only if called at runtime, not during initial OnValidate discovery.
            if (Application.isPlaying)
            {
                 Debug.LogError("Guardian Architect FATAL_ERROR: 'badgeImage' is not assigned on AchievementBadge. Cannot set tier color. Please assign the reference in the Inspector.", this.gameObject);
            }
            return;
        }

        // --- CORE LOGIC: Apply the color based on the provided tier ---
        switch (tier)
        {
            case AchievementTier.Bronze:
                badgeImage.color = bronzeColor;
                break;
            case AchievementTier.Silver:
                badgeImage.color = silverColor;
                break;
            case AchievementTier.Gold:
                badgeImage.color = goldColor;
                break;
            case AchievementTier.Diamond:
                badgeImage.color = diamondColor;
                break;
            default:
                // In case a new tier is added to the enum but not this switch statement.
                Debug.LogWarning($"Guardian Architect Warning: Unhandled AchievementTier '{tier}'. Defaulting badge color.", this.gameObject);
                badgeImage.color = Color.white; // Default fallback
                break;
        }
    }
}
