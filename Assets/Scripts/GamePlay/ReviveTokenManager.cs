using UnityEngine;
using System;

/// <summary>
/// Manages the player's inventory of Revive Tokens.
/// </summary>
public class ReviveTokenManager : MonoBehaviour
{
    public static ReviveTokenManager Instance { get; private set; }

    public static event Action<int> OnTokenCountChanged;

    private int tokenCount;
    private const string TOKEN_COUNT_KEY = "PlayerReviveTokens";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadTokens();
        }
    }

    private void Start()
    {
        // Notify UI of initial count
        OnTokenCountChanged?.Invoke(tokenCount);
    }

    private void LoadTokens()
    {
        tokenCount = PlayerPrefs.GetInt(TOKEN_COUNT_KEY, 0);
    }

    private void SaveTokens()
    {
        PlayerPrefs.SetInt(TOKEN_COUNT_KEY, tokenCount);
        PlayerPrefs.Save();
    }

    public int GetTokenCount() => tokenCount;

    public void AddTokens(int amount)
    {
        if (amount <= 0) return;
        tokenCount += amount;
        SaveTokens();
        OnTokenCountChanged?.Invoke(tokenCount);
    }

    public bool UseToken()
    {
        if (tokenCount <= 0) return false;
        tokenCount--;
        SaveTokens();
        OnTokenCountChanged?.Invoke(tokenCount);
        return true;
    }
}
