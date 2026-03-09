
using System;

namespace Core
{
    /// <summary>
    /// A static class to hold all game-wide events.
    /// </summary>
    public static class GameEvents
    {
        /// <summary>
        /// Event fired when the player collides with an obstacle.
        /// </summary>
        public static event Action OnPlayerDied;

        /// <summary>
        /// Event fired when a coin is collected. The integer value represents the coin's worth.
        /// </summary>
        public static event Action<int> OnCoinCollected;

        // --- Helper methods to invoke events safely ---

        public static void TriggerPlayerDied()
        {
            OnPlayerDied?.Invoke();
        }

        public static void TriggerCoinCollected(int amount)
        {
            OnCoinCollected?.Invoke(amount);
        }
    }
}
