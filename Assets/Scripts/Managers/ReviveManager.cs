
using UnityEngine;
using System;

public class ReviveManager : MonoBehaviour
{
    public static ReviveManager Instance { get; private set; }

    public static event Action OnPlayerRevived;

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

    public void ReviveWithAd()
    {
        // AdManager.Instance.ShowReviveAd();
        // OnPlayerRevived?.Invoke();
    }

    public void ReviveWithGems(int cost)
    {
        // if (CurrencyManager.Instance.GetGems() >= cost)
        // {
        //     CurrencyManager.Instance.RemoveGems(cost);
        //     OnPlayerRevived?.Invoke();
        // }
    }
}
