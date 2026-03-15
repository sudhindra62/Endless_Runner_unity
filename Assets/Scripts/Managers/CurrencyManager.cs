
using UnityEngine;
using EndlessRunner.Core;

namespace EndlessRunner.Managers
{
    public class CurrencyManager : Singleton<CurrencyManager>
    {
        public int Coins { get; private set; }
        public int Gems { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Register(this);
        }

        private void Start()
        {
            LoadCurrency();
        }

        public void AddCoins(int amount)
        {
            if (amount < 0) return;
            Coins += amount;
            GameEvents.TriggerCoinsGained(Coins);
            SaveCurrency();
        }

        public bool SpendCoins(int amount)
        {
            if (amount < 0 || Coins < amount) return false;
            Coins -= amount;
            GameEvents.TriggerCoinsGained(Coins);
            SaveCurrency();
            return true;
        }

        public void AddGems(int amount)
        {
            if (amount < 0) return;
            Gems += amount;
            SaveCurrency();
        }

        public bool TrySpendGems(int amount)
        {
            if (amount < 0 || Gems < amount) return false;
            Gems -= amount;
            SaveCurrency();
            return true;
        }

        public bool HasEnoughGems(int amount)
        {
            return Gems >= amount;
        }

        private void SaveCurrency()
        {
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.Data.coins = Coins;
                SaveManager.Instance.Data.gems = Gems;
                SaveManager.Instance.SaveGame();
            }
        }

        private void LoadCurrency()
        {
            if (SaveManager.Instance != null)
            {
                Coins = SaveManager.Instance.Data.coins;
                Gems = SaveManager.Instance.Data.gems;
                GameEvents.TriggerCoinsGained(Coins);
            }
        }
    }
}
