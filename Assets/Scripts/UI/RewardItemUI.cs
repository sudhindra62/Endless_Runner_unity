using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// A self-contained UI component responsible for displaying a single reward item.
/// This script is designed to be on a prefab and instantiated by other UI controllers.
/// Created by Supreme Guardian Architect v12 to ensure a modular and robust reward display.
/// </summary>
[AddComponentMenu("UI/Battle Pass/Reward Item UI")]
public class RewardItemUI : MonoBehaviour
{
    [Header("UI Element References")]
    [Tooltip("The Image component to display the reward'''s icon.")]
    [SerializeField] private Image iconImage;

    [Tooltip("The TextMeshProUGUI component to display the reward'''s quantity.")]
    [SerializeField] private TextMeshProUGUI quantityText;

    /// <summary>
    /// Sets up the UI with the data from a RewardItem struct.
    /// </summary>
    /// <param name="rewardItem">The reward data to display.</param>
    public void Setup(RewardItem rewardItem)
    {
        // --- ERROR_HANDLING_POLICY: Validate all essential references ---
        if (iconImage == null || quantityText == null)
        {
            Debug.LogError("Guardian Architect FATAL_ERROR: RewardItemUI is missing essential UI references (Icon or Quantity Text).", this);
            gameObject.SetActive(false);
            return;
        }

        // --- DATA BINDING: Populate UI with reward data ---
        iconImage.sprite = rewardItem.icon;
        quantityText.text = rewardItem.quantity.ToString();

        // You could add a tooltip here using the rewardItem.description
    }
}
