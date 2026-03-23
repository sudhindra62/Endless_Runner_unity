
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    public void UpdateDisplay(int amount) { /* UI Logic */ }

    public void UpdateDisplay(int coins, int gems)
    {
        UpdateDisplay(coins);
    }
}
