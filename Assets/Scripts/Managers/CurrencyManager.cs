using System;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    public static event Action<int> OnCoinsChanged;

    public int Coins { get; private set; }

    private bool isCoinDoublerActive = false;

    public void AddCoins(int amount)
    {
        int amountToAdd = isCoinDoublerActive ? amount * 2 : amount;
        Coins += amountToAdd;
        OnCoinsChanged?.Invoke(Coins);
    }

    public void ActivateCoinDoubler(bool isActive)
    {
        isCoinDoublerActive = isActive;
    }
}