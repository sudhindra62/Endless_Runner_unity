using UnityEngine;
using TMPro;

public class CoinBalanceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI balanceText;

    void Start()
    {
        UpdateBalance(CurrencyManager.Instance.Coins);
        //CurrencyManager.OnCoinsChanged += UpdateBalance;
    }

    void OnDestroy()
    {
        //CurrencyManager.OnCoinsChanged -= UpdateBalance;
    }

    private void UpdateBalance(int newBalance)
    {
        balanceText.text = newBalance.ToString();
    }
}
