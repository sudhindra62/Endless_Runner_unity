
using UnityEngine;

public class ReviveManager : MonoBehaviour
{
    public static ReviveManager Instance { get; private set; }

    public int ReviveTokens { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UseReviveToken()
    {
        if (ReviveTokens > 0)
        {
            ReviveTokens--;
            // Logic to revive the player
        }
    }

    public void AddReviveTokens(int amount)
    {
        ReviveTokens += amount;
    }
}
