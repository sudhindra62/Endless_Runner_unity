
using UnityEngine;

namespace EndlessRunner.Managers
{
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

        // Placeholder for checking premium subscription status.
        // In a real app, this would involve checking with a backend or a store API.
        public bool IsSubscribed()
        {
            // For testing purposes, we can use a PlayerPref to toggle subscription.
            // Set "IsPremiumSubscribed" to 1 in PlayerPrefs to simulate a subscription.
            return PlayerPrefs.GetInt("IsPremiumSubscribed", 0) == 1;
        }
    }
}
