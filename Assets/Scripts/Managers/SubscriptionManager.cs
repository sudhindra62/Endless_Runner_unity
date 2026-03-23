
using UnityEngine;

    public class SubscriptionManager : MonoBehaviour
    {
        public static SubscriptionManager Instance;

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

        private const string PREMIUM_STATUS_KEY = "PremiumStatus";
        private const string PREMIUM_EXPIRATION_KEY = "PremiumExpiration";

        public bool IsSubscribed()
        {
            CheckSubscriptionStatus();
            return SaveManager.Instance != null && SaveManager.Instance.Data.isPremiumSubscribed;
        }

        public void CheckSubscriptionStatus()
        {
            if (SaveManager.Instance == null) return;
            
            bool isPremium = SaveManager.Instance.Data.isPremiumSubscribed;
            if (isPremium)
            {
                long expirationTicks = SaveManager.Instance.Data.premiumExpirationTimestamp;
                if (expirationTicks != 0)
                {
                    System.DateTime expirationTime = new System.DateTime(expirationTicks);
                    if (System.DateTime.UtcNow > expirationTime)
                    {
                        SaveManager.Instance.Data.isPremiumSubscribed = false;
                        SaveManager.Instance.SaveGame();
                    }
                }
            }
        }

        public void Subscribe(int days)
        {
            if (SaveManager.Instance == null) return;
            
            System.DateTime expirationTime = System.DateTime.UtcNow.AddDays(days);
            SaveManager.Instance.Data.isPremiumSubscribed = true;
            SaveManager.Instance.Data.premiumExpirationTimestamp = expirationTime.Ticks;
            SaveManager.Instance.SaveGame();
            
            if (ThemeUnlockManager.Instance != null)
            {
                ThemeUnlockManager.Instance.UnlockAllPremiumThemes();
            }
        }
    }

