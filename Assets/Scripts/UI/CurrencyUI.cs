
using UnityEngine;
using TMPro;

/// <summary>
/// Displays the player's currency (coins and gems) and updates it in real-time.
/// </summary>
public class CurrencyUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text gemsText;

    private void OnEnable()
    {
        CurrencyManager.OnCoinsChanged += UpdateCoinsDisplay;
        CurrencyManager.OnGemsChanged += UpdateGemsDisplay;

        // Initial display update
        if (CurrencyManager.Instance != null)
        {
            UpdateCoinsDisplay(CurrencyManager.Instance.Coins);
            UpdateGemsDisplay(CurrencyManager.Instance.Gems);
        }
    }

    private void OnDisable()
    {
        CurrencyManager.OnCoinsChanged -= UpdateCoinsDisplay;
        CurrencyManager.OnGemsChanged -= UpdateGemsDisplay;
    }

    private void UpdateCoinsDisplay(int newAmount)
    {
        if (coinsText != null)
        {
            coinsText.text = newAmount.ToString();
        }
    }

    private void UpdateGemsDisplay(int newAmount)
    {
        if (gemsText != null)
        {
            gemsText.text = newAmount.ToString();
        }
    }
}
