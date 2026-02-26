using UnityEngine;
using TMPro;

public class GemBalanceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI balanceText;

    void Start()
    {
        UpdateBalance(CurrencyManager.Instance.Gems);
        //CurrencyManager.OnGemsChanged += UpdateBalance;
    }

    void OnDestroy()
    {
        //CurrencyManager.OnGemsChanged -= UpdateBalance;
    }

    private void UpdateBalance(int newBalance)
    {
        balanceText.text = newBalance.ToString();
    }
}
