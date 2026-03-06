
using System;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Manages all player currencies (Coins, Gems, etc.) and transactions.
    /// Provides a centralized point for adding, spending, and querying currency balances.
    /// Integrates with the SaveManager to persist currency data.
    /// </summary>
    public class CurrencyManager : MonoBehaviour
    {
        public static CurrencyManager Instance { get; private set; }

        public event Action<int> OnCoinsChanged;
        public event Action<int> OnGemsChanged;

        private int _coins;
        private int _gems;

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
                return;
            }

            LoadCurrency();
        }

        public int GetCoins() => _coins;

        public void AddCoins(int amount)
        {
            if (amount <= 0) return;
            _coins += amount;
            OnCoinsChanged?.Invoke(_coins);
            // NOTE: We will let the SaveManager handle saving periodically.
        }

        public bool SpendCoins(int amount)
        {
            if (amount <= 0 || _coins < amount)
            {
                return false;
            }
            _coins -= amount;
            OnCoinsChanged?.Invoke(_coins);
            return true;
        }
        
        public int GetGems() => _gems;

        public void AddGems(int amount)
        {
            if (amount <= 0) return;
            _gems += amount;
            OnGemsChanged?.Invoke(_gems);
        }

        public bool SpendGems(int amount)
        {
            if (amount <= 0 || _gems < amount)
            {
                return false;
            }
            _gems -= amount;
            OnGemsChanged?.Invoke(_gems);
            return true;
        }

        public void LoadCurrencyFromSaveData(GameData data)
        {
            _coins = data.playerCoins;
            _gems = data.playerGems;
            OnCoinsChanged?.Invoke(_coins);
            OnGemsChanged?.Invoke(_gems);
        }
        
        public void PopulateSaveData(GameData data)
        {
            data.playerCoins = _coins;
            data.playerGems = _gems;
        }
        
        private void LoadCurrency()
        {
            // Initial load will be handled by the DataManager/SaveManager
            // This ensures all data is loaded in the correct order.
            if (DataManager.Instance != null)
            {
                LoadCurrencyFromSaveData(DataManager.Instance.GameData);
            }
            else
            {
                _coins = 0;
                _gems = 0;
            }
        }
    }
}
