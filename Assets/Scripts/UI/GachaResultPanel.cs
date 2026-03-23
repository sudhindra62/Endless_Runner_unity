
using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
using TMPro;

/// <summary>
/// Displays the result of a gacha pull to the player.
/// Created by Supreme Guardian Architect v12 to fulfill the A-to-Z Connectivity mandate.
/// </summary>
public class GachaResultPanel : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("The main canvas group for the panel to allow fading.")]
    [SerializeField] private CanvasGroup panelCanvasGroup;

    [Tooltip("The TextMeshProUGUI element to display the name of the pulled item.")]
    [SerializeField] private TextMeshProUGUI itemNameText;

    [Tooltip("The Image element to display the thumbnail of the pulled item.")]
    [SerializeField] private Image itemThumbnailImage;

    [Header("Configuration")]
    [Tooltip("A default sprite to use if the item has no thumbnail URL or if it fails to load.")]
    [SerializeField] private Sprite defaultThumbnail;

    private void Awake()
    {
        // Ensure the panel is hidden on start
        if(panelCanvasGroup != null)
        { 
            panelCanvasGroup.alpha = 0;
            panelCanvasGroup.interactable = false;
            panelCanvasGroup.blocksRaycasts = false;
        }
    }

    /// <summary>
    /// Shows the panel and populates it with the data from the pulled asset.
    /// </summary>
    /// <param name="asset">The LootLocker asset instance that was pulled.</param>
    public void ShowGachaResultPanel(LootLockerCommonAsset asset)
    {
        if (asset == null)
        {
            Debug.LogError("Cannot show gacha result panel for a null asset.");
            return;
        }

        if (itemNameText != null) itemNameText.text = asset.name;
        
        // Asynchronously load the thumbnail image
        if (itemThumbnailImage != null)
        {
            itemThumbnailImage.sprite = defaultThumbnail;
        }

        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 1;
            panelCanvasGroup.interactable = true;
            panelCanvasGroup.blocksRaycasts = true;
        }
    }

    /// <summary>
    /// Hides the panel. Called by a UI button on the panel itself.
    /// </summary>
    public void HidePanel()
    {
        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 0;
            panelCanvasGroup.interactable = false;
            panelCanvasGroup.blocksRaycasts = false;
        }
    }
}
