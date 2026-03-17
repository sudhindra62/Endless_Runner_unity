
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

        public bool IsSubscribed()
        {
            // In a real implementation, this would check with a subscription service.
            // For now, it will return a default value.
            return false;
        }
    }
}
