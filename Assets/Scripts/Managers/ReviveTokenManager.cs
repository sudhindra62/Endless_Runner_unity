using UnityEngine;

/// <summary>
/// Manages the player's revive tokens.
/// </summary>
public class ReviveTokenManager : MonoBehaviour
{
    [SerializeField] private int _maxReviveTokens = 3;
    private int _reviveTokens;

    public int ReviveTokens => _reviveTokens;

    /// <summary>
    /// Adds revive tokens.
    /// </summary>
    public void AddReviveTokens(int amount)
    {
        if (amount < 0) return;
        _reviveTokens = Mathf.Min(_reviveTokens + amount, _maxReviveTokens);
    }

    /// <summary>
    /// Uses a revive token.
    /// </summary>
    /// <returns>True if a token was used, false otherwise.</returns>
    public bool UseReviveToken()
    {
        if (_reviveTokens > 0)
        {
            _reviveTokens--;
            return true;
        }
        return false;
    }
}
