
using UnityEngine;

namespace EndlessRunner.Managers
{
    public class CurrencyManager : MonoBehaviour
    {
        public static CurrencyManager Instance;

        public int CurrentCoins { get; private set; }

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

            CurrentCoins = PlayerPrefs.GetInt("Coins", 0);
        }

        public void AddCoins(int amount)
        {
            CurrentCoins += amount;
            PlayerPrefs.SetInt("Coins", CurrentCoins);
            PlayerPrefs.Save();
        }

        public void RemoveCoins(int amount)
        {
            CurrentCoins -= amount;
            PlayerPrefs.SetInt("Coins", CurrentCoins);
            PlayerPrefs.Save();
        }
    }
}
