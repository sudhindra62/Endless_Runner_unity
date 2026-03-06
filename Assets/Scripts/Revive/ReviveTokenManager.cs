
using UnityEngine;
using System;

/// <summary>
/// Manages the player's inventory of Revive Tokens.
/// Handles adding, spending, and querying the token count.
/// </summary>
public class ReviveTokenManager : Singleton<ReviveTokenManager>
{
    public static event Action<int> OnReviveTokensChanged;

    private int tokenCount;
    private const string TOKEN_COUNT_KEY = "ReviveTokenCount";

    protected override void Awake()
    {
        base.Awake();
        LoadTokenCount();
    }

    private void LoadTokenCount()
    {
        tokenCount = PlayerPrefs.GetInt(TOKEN_COUNT_KEY, 0);
    }

    public int GetTokenCount()
    {
        return tokenCount;
    }

    public bool HasEnoughTokens(int amount = 1)
    {
        return tokenCount >= amount;
    }

    public void AddTokens(int amount)
    {
        tokenCount += amount;
        SaveTokenCount();
        OnReviveTokensChanged?.Invoke(tokenCount);
    }

    public bool TrySpendTokens(int amount = 1)
    {
        if (HasEnoughTokens(amount))
        {
            tokenCount -= amount;
            SaveTokenCount();
            OnReviveTokensChanged?.Invoke(tokenCount);
            return true;
        }
        return false;
    }

    private void SaveTokenCount()
    {
        PlayerPrefs.SetInt(TOKEN_COUNT_KEY, tokenCount);
        PlayerPrefs.Save();
    }
}
