
using UnityEngine;

namespace EndlessRunner.Player
{
    public class PlayerCurrency : MonoBehaviour
    {
        public static PlayerCurrency Instance { get; private set; }

        public int Coins { get; private set; }
        public int Gems { get; private set; }

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

            LoadCurrency();
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
            SaveCurrency();
        }

        public void AddGems(int amount)
        {
            Gems += amount;
            SaveCurrency();
        }

        public bool SpendCoins(int amount)
        {
            if (Coins >= amount)
            {
                Coins -= amount;
                SaveCurrency();
                return true;
            }
            return false;
        }

        public bool SpendGems(int amount)
        {
            if (Gems >= amount)
            {
                Gems -= amount;
                SaveCurrency();
                return true;
            }
            return false;
        }

        private void LoadCurrency()
        {
            Coins = PlayerPrefs.GetInt("PlayerCoins", 0);
            Gems = PlayerPrefs.GetInt("PlayerGems", 0);
        }

        private void SaveCurrency()
        {
            PlayerPrefs.SetInt("PlayerCoins", Coins);
            PlayerPrefs.SetInt("PlayerGems", Gems);
            PlayerPrefs.Save();
        }
    }
}
