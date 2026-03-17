
using UnityEngine;

namespace EndlessRunner.Managers
{
    public class CurrencyManager : MonoBehaviour
    {
        public static CurrencyManager Instance;

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

            Coins = PlayerPrefs.GetInt("Coins", 0);
            Gems = PlayerPrefs.GetInt("Gems", 0);
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
            PlayerPrefs.SetInt("Coins", Coins);
            PlayerPrefs.Save();
        }

        public bool SpendCoins(int amount)
        {
            if (Coins >= amount)
            {
                Coins -= amount;
                PlayerPrefs.SetInt("Coins", Coins);
                PlayerPrefs.Save();
                return true;
            }
            return false;
        }
        
        public void AddGems(int amount)
        {
            Gems += amount;
            PlayerPrefs.SetInt("Gems", Gems);
            PlayerPrefs.Save();
        }

        public bool SpendGems(int amount)
        {
            if (Gems >= amount)
            {
                Gems -= amount;
                PlayerPrefs.SetInt("Gems", Gems);
                PlayerPrefs.Save();
                return true;
            }
            return false;
        }
    }
}
