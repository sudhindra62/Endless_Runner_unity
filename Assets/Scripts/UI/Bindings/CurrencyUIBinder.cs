using UnityEngine;
using TMPro;

    /// <summary>
    /// Display-only binder for coins & gems UI.
    /// Reacts to DataManager static events.
    /// </summary>
    public class CurrencyUIBinder : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text coinsText;
        [SerializeField] private TMP_Text gemsText;

        private void OnEnable()
        {
            DataManager.OnCoinsChanged += UpdateCoins;
            DataManager.OnGemsChanged += UpdateGems;

            // Initial sync (important on scene load)
            if (DataManager.Instance != null)
            {
                UpdateCoins(DataManager.Instance.Coins);
                UpdateGems(DataManager.Instance.Gems);
            }
        }

        private void OnDisable()
        {
            DataManager.OnCoinsChanged -= UpdateCoins;
            DataManager.OnGemsChanged -= UpdateGems;
        }

        private void UpdateCoins(int value)
        {
            // ADDED: Prevent overwriting in-run score UI
            if (GameStateManager.Instance != null &&
                GameStateManager.Instance.CurrentState == GameStateManager.GameState.Playing)
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
