
using UnityEngine;

    public class CurrencyManager : MonoBehaviour
    {
        public static CurrencyManager Instance;

        public int Coins => PlayerDataManager.Instance != null ? PlayerDataManager.Instance.GetCurrency(CurrencyType.Coins) : 0;
        public int Gems => PlayerDataManager.Instance != null ? PlayerDataManager.Instance.GetCurrency(CurrencyType.Gems) : 0;

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

        public void AddCoins(int amount)
        {
            if (PlayerDataManager.Instance != null) PlayerDataManager.Instance.AddCurrency(CurrencyType.Coins, amount);
        }

        public bool SpendCoins(int amount)
        {
            return PlayerDataManager.Instance != null && PlayerDataManager.Instance.SpendCurrency(CurrencyType.Coins, amount);
        }
        
        public void AddGems(int amount)
        {
            if (PlayerDataManager.Instance != null) PlayerDataManager.Instance.AddCurrency(CurrencyType.Gems, amount);
        }

        public bool SpendGems(int amount)
        {
            return PlayerDataManager.Instance != null && PlayerDataManager.Instance.SpendCurrency(CurrencyType.Gems, amount);
        }

        // --- Proxy Methods for Architectural Sync (Folder 3) ---
        public int GetCurrency(CurrencyType type) => PlayerDataManager.Instance != null ? PlayerDataManager.Instance.GetCurrency(type) : 0;
        public void AddCurrency(CurrencyType type, int amount)
        {
            if (PlayerDataManager.Instance != null) PlayerDataManager.Instance.AddCurrency(type, amount);
        }
        public void UpdateCoins(int amount) => AddCoins(amount);
        public int GetTotalCoins() => Coins;

        // --- Fusion PowerUp multiplier API ---
        private float _coinMultiplier = 1f;
        private float _speedMultiplier = 1f;
        private float _multiplierCap = 10f;

        public void SetCoinMultiplier(float multiplier)
        {
            _coinMultiplier = Mathf.Min(multiplier, _multiplierCap);
            Debug.Log($"[CurrencyManager] Coin multiplier set to {_coinMultiplier}x");
        }

        public void SetSpeedBoostMultiplier(float multiplier)
        {
            _speedMultiplier = Mathf.Min(multiplier, _multiplierCap);
        }

        public void SetMultiplierCap(float cap)
        {
            _multiplierCap = cap;
        }

        public float GetCoinMultiplier() => _coinMultiplier;

        public bool IsAffordable(int cost)
        {
            return Coins >= cost;
        }

        public bool IsAffordablePremium(int cost)
        {
            return Gems >= cost;
        }

        public bool CanSpend(CurrencyType type, int amount)
        {
            return GetCurrency(type) >= amount;
        }

        public Sprite GetCurrencyIcon(CurrencyType type)
        {
            // Placeholder: would return appropriate icon sprite
            return null;
        }

        public static event System.Action<int> OnCoinsChanged;
        public static event System.Action<int> OnGemsChanged;

        // --- Type Conversion Bridges (Phase 2A: Type Consistency) ---
        
        public void AddCoins(long amount)
        {
            AddCoins((int)System.Math.Min(amount, int.MaxValue));
        }

        public bool SpendCoins(long amount)
        {
            return SpendCoins((int)System.Math.Min(amount, int.MaxValue));
        }

        public void AddGems(long amount)
        {
            AddGems((int)System.Math.Min(amount, int.MaxValue));
        }

        public bool SpendGems(long amount)
        {
            return SpendGems((int)System.Math.Min(amount, int.MaxValue));
        }

        public void AddCurrency(CurrencyType type, long amount)
        {
            AddCurrency(type, (int)System.Math.Min(amount, int.MaxValue));
        }

        public bool CanSpend(CurrencyType type, long amount)
        {
            return CanSpend(type, (int)System.Math.Min(amount, int.MaxValue));
        }

        public bool IsAffordable(long cost)
        {
            return IsAffordable((int)System.Math.Min(cost, int.MaxValue));
        }

        public bool IsAffordablePremium(long cost)
        {
            return IsAffordablePremium((int)System.Math.Min(cost, int.MaxValue));
        }

        public void SetCoinMultiplier(double multiplier)
        {
            SetCoinMultiplier((float)multiplier);
        }

        public void SetSpeedBoostMultiplier(double multiplier)
        {
            SetSpeedBoostMultiplier((float)multiplier);
        }

        public void SetMultiplierCap(double cap)
        {
            SetMultiplierCap((float)cap);
        }
    }

