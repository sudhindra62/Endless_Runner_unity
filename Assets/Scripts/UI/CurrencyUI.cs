
using UnityEngine;
using TMPro;
using EndlessRunner.Core;

namespace EndlessRunner.UI
{
    public class CurrencyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currencyText;

        private void OnEnable()
        {
            GameEvents.OnCoinsGained += UpdateCurrencyText;
        }

        private void OnDisable()
        {
            GameEvents.OnCoinsGained -= UpdateCurrencyText;
        }

        private void Start()
        {
            UpdateCurrencyText(0); // Initialize with 0 coins
        }

        public void UpdateCurrencyText(int amount)
        {
            if (currencyText != null)
            {
                currencyText.text = $"Coins: {amount}";
            }
        }
    }
}
