using UnityEngine;
using TMPro;

namespace EndlessRunner.UI.Bindings
{
    /// <summary>
    /// Display-only binder for coins & gems UI.
    /// Reacts to CurrencyManager static events.
    /// </summary>
    public class CurrencyUIBinder : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text coinsText;
        [SerializeField] private TMP_Text gemsText;

        private void OnEnable()
        {
            CurrencyManager.OnCoinsChanged += UpdateCoins;
            CurrencyManager.OnGemsChanged += UpdateGems;

            // Initial sync (important on scene load)
            if (CurrencyManager.Instance != null)
            {
                UpdateCoins(CurrencyManager.Instance.Coins);
                UpdateGems(CurrencyManager.Instance.Gems);
            }
        }

        private void OnDisable()
        {
            CurrencyManager.OnCoinsChanged -= UpdateCoins;
            CurrencyManager.OnGemsChanged -= UpdateGems;
        }

        private void UpdateCoins(int value)
        {
            // ADDED: Prevent overwriting in-run score UI
            if (GameStateManager.Instance != null &&
                GameStateManager.Instance.CurrentState == GameState.Playing)
            {
                return;
            }

            if (coinsText != null)
                coinsText.text = value.ToString();
        }

        private void UpdateGems(int value)
        {
            if (gemsText != null)
                gemsText.text = value.ToString();
        }
    }
}
