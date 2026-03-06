
using UnityEngine;

public class EventCurrencyManager : Singleton<EventCurrencyManager>
{
    private int eventCurrencyBalance = 0;
    private const float CONVERSION_RATE = 0.5f; // Example: 1 event currency = 0.5 coins

    public void AddEventCurrency(int amount)
    {
        eventCurrencyBalance += amount;
        Debug.Log($"Added {amount} event currency. Total: {eventCurrencyBalance}");
    }

    public bool SpendEventCurrency(int amount)
    {
        if (eventCurrencyBalance >= amount)
        {
            eventCurrencyBalance -= amount;
            Debug.Log($"Spent {amount} event currency. Remaining: {eventCurrencyBalance}");
            return true;
        }
        return false;
    }

    public int GetEventCurrencyBalance()
    {
        return eventCurrencyBalance;
    }

    public void ConvertEventCurrencyToCoins()
    {
        int coinsToAdd = Mathf.FloorToInt(eventCurrencyBalance * CONVERSION_RATE);
        if (coinsToAdd > 0)
        {
            // Assume a PlayerWallet or similar manager exists
            // PlayerWallet.Instance.AddCoins(coinsToAdd);
            Debug.Log($"Converted {eventCurrencyBalance} event currency to {coinsToAdd} coins.");
        }
        eventCurrencyBalance = 0;
    }
}
