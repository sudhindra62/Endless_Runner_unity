using UnityEngine;
using TMPro;

/// <summary>
/// Updates a TextMeshProUGUI element to display the player's current gem balance.
/// It subscribes to the CurrencyManager's OnGemsChanged event to stay in sync.
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class GemCounterUI : MonoBehaviour
{
    private TextMeshProUGUI gemText;

    void Awake()
    {
        gemText = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        // Subscribe to the event to get notified of changes
        CurrencyManager.OnGemsChanged += UpdateGemText;

        // Also, set the initial value when the UI becomes active
        if (CurrencyManager.Instance != null)
        {
            UpdateGemText(CurrencyManager.Instance.Gems);
        }
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        CurrencyManager.OnGemsChanged -= UpdateGemText;
    }

    /// <summary>
    /// Callback function to update the text with the new gem count.
    /// </summary>
    /// <param name="newGemCount">The player's new gem balance.</param>
    private void UpdateGemText(int newGemCount)
    {
        if (gemText != null)
        {
            gemText.text = newGemCount.ToString();
        }
    }
}
